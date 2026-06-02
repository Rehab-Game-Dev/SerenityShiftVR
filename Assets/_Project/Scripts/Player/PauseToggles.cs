using UnityEngine;
using UnityEngine.UI;

public class PauseToggles : MonoBehaviour
{
    public Toggle compassToggle;
    public Toggle timerToggle;
    public Toggle volumeToggle;

    void Start()
    {
        // Load saved states
        compassToggle.isOn = PlayerPrefs.GetInt("CompassToggle", 1) == 1;
        timerToggle.isOn = PlayerPrefs.GetInt("TimerToggle", 1) == 1;
        volumeToggle.isOn = PlayerPrefs.GetInt("VolumeToggle", 1) == 1;

        // Listen for changes
        compassToggle.onValueChanged.AddListener(val => PlayerPrefs.SetInt("CompassToggle", val ? 1 : 0));
        timerToggle.onValueChanged.AddListener(val => PlayerPrefs.SetInt("TimerToggle", val ? 1 : 0));
        volumeToggle.onValueChanged.AddListener(val => PlayerPrefs.SetInt("VolumeToggle", val ? 1 : 0));
    }
}