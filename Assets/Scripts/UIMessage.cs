using UnityEngine;
using TMPro;
using System.Collections;

public class UIMessage : MonoBehaviour
{
    [Header("References")]
    public CanvasGroup canvasGroup;
    public TMP_Text messageText;

    [Header("Settings")]
    public float showTime = 5f;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    public void ShowMessage(string msg = null)
    {
        if (msg != null && messageText != null)
            messageText.text = msg;

        Debug.Log("ShowMessage Called");  // בדיקה שהפונקציה מופעלת

        StopAllCoroutines();
        StartCoroutine(MessageRoutine());
    }

    private IEnumerator MessageRoutine()
    {
        Debug.Log("Start Fade IN");

        // Fade In
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        Debug.Log("Shown. Waiting...");

        // מחכים X שניות
        yield return new WaitForSeconds(showTime);

        Debug.Log("Start Fade OUT");

        // Fade Out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;

        Debug.Log("Message Hidden");
    }
}
