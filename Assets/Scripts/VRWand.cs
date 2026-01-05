using UnityEngine;
using UnityEngine.XR; // ספרייה חובה לשימוש בבקרים

public class VRWand : MonoBehaviour
{
    [Header("Settings")]
    public XRNode controllerNode = XRNode.RightHand; // איזו יד זו? ימין או שמאל
    public float range = 100f; // מרחק הקרן
    public LayerMask birdLayer; // כדי שנפגע רק בציפורים (אופציונלי)

    [Header("Visuals")]
    public GameObject hitEffect; // אפקט פגיעה (אופציונלי)

    private bool isTriggerPressed = false;
    private bool wasPressedLastFrame = false; // כדי למנוע ירי רציף

    void Update()
    {
        // 1. בדיקת הקלט מהבקר (האם לחצו על הטריגר?)
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        device.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPressed);

        // אנחנו רוצים שזה יקרה רק ברגע הלחיצה (Down), לא כשהכפתור מוחזק
        if (isTriggerPressed && !wasPressedLastFrame)
        {
            ShootRay();
        }

        wasPressedLastFrame = isTriggerPressed;
    }

    void ShootRay()
    {
        RaycastHit hit;
        // הקרן יוצאת מהמיקום של האובייקט עליו יושב הסקריפט (היד)
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            // בדיקה האם פגענו בציפור
            // אופציה א: לפי תגית (כמו שעשינו קודם)
            if (hit.transform.CompareTag("Bird")) 
            {
                CatchBird(hit.transform.gameObject);
            }
            // אופציה ב: לפי שם (למקרה ששכחת לשים תגית)
            else if (hit.transform.name.Contains("cardinal") || hit.transform.name.Contains("Bird"))
            {
                CatchBird(hit.transform.gameObject);
            }
        }
    }

    void CatchBird(GameObject bird)
    {
        Debug.Log("תפסתי ציפור! " + bird.name);
        
        // כאן אפשר להוסיף אפקט לפני המחיקה
        if (hitEffect != null)
        {
            Instantiate(hitEffect, bird.transform.position, Quaternion.identity);
        }

        // השמדת הציפור
        Destroy(bird);
    }
}