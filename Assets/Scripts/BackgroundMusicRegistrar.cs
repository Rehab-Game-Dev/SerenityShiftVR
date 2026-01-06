using UnityEngine;

public class BackgroundMusicRegistrar : MonoBehaviour
{
    // Attach this script to your background music GameObject in each scene
    
    void Start()
    {
        AudioSource musicSource = GetComponent<AudioSource>();
        
        if (musicSource != null && AudioManager.Instance != null)
        {
            // Register this music source with the AudioManager
            AudioManager.Instance.RegisterBackgroundMusic(musicSource);
        }
    }
}