using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Linq;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    private List<AudioSource> pausedAudioSources = new List<AudioSource>();
    public GameObject pauseOverlay;
    
    [Header("VR Input")]
    public InputActionReference pauseAction;
    public InputActionReference jumpAction;

    void Start()
    {
        Debug.Log("PauseManager started on: " + gameObject.name);
        
        if (pauseOverlay == null)
        {
            // Try to find the PauseOverlay even if it's inactive
            var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            pauseOverlay = allObjects.FirstOrDefault(go => go.name == "PauseOverlay" && go.scene == gameObject.scene);
        }
        
        if (pauseOverlay == null)
        {
            // Fallback to searching child of GameHUD
            var hud = GameObject.Find("GameHUD");
            if (hud != null)
            {
                var transform = hud.transform.Find("PauseOverlay");
                if (transform != null) pauseOverlay = transform.gameObject;
            }
        }

        Debug.Log("PauseOverlay found: " + (pauseOverlay != null ? pauseOverlay.name : "NULL"));
    }

    private void OnEnable()
    {
        if (pauseAction != null && pauseAction.action != null)
        {
            pauseAction.action.Enable();
            pauseAction.action.performed += OnPauseAction;
        }
        if (jumpAction != null && jumpAction.action != null)
        {
            jumpAction.action.Enable();
            jumpAction.action.performed += OnPauseAction;
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null && pauseAction.action != null)
            pauseAction.action.performed -= OnPauseAction;
        if (jumpAction != null && jumpAction.action != null)
            jumpAction.action.performed -= OnPauseAction;
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

    public event System.Action OnPaused;
    public event System.Action OnResumed;

    public static event System.Action OnPausedGlobal;
    public static event System.Action OnResumedGlobal;

    public void Pause()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0f;
        
        if (pauseOverlay != null)
            pauseOverlay.SetActive(true);

        OnPaused?.Invoke();
        OnPausedGlobal?.Invoke();

        if (!AuthManager.VR_ON)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        foreach (NavMeshAgent agent in FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None))
        {
            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
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

        OnResumed?.Invoke();
        OnResumedGlobal?.Invoke();

        if (!AuthManager.VR_ON)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        foreach (NavMeshAgent agent in FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None))
        {
            if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
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