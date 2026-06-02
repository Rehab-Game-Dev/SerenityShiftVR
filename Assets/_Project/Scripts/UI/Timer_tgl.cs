using UnityEngine;
using UnityEngine.UI;

public class TimerToggle : MonoBehaviour
{
    public GameObject timerPanel;

    void OnEnable()
    {
        bool saved = PlayerPrefs.GetInt("TimerVisible", 1) == 1;
        timerPanel.SetActive(saved);
        GetComponent<Toggle>().isOn = saved;
        GetComponent<Toggle>().onValueChanged.AddListener(OnToggleChanged);
    }

    void OnDisable()
    {
        GetComponent<Toggle>().onValueChanged.RemoveListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        timerPanel.SetActive(isOn);
        PlayerPrefs.SetInt("TimerVisible", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}