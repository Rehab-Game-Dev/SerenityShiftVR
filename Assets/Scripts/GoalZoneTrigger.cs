using UnityEngine;
using TMPro;
using System.Collections;

public class GoalZoneTrigger : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI goalMessage;
    public GameObject instructionPanel; // Add this
    
    [Header("Settings")]
    public float displayDuration = 5f;
    
    private bool hasTriggered = false;
    private TargetPulse pulseScript;
    private ParticleSystem particles;
    
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
        
        // Get references to the pulse script and particle system
        pulseScript = GetComponent<TargetPulse>();
        particles = GetComponentInChildren<ParticleSystem>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if player entered the target zone
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StopEffects();
            StartCoroutine(ShowGoalMessage());
        }
    }
    
    private void StopEffects()
    {
        // Stop the pulsing animation
        if (pulseScript != null)
        {
            pulseScript.enabled = false;
        }
        
        // Stop the particle system
        if (particles != null)
        {
            particles.Stop();
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
            
            // Hide the instruction panel
            if (instructionPanel != null)
            {
                instructionPanel.SetActive(false);
                Debug.Log("Instruction panel hidden");
            }
            
            // Optional: Reset the trigger if you want it to work multiple times
            // hasTriggered = false;
        }
    }
    
    // Optional: manually reset the trigger
    public void ResetTrigger()
    {
        hasTriggered = false;
        
        // Restart effects
        if (pulseScript != null)
        {
            pulseScript.enabled = true;
        }
        
        if (particles != null)
        {
            particles.Play();
        }
    }
}