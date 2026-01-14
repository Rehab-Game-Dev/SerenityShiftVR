using UnityEngine;
using TMPro;

public class BirdGameManager : MonoBehaviour
{
    public static BirdGameManager Instance;
    
    public int caughtCount = 0;
    public int totalBirds = 3;
    
    [Header("UI Reference")]
    public TextMeshProUGUI birdCounterText; 
    
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
            Debug.Log("All birds caught! Switching to buttons mission.");
            
            // --- העדכון החדש כאן ---
            // אנחנו מחפשים את מנהל המשימות ומורים לו לעבור לשלב 3 (כפתורים)
            MissionManager mm = FindObjectOfType<MissionManager>();
            if (mm != null)
            {
                mm.SetMission(3); 
            }
            else
            {
                Debug.LogWarning("MissionManager not found in the scene!");
            }
            // -----------------------
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