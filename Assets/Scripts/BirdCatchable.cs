using UnityEngine;

public class BirdCatchable : MonoBehaviour
{
    public bool hasBeenCaught = false;

    public void CatchBird()
    {
        if (hasBeenCaught) return;
        
        hasBeenCaught = true;
        
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