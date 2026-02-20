using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    private AudioSource[] allAudioSources;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        // Pause all audio in the scene
        allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audio in allAudioSources)
        {
            if (audio.isPlaying)
                audio.Pause();
        }
    }

    void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;

        // Resume all audio
        foreach (AudioSource audio in allAudioSources)
        {
            audio.UnPause();
        }
    }
}