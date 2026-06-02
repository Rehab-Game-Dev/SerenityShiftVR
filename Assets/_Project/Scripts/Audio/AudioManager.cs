using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Volume State")]
    public bool isMusicMuted = false;
    
    [Header("Background Music")]
    public AudioSource backgroundMusicSource;
    
    void Awake()
    {
        // Singleton pattern - keep only one instance across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Apply saved music state
            UpdateMusicState();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Find background music if not assigned
        if (backgroundMusicSource == null)
        {
            GameObject musicObj = GameObject.Find("CityAudioSource"); // Change this to your music object name
            if (musicObj != null)
            {
                backgroundMusicSource = musicObj.GetComponent<AudioSource>();
            }
        }
        
        UpdateMusicState();
    }
    
    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;
        UpdateMusicState();
    }
    
    public void SetMusicMute(bool mute)
    {
        isMusicMuted = mute;
        UpdateMusicState();
    }
    
    private void UpdateMusicState()
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.mute = isMusicMuted;
        }
    }
    
    // Call this from other scripts when a new scene loads to register the music source
    public void RegisterBackgroundMusic(AudioSource musicSource)
    {
        backgroundMusicSource = musicSource;
        UpdateMusicState();
    }
}