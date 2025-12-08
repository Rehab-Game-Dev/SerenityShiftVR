using UnityEngine;
using TMPro;

public class FloatingMessageUI : MonoBehaviour
{
    [Header("References")]
    public Transform targetCamera;          // ה-Camera של ה-PC או ה-XR
    public TextMeshProUGUI messageText;     // ה-TMP בתוך הקנבס
    public CanvasGroup canvasGroup;         // על הקנבס

    [Header("Placement")]
    public float distance = 1.5f;           // כמה מטרים מול המשתמש
    public Vector3 offset = new Vector3(0f, -0.15f, 0f); // קצת מתחת למרכז

    [Header("Fade")]
    public float fadeSpeed = 8f;

    float desiredAlpha = 0f;

    void Reset()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        messageText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        if (!targetCamera) return;

        // מיקום מול המצלמה
        transform.position = targetCamera.position + targetCamera.forward * distance + targetCamera.TransformVector(offset);

        // שהקנבס יפנה למצלמה
        Vector3 lookDir = targetCamera.position - transform.position; // <-- הפוך
        transform.rotation = Quaternion.LookRotation(lookDir);


        // Fade
        if (canvasGroup)
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, desiredAlpha, Time.deltaTime * fadeSpeed);
    }

    public void Show(string text)
    {
        if (messageText) messageText.text = text;
        desiredAlpha = 1f;
        if (canvasGroup) canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        desiredAlpha = 0f;
        if (canvasGroup) canvasGroup.blocksRaycasts = false;
    }
}
