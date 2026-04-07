using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    private List<AudioSource> pausedAudioSources = new List<AudioSource>();
    public GameObject pauseOverlay;

    void Start()
    {
        Debug.Log("PauseManager started on: " + gameObject.name);
        if (pauseOverlay == null)
            pauseOverlay = GameObject.Find("PauseOverlay");
        Debug.Log("PauseOverlay found: " + (pauseOverlay != null ? pauseOverlay.name : "NULL"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseOverlay.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        foreach (NavMeshAgent agent in FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None))
        {
            agent.isStopped = true;
        }
        pausedAudioSources.Clear();
        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audio in allAudioSources)
        {
            if (audio.isPlaying)
            {
                audio.Pause();
                pausedAudioSources.Add(audio);
            }
        }
    }
    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseOverlay.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach (NavMeshAgent agent in FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None))
        {
            agent.isStopped = false;
        }
        foreach (AudioSource audio in pausedAudioSources)
        {
            if (audio != null)
                audio.UnPause();
        }
        pausedAudioSources.Clear();
    }
}