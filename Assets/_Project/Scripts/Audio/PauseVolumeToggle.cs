using UnityEngine;
using UnityEngine.UI;

public class PauseVolumeToggle : MonoBehaviour
{
    private Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();

        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager.Instance is NULL!");
            return;
        }

        Debug.Log("AudioManager found. isMusicMuted: " + AudioManager.Instance.isMusicMuted);
        toggle.isOn = !AudioManager.Instance.isMusicMuted;
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        Debug.Log("Toggle changed to: " + isOn);
        if (AudioManager.Instance != null)
        {
            Debug.Log("Before toggle - isMusicMuted: " + AudioManager.Instance.isMusicMuted);
            if (isOn == AudioManager.Instance.isMusicMuted)
                AudioManager.Instance.ToggleMusic();
            Debug.Log("After toggle - isMusicMuted: " + AudioManager.Instance.isMusicMuted);
        }
    }
}