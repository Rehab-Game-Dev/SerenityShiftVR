using UnityEngine;
using TMPro;
using System.Collections;
public class GoalZoneTrigger : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI goalMessage;
    public GameObject instructionPanel;
    
    [Header("Settings")]
    public float displayDuration = 5f;
    
    private bool hasTriggered = false;
    private TargetPulse pulseScript;
    private ParticleSystem particles;
    
    private void Start()
    {
        if (goalMessage != null)
        {
            goalMessage.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Goal Message is not assigned in GoalZoneTrigger!");
        }
        
        pulseScript = GetComponent<TargetPulse>();
        particles = GetComponentInChildren<ParticleSystem>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            TimerManager timer = FindFirstObjectByType<TimerManager>();
            if (timer != null) timer.StopTimer();

            StopEffects();
            StartCoroutine(ShowGoalMessage());
        }
    }
    
    private void StopEffects()
    {
        if (pulseScript != null)
        {
            pulseScript.enabled = false;
        }
        
        if (particles != null)
        {
            particles.Stop();
        }
    }
    
    private IEnumerator ShowGoalMessage()
    {
        if (goalMessage != null)
        {
            TimerManager timer = FindFirstObjectByType<TimerManager>();
            string timeString = timer != null ? timer.GetFormattedTime() : "";
            goalMessage.text = "You reached the goal!\n" + timeString;
            goalMessage.gameObject.SetActive(true);
            Debug.Log("You reached the goal!");
            
            yield return new WaitForSeconds(displayDuration);
            
            goalMessage.gameObject.SetActive(false);
            
            if (instructionPanel != null)
            {
                instructionPanel.SetActive(false);
                Debug.Log("Instruction panel hidden");
            }
        }
    }
    
    public void ResetTrigger()
    {
        hasTriggered = false;
        
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