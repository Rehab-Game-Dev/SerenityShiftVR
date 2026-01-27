using UnityEngine;

public class NoteDetector : MonoBehaviour
{
    public string noteName;
    public AudioClip noteSound;
    
    private AudioSource audioSource;
    private SequenceChecker sequenceChecker; // Add this
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Find the SequenceChecker in the scene
        sequenceChecker = FindObjectOfType<SequenceChecker>();
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
            
            // Register the note with the sequence checker
            if (sequenceChecker != null)
            {
                sequenceChecker.NotePressed(noteName);
            }
        }
    }
}