using UnityEngine;
using UnityEngine.UI;

public class VolumeToggle : MonoBehaviour
{
    [Header("Volume Icons")]
    public Sprite volumeEnabledSprite;
    public Sprite volumeDisabledSprite;
    
    private Image buttonImage;
    private Button button;

    void Start()
    {
        // Get the Image component from the button
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
        
        // Add click listener
        button.onClick.AddListener(ToggleVolume);
        
        // Set initial state from AudioManager
        UpdateVolumeIcon();
    }

    public void ToggleVolume()
    {
        // Toggle music mute state in AudioManager
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleMusic();
        }
        
        // Update the icon
        UpdateVolumeIcon();
    }

    private void UpdateVolumeIcon()
    {
        // Get mute state from AudioManager
        bool isMuted = false;
        if (AudioManager.Instance != null)
        {
            isMuted = AudioManager.Instance.isMusicMuted;
        }
        
        // Update sprite based on state
        if (isMuted)
        {
            buttonImage.sprite = volumeDisabledSprite;
        }
        else
        {
            buttonImage.sprite = volumeEnabledSprite;
        }
    }
}