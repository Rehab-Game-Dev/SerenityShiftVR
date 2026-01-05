using UnityEngine;
using TMPro;
using System.Collections;

public class GoalZoneTrigger : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI goalMessage;
    
    [Header("Settings")]
    public float displayDuration = 5f;
    
    private bool hasTriggered = false;

    private void Start()
    {
        // Make sure the message is hidden at start
        if (goalMessage != null)
        {
            goalMessage.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Goal Message is not assigned in GoalZoneTrigger!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if player entered the target zone
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(ShowGoalMessage());
        }
    }

    private IEnumerator ShowGoalMessage()
    {
        if (goalMessage != null)
        {
            // Show the message
            goalMessage.gameObject.SetActive(true);
            Debug.Log("You reached the goal!");
            
            // Wait for duration
            yield return new WaitForSeconds(displayDuration);
            
            // Hide the message
            goalMessage.gameObject.SetActive(false);
            
            // Optional: Reset the trigger if you want it to work multiple times
            // hasTriggered = false;
        }
    }
    
    // Optional: If you want to manually reset the trigger
    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}