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
    private int birdsCaught = 0;
    
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
        ShowNPCMessage();
    }
    
    public void OnBirdCaught()
    {
        birdsCaught++;
        ShowBirdMessage();
    }
    
    void ShowNPCMessage()
    {
        if (goalText != null)
        {
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
    
    void ShowBirdMessage()
    {
        if (goalText != null)
        {
            if (birdsCaught == 1)
            {
                goalText.text = "Great job! 2 more birds to go!";
            }
            else if (birdsCaught == 2)
            {
                goalText.text = "Nice catch! 1 more bird!";
            }
            else if (birdsCaught >= 3)
            {
                goalText.text = "Excellent! You caught all the birds!";
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