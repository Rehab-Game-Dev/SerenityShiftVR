using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public int caughtCount = 0;
    public int totalNPCs = 3;
    
    [Header("UI Reference")]
    public TextMeshProUGUI counterText;
    
    [Header("Men UI Objects to Hide")]
    public GameObject panel1_Men;
    public GameObject panel2_Men; 
    public GameObject npcIcon;
    
    [Header("Bird UI Objects to Show")]
    public GameObject panel1_Birds;
    public GameObject panel2_Birds;
    public GameObject birdIcon;
    public GameObject corsair;
    
    [Header("Bird References")]
    public BirdSpawner birdSpawner;
    public BirdManager birdManager;
    
    private void Awake()
    {
        Instance = this;
        caughtCount = 0;
        UpdateCounterUI();
        HideBirdUIElements();
    }
    
    public void NPCCaught()
    {
        caughtCount++;
        Debug.Log("caught " + caughtCount + "/" + totalNPCs);
        UpdateCounterUI();
        
        if (caughtCount >= totalNPCs)
        {
            Debug.Log("All NPCs caught!");
            
            HideMenUIElements();
            ShowBirdUIElements();
            
            // Activate birds from scene
            if (birdManager != null)
            {
                birdManager.ActivateBirds();
            }
            
            // Start spawning new birds
            if (birdSpawner != null)
            {
                birdSpawner.StartSpawning();
            }
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
    
    private void HideMenUIElements()
    {
        if (panel1_Men != null)
            panel1_Men.SetActive(false);
            
        if (panel2_Men != null)
            panel2_Men.SetActive(false);
            
        if (npcIcon != null)
            npcIcon.SetActive(false);
            
        Debug.Log("Men UI elements hidden!");
    }
    
    private void HideBirdUIElements()
    {
        if (panel1_Birds != null)
            panel1_Birds.SetActive(false);
            
        if (panel2_Birds != null)
            panel2_Birds.SetActive(false);
            
        if (birdIcon != null)
            birdIcon.SetActive(false);
            
        if (corsair != null)
            corsair.SetActive(false);
    }
    
    private void ShowBirdUIElements()
    {
        if (panel1_Birds != null)
            panel1_Birds.SetActive(true);
            
        if (panel2_Birds != null)
            panel2_Birds.SetActive(true);
            
        if (birdIcon != null)
            birdIcon.SetActive(true);
            
        if (corsair != null)
            corsair.SetActive(true);
            
        Debug.Log("Bird UI elements shown!");
    }
}