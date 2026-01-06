using UnityEngine;

public class BirdCatchable : MonoBehaviour
{
    public bool hasBeenCaught = false;
    
    [Header("Sound Effect")]
    public AudioClip catchSound;
    private AudioSource audioSource;
    
    private void Start()
    {
        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configure AudioSource for sound effects
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D sound
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
        
        // Show the bird caught message
        if (BirdMessageController.Instance != null)
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