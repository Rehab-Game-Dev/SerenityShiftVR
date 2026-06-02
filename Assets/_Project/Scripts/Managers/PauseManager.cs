using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    private List<AudioSource> pausedAudioSources = new List<AudioSource>();
    public GameObject pauseOverlay;
    
    [Header("VR Input")]
    public InputActionReference pauseAction;

    void Start()
    {
        Debug.Log("PauseManager started on: " + gameObject.name);
        if (pauseOverlay == null)
            pauseOverlay = GameObject.Find("PauseOverlay");
        
        if (pauseOverlay == null)
            pauseOverlay = GameObject.Find("GameHUD")?.transform.Find("PauseOverlay")?.gameObject;

        Debug.Log("PauseOverlay found: " + (pauseOverlay != null ? pauseOverlay.name : "NULL"));
    }

    private void OnEnable()
    {
        if (pauseAction != null) pauseAction.action.performed += OnPauseAction;
    }

    private void OnDisable()
    {
        if (pauseAction != null) pauseAction.action.performed -= OnPauseAction;
    }

    private void OnPauseAction(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0f;
        
        if (pauseOverlay != null)
            pauseOverlay.SetActive(true);

        if (!AuthManager.VR_ON)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        foreach (NavMeshAgent agent in FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None))
        {
            agent.isStopped = true;
        }
        
        pausedAudioSources.Clear();
        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audio in allAudioSources)
        {
            if (audio != null && audio.isPlaying)
            {
                audio.Pause();
                pausedAudioSources.Add(audio);
            }
        }
    }

    public void Resume()
    {
        if (!isPaused) return;
        isPaused = false;
        Time.timeScale = 1f;
        
        if (pauseOverlay != null)
            pauseOverlay.SetActive(false);

        if (!AuthManager.VR_ON)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        foreach (NavMeshAgent agent in FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None))
        {
            if (agent != null && agent.isOnNavMesh)
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