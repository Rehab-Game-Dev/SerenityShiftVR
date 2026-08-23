using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class BirdCatchable : MonoBehaviour
{
    public bool hasBeenCaught = false;

    [Header("Sound Effect")]
    public AudioClip catchSound;
    private AudioSource audioSource;

    private XRSimpleInteractable interactable;

    private void Awake()
    {
        // מתחבר אוטומטית לאירוע הבחירה — עובד גם על ציפורים מה-spawner
        interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnSelected);
    }

    private void OnDestroy()
    {
        // ניקוי המאזין כדי למנוע דליפות
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnSelected);
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        CatchBird();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    public void CatchBird()
    {
        if (hasBeenCaught) return;
        hasBeenCaught = true;

        // צליל בעמדת השחקן
        if (catchSound != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 pos = player != null ? player.transform.position : transform.position;
            AudioSource.PlayClipAtPoint(catchSound, pos);
        }

        // עדכוני flow — עטופים כדי שקריסה לא תמנע את ההשמדה
        try
        {
            if (GoalMessageController.Instance != null)
                GoalMessageController.Instance.OnBirdCaught();
            else if (BirdMessageController.Instance != null)
                BirdMessageController.Instance.OnBirdCaught();

            if (BirdGameManager.Instance != null)
                BirdGameManager.Instance.BirdCaught();
            else
                Debug.LogError("BirdGameManager.Instance is NULL!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[BirdCatchable] flow crashed: {e.Message}\n{e.StackTrace}");
        }

        // שחרר את ה-interactor מיד ואז מחק — כך שאר הציפורים נשארות ניתנות לתפיסה
        if (interactable != null) interactable.enabled = false;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 0.05f);
    }
}