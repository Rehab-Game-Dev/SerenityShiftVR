using UnityEngine;
using TMPro;
using System.Collections;

public class StreetPerformerDetection : MonoBehaviour
{
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private ParticleSystem musicParticles;
    [SerializeField] private GameObject cubeNotes;
    [SerializeField] private float fadeOutDuration = 2f;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private string newInstructionMessage = "Walk on the colored notes to play the sequence: red, blue, green, purple, green, blue, yellow, red";
    [SerializeField] private float dialogDisplayTime = 10f; // Time before dialogue disappears
    
    private AudioSource musicAudioSource;
    private bool missionStarted = false;
    
    private void Start()
    {
        musicAudioSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !missionStarted)
        {
            Debug.Log("Found the street performer");
            
            missionStarted = true;
            
            if (dialogPanel != null)
            {
                dialogPanel.SetActive(true);
                StartCoroutine(HideDialogAfterDelay());
            }
            
            // Update the instruction text
            if (instructionText != null)
            {
                instructionText.text = newInstructionMessage;
            }
            
            // Fade out the music instead of stopping immediately
            if (musicAudioSource != null)
            {
                StartCoroutine(FadeOutMusic());
            }
            
            if (musicParticles != null)
            {
                musicParticles.Stop();
            }
            
            startMissionTwo();
        }
    }
    
    private IEnumerator HideDialogAfterDelay()
    {
        yield return new WaitForSeconds(dialogDisplayTime);
        
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(false);
        }
    }
    
    private IEnumerator FadeOutMusic()
    {
        float startVolume = musicAudioSource.volume;
        float elapsed = 0f;
        
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeOutDuration);
            yield return null;
        }
        
        musicAudioSource.Stop();
        musicAudioSource.volume = startVolume;
    }
    
    private void startMissionTwo()
    {
        Debug.Log("Mission Two Started");
        
        if (cubeNotes != null)
        {
            cubeNotes.SetActive(true);
        }
    }
}