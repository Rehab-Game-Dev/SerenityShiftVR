using UnityEngine;
using System.Collections;

public class TargetZoneTrigger : MonoBehaviour
{
    public GameObject goalMessage;
    public GameObject instructionMSG;
    public float displayTime = 5f; // You can adjust this in the Inspector
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the target zone!");
            
            // Hide instruction message immediately
            if (instructionMSG != null)
            {
                instructionMSG.SetActive(false);
                Debug.Log("Instruction message hidden!");
            }
            
            // Show goal message
            if (goalMessage != null)
            {
                Debug.Log("Activating goal message!");
                goalMessage.SetActive(true);
                StartCoroutine(HideMessageAfterDelay());
            }
            else
            {
                Debug.Log("ERROR: Goal Message is null!");
            }
        }
    }

    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        goalMessage.SetActive(false);
        Debug.Log("Goal message hidden!");
    }
}