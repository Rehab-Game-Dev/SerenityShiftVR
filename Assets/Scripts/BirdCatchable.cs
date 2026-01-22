using UnityEngine;

public class BirdCatchable : MonoBehaviour
{
    public bool hasBeenCaught = false;
    
    [Header("Sound Effect")]
    public AudioClip catchSound;
    private AudioSource audioSource;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }
    
    public void CatchBird()
    {
        if (hasBeenCaught) return;
        
        hasBeenCaught = true;
        
        // Play catch sound
        if (catchSound != null)
        {
            AudioSource.PlayClipAtPoint(catchSound, transform.position);
        }
        
        // Try GoalMessageController first (for demo/hybrid levels)
        if (GoalMessageController.Instance != null)
        {
            GoalMessageController.Instance.OnBirdCaught();
        }
        // Fall back to BirdMessageController (for medium level)
        else if (BirdMessageController.Instance != null)
        {
            BirdMessageController.Instance.OnBirdCaught();
        }
        
        if (BirdGameManager.Instance != null)
        {
            BirdGameManager.Instance.BirdCaught();
        }
        else
        {
            Debug.LogError("BirdGameManager.Instance is NULL!");
        }
        
        Destroy(gameObject);
    }
}