using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public int caughtCount = 0;
    public int totalNPCs = 3;
    
    [Header("UI Reference")]
    public TextMeshProUGUI counterText; // Reference to your counter text
    
    private void Awake()
    {
        Instance = this;
        caughtCount = 0;
        UpdateCounterUI();
    }
    
    public void NPCCaught()
    {
        caughtCount++;
        Debug.Log("caught " + caughtCount + "/" + totalNPCs);
        UpdateCounterUI();
        
        if (caughtCount >= totalNPCs)
        {
            Debug.Log("All NPCs caught! You win!");
            // You can add win condition logic here
        }
    }
    
    private void UpdateCounterUI()
    {
        if (counterText != null)
        {
            counterText.text = caughtCount + " / " + totalNPCs;
        }
        else
        {
            Debug.LogWarning("Counter Text is not assigned in GameManager!");
        }
    }
}