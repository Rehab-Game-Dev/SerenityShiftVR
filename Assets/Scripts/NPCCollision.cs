using UnityEngine;
public class NPCCollision : MonoBehaviour
{
    public bool isCatchable = false;
    private bool hasBeenCaught = false;
    private void OnTriggerEnter(Collider other)
    {
        // Check if already caught
        if (hasBeenCaught) return;
        
        if (other.CompareTag("Player") && isCatchable)
        {
            hasBeenCaught = true;
            
            // Show the goal message for 3 seconds
            if (GoalMessageController.Instance != null)
            {
                GoalMessageController.Instance.OnNPCCaught();
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.NPCCaught();
            }
            else
            {
                Debug.LogError("GameManager.Instance is NULL!");
            }
            
            Destroy(gameObject);
        }
    }
}