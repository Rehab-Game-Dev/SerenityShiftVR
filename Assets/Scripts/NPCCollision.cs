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