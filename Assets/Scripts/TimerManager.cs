using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float elapsedTime = 0f;
    private bool isRunning = true;

    public GameObject timerPanel;

    void Start()
    {
        bool saved = PlayerPrefs.GetInt("TimerVisible", 1) == 1;
        timerPanel.SetActive(saved);
    }
    
    void Update()
    {
        if (!isRunning) return;
        elapsedTime += Time.deltaTime;
        DisplayTime(elapsedTime);
    }

    void DisplayTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetFinalTime()
    {
        return elapsedTime;
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        return string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}