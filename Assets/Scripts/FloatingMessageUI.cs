using UnityEngine;
using TMPro;

public class FloatingMessageUI : MonoBehaviour
{
    [Header("References")]
    public Transform targetCamera;          // is the camera of the user or the scene?
    public TextMeshProUGUI messageText;     // the TMP inside the canvas
    public CanvasGroup canvasGroup;         // on the canvas

    [Header("Placement")]
    public float distance = 1.5f;           // how many meters in front of the user
    public Vector3 offset = new Vector3(0f, -0.15f, 0f); // slightly below the center
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

        // Position in front of the camera
        transform.position = targetCamera.position + targetCamera.forward * distance + targetCamera.TransformVector(offset);

        // Make the canvas face the camera
        Vector3 lookDir = targetCamera.position - transform.position; // <-- reversed
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
