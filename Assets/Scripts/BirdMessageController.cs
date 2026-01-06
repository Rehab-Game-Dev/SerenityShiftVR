using System.Collections;
using UnityEngine;
using TMPro;

public class BirdMessageController : MonoBehaviour
{
    public static BirdMessageController Instance;
    
    public GameObject birdMessage;
    public TextMeshProUGUI birdText;
    public float displayDuration = 3f;
    
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
        if (birdMessage != null)
        {
            birdMessage.SetActive(false);
        }
    }
    
    public void OnBirdCaught()
    {
        birdsCaught++;
        ShowMessage();
    }
    
    void ShowMessage()
    {
        if (birdText != null)
        {
            // Same messages as NPCs
            if (birdsCaught == 1)
            {
                birdText.text = "Great, 2 more to go!";
            }
            else if (birdsCaught == 2)
            {
                birdText.text = "Nice, 1 more!";
            }
            else if (birdsCaught >= 3)
            {
                birdText.text = "Excellent, you caught all of them!";
            }
        }
        
        if (birdMessage != null)
        {
            StopAllCoroutines();
            StartCoroutine(ShowMessageForDuration());
        }
    }
    
    IEnumerator ShowMessageForDuration()
    {
        birdMessage.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        birdMessage.SetActive(false);
    }
}