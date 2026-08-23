using UnityEngine;
using UnityEngine.XR;

public class VRWand : MonoBehaviour
{
    [Header("Settings")]
    public XRNode controllerNode = XRNode.RightHand; // איזו יד? ימין או שמאל
    public float range = 100f;                        // מרחק הקרן

    [Tooltip("להשאיר Everything בזמן דיבוג כדי לפגוע בכל ה-layers. " +
             "אחרי שהקאץ' עובד, לסמן כאן רק את ה-layer של הציפורים/NPCs.")]
    public LayerMask hitLayers = ~0;                  // ברירת מחדל: הכל (~0), לא Nothing

    [Header("Visuals")]
    public GameObject hitEffect;                      // אפקט פגיעה (אופציונלי)

    [Header("Debug")]
    public bool debugLogging = true;                  // הדפסות + ציור הקרן ב-Scene view

    private bool isTriggerPressed = false;
    private bool wasPressedLastFrame = false;         // מונע ירי רציף כשמחזיקים

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);

        // אם השלט לא נמצא/לא tracked — נדע מיד במקום "לחיצה שלא עושה כלום"
        if (!device.isValid)
        {
            if (debugLogging && Time.frameCount % 120 == 0)
                Debug.LogWarning($"[VRWand] No valid XR device at {controllerNode}. " +
                                 "בדוק שה-controllerNode נכון ושהשלט מזוהה.");
            return;
        }

        device.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPressed);

        // רק ברגע הלחיצה (Down), לא בהחזקה
        if (isTriggerPressed && !wasPressedLastFrame)
        {
            ShootRay();
        }
        wasPressedLastFrame = isTriggerPressed;
    }

    void ShootRay()
    {
        RaycastHit hit;
        bool didHit = Physics.Raycast(transform.position, transform.forward,
                                      out hit, range, hitLayers, QueryTriggerInteraction.Ignore);

        if (debugLogging)
        {
            Debug.DrawRay(transform.position, transform.forward * range,
                          didHit ? Color.green : Color.red, 2f);

            if (didHit)
                Debug.Log($"[VRWand] HIT '{hit.transform.name}' " +
                          $"on layer '{LayerMask.LayerToName(hit.transform.gameObject.layer)}'");
            else
                Debug.Log("[VRWand] Ray hit NOTHING. " +
                          "אם גם עם hitLayers=Everything אין פגיעה — בעיית כיוון/קוליידר, לא layer.");
        }

        if (!didHit) return;

        Transform hitTransform = hit.transform;

        // קודם ננסה לקבל את הרכיב הלוגי ישירות מההיררכיה של מה שפגענו בו
        BirdCatchable bird = hitTransform.GetComponentInParent<BirdCatchable>();
        if (bird != null)
        {
            // CatchBird() אחראי בעצמו לבדוק אם כבר נתפסה — הידע הזה שייך לציפור, לא ל-wand
            if (debugLogging) Debug.Log($"[VRWand] Catching bird '{bird.name}'");
            bird.CatchBird();
            SpawnHitEffect(bird.transform.position);
            return;
        }

        // fallback: זיהוי לפי tag/שם עד ל-root של הפריפאב
        Transform birdRoot = GetBirdRoot(hitTransform);
        if (birdRoot != null)
        {
            CatchBirdFallback(birdRoot.gameObject);
            return;
        }

        // אם זו לא ציפור — אולי זה NPC
        NPCCollision npc = hitTransform.GetComponentInParent<NPCCollision>();
        if (npc != null && npc.isCatchable)
        {
            npc.CatchNPC();
            SpawnHitEffect(hitTransform.position);
        }
        else if (debugLogging)
        {
            Debug.Log($"[VRWand] פגעתי ב-'{hitTransform.name}' אבל אין עליו " +
                      "BirdCatchable/NPCCollision באף הורה. כנראה קוליידר לא נכון או prefab root.");
        }
    }

    private Transform GetBirdRoot(Transform t)
    {
        Transform current = t;
        Transform root = null;
        while (current != null)
        {
            if (current.CompareTag("Bird") || current.CompareTag("lb_bird") ||
                current.name.ToLower().Contains("cardinal"))
            {
                root = current;
            }
            current = current.parent;
        }
        return root;
    }

    void CatchBirdFallback(GameObject bird)
    {
        if (debugLogging) Debug.Log("[VRWand] Caught bird (fallback): " + bird.name);

        if (bird.name.ToLower().Contains("cardinal") ||
            bird.CompareTag("Bird") || bird.CompareTag("lb_bird"))
        {
            if (GoalMessageController.Instance != null)
                GoalMessageController.Instance.OnBirdCaught();
            else
                Debug.LogWarning("[VRWand] נתפסה ציפור-מטרה ב-fallback אבל " +
                                 "GoalMessageController.Instance == null!");
        }

        SpawnHitEffect(bird.transform.position);
        Destroy(bird);
    }

    private void SpawnHitEffect(Vector3 pos)
    {
        if (hitEffect != null) Instantiate(hitEffect, pos, Quaternion.identity);
    }
}