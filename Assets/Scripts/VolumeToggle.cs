using UnityEngine;
using UnityEngine.UI;

public class VolumeToggle : MonoBehaviour
{
    [Header("Volume Icons")]
    public Sprite volumeEnabledSprite;
    public Sprite volumeDisabledSprite;
    
    private RawImage buttonImage;
    private Button button;
    
    void Start()
    {
        // Get the RawImage component from Vol_Icon child
        buttonImage = transform.Find("Vol_Icon").GetComponent<RawImage>();
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
            buttonImage.texture = volumeDisabledSprite.texture;
        }
        else
        {
            buttonImage.texture = volumeEnabledSprite.texture;
        }
    }
}