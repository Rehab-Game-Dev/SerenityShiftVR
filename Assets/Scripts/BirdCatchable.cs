using UnityEngine;

public class BirdCatchable : MonoBehaviour
{
    public bool hasBeenCaught = false;

    public void CatchBird()
    {
        if (hasBeenCaught) return;
        
        hasBeenCaught = true;
        
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