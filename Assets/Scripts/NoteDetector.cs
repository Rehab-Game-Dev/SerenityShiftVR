using UnityEngine;

public class NoteDetector : MonoBehaviour
{
    public string noteName;
    public AudioClip noteSound;  // Add this line
    
    private AudioSource audioSource;  // Add this line
    
    private void Start()
    {
        // Add AudioSource component if it doesn't exist
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(noteName + " pressed");
            
            // Play the sound
            if (noteSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(noteSound);
            }
        }
    }
}