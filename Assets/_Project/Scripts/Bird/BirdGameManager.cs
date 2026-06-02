using UnityEngine;
using TMPro;

public class BirdGameManager : MonoBehaviour
{
    public static BirdGameManager Instance;
    
    public int caughtCount = 0;
    public int totalBirds = 3;
    
    [Header("UI Reference")]
    public TextMeshProUGUI birdCounterText; // Reference to your bird counter text
    
    private void Awake()
    {
        Instance = this;
        caughtCount = 0;
        UpdateCounterUI();
    }
    
    public void BirdCaught()
    {
        caughtCount++;
        Debug.Log("caught bird " + caughtCount + "/" + totalBirds);
        UpdateCounterUI();
        
        if (caughtCount >= totalBirds)
        {
            Debug.Log("All birds caught! You win!");
            // You can add win condition logic here
        }
    }
    
    private void UpdateCounterUI()
    {
        if (birdCounterText != null)
        {
            birdCounterText.text = caughtCount + " / " + totalBirds;
        }
        else
        {
            Debug.LogWarning("Bird Counter Text is not assigned in BirdGameManager!");
        }
    }
}