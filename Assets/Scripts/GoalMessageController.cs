using System.Collections;
using UnityEngine;
using TMPro;

public class GoalMessageController : MonoBehaviour
{
    public static GoalMessageController Instance;
    
    public GameObject goalMessage;
    public TextMeshProUGUI goalText;
    public float displayDuration = 3f;
    
    private int redShirtsCaught = 0;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        if (goalMessage != null)
        {
            goalMessage.SetActive(false);
        }
    }
    
    public void OnNPCCaught()
    {
        redShirtsCaught++;
        ShowMessage();
    }
    
    void ShowMessage()
    {
        if (goalText != null)
        {
            // Set message based on how many caught
            if (redShirtsCaught == 1)
            {
                goalText.text = "Great, 2 more to go!";
            }
            else if (redShirtsCaught == 2)
            {
                goalText.text = "Nice, 1 more!";
            }
            else if (redShirtsCaught >= 3)
            {
                goalText.text = "Excellent, you caught all of them!";
            }
        }
        
        if (goalMessage != null)
        {
            StopAllCoroutines();
            StartCoroutine(ShowMessageForDuration());
        }
    }
    
    IEnumerator ShowMessageForDuration()
    {
        goalMessage.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        goalMessage.SetActive(false);
    }
}