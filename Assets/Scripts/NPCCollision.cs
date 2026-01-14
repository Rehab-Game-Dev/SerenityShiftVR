using UnityEngine;

public class NPCCollision : MonoBehaviour
{
    // Determines if this NPC can be caught by the player at the current time
    public bool isCatchable = false;
    
    // Internal flag to prevent multiple trigger events for the same object
    private bool hasBeenCaught = false;
    
    [Header("Sound Effect")]
    public AudioClip catchSound;
    private AudioSource audioSource;
    
    private void Start()
    {
        // Initialization: Ensure an AudioSource component exists for sound playback
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configure audio settings: Play on demand and set to 2D for consistent volume
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; 
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Exit if the NPC has already been processed to avoid redundant logic
        if (hasBeenCaught) return;
        
        // Check for collision with the Player's hands/camera and verify catchable state
        if (other.CompareTag("Player") && isCatchable)
        {
            hasBeenCaught = true;
            
            // VR Feedback: Play audio at the world position for spatial immersion
            if (catchSound != null)
            {
                AudioSource.PlayClipAtPoint(catchSound, transform.position);
            }
            
            // Visual Feedback: Notify the UI system to show success messages
            if (GoalMessageController.Instance != null)
            {
                GoalMessageController.Instance.OnNPCCaught();
            }

            // Progression Logic: Report to the GameManager and check mission status
            if (GameManager.Instance != null)
            {
                GameManager.Instance.NPCCaught();
                
                // Mission Transition: If target count is met, trigger the next mission phase
                if (GameManager.Instance.caughtCount >= 3) 
                {
                    MissionManager mm = FindObjectOfType<MissionManager>();
                    if (mm != null)
                    {
                        // Transition to Mission 2 (Birds Mission)
                        mm.SetMission(2); 
                    }
                }
            }
            else
            {
                // Error Handling: Crucial for debugging complex VR scenes
                Debug.LogError("GameManager.Instance is NULL!");
            }
            
            // Cleanup: Remove the NPC from the scene after successful interaction
            Destroy(gameObject);
        }
    }
}