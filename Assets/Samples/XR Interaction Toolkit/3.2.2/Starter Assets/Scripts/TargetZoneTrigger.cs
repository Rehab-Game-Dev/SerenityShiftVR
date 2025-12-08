using UnityEngine;


public class TargetZoneTrigger : MonoBehaviour
{
    public GameObject goalMessage;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the target zone!");
            
            if (goalMessage != null)
            {
                Debug.Log("Activating goal message!");
                goalMessage.SetActive(true);
            }
            else
            {
                Debug.Log("ERROR: Goal Message is null!");
            }
        }
    }
}