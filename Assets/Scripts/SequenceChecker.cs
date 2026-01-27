using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SequenceChecker : MonoBehaviour
{
    [SerializeField] private List<string> correctSequence = new List<string> 
    { "Do", "Mi", "Fa", "Sol", "Fa", "Mi", "Re", "Do" };
    
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private float dialogDisplayTime = 5f;
    
    [SerializeField] private string mistakeMessage = "Django (street performer) : \"Don't worry you can try again you got this!\"";
    [SerializeField] private string successMessage = "Django (street performer) : \"Well done, I recognize a talented musician when i see one\"";
    
    [SerializeField] private AudioSource djangoMusicSource;
    [SerializeField] private GameObject cubeNotes;
    [SerializeField] private GameObject instructionPanel;
    
    private List<string> playerSequence = new List<string>();
    private bool puzzleSolved = false;
    private bool isShowingDialog = false;
    
    public void NotePressed(string noteName)
    {
        if (puzzleSolved || isShowingDialog) return;
        
        playerSequence.Add(noteName);
        Debug.Log("Current sequence: " + string.Join(", ", playerSequence));
        
        if (!IsSequenceCorrectSoFar())
        {
            Debug.Log("Wrong note! Sequence reset.");
            playerSequence.Clear();
            ShowDialog(mistakeMessage);
            return;
        }
        
        if (playerSequence.Count == correctSequence.Count)
        {
            Debug.Log("Puzzle solved! Correct sequence completed!");
            puzzleSolved = true;
            ShowDialog(successMessage);
            StartCoroutine(RestartMusicAfterDialog());
        }
    }
    
    private bool IsSequenceCorrectSoFar()
    {
        for (int i = 0; i < playerSequence.Count; i++)
        {
            if (playerSequence[i] != correctSequence[i])
            {
                return false;
            }
        }
        return true;
    }
    
    private void ShowDialog(string message)
    {
        if (dialogPanel != null && dialogText != null)
        {
            dialogText.text = message;
            dialogPanel.SetActive(true);
            StartCoroutine(HideDialogAfterDelay());
        }
    }
    
    private IEnumerator HideDialogAfterDelay()
    {
        isShowingDialog = true;
        yield return new WaitForSeconds(dialogDisplayTime);
        
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(false);
        }
        isShowingDialog = false;
    }
    
    private IEnumerator RestartMusicAfterDialog()
    {
        // Wait for the dialog to finish (5 seconds total)
        yield return new WaitForSeconds(5f);
        
        if (djangoMusicSource != null)
        {
            djangoMusicSource.volume = 0.5f;
            
            if (!djangoMusicSource.isPlaying)
            {
                djangoMusicSource.Play();
            }
            
            Debug.Log("Django's music resumed at half volume");
        }
        
        // Hide the cube notes after success
        if (cubeNotes != null)
        {
            cubeNotes.SetActive(false);
            Debug.Log("Cube notes hidden");
        }
        
        // Hide the instruction panel
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(false);
            Debug.Log("Instruction panel hidden");
        }
    }
    
    public void ResetSequence()
    {
        playerSequence.Clear();
        puzzleSolved = false;
        Debug.Log("Sequence reset");
    }
}