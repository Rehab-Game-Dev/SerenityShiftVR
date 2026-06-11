using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VRPauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject vrMenuCanvas;
    public Toggle compassToggle;
    public Button resumeButton;
    public Button mainMenuButton;

    [Header("Placement Settings")]
    public Transform cameraTransform;
    public float distanceFromCamera = 0.8f;
    public float heightOffset = 0.0f;
    public float horizontalOffset = 0.0f;

    private void Awake()
    {
        if (compassToggle != null)
        {
            compassToggle.isOn = PlayerPrefs.GetInt("CompassToggle", 1) == 1;
            compassToggle.onValueChanged.AddListener(OnCompassToggleChanged);
        }
        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        string sceneName = AuthManager.VR_ON ? "MainMenu_VR" : "MainMenu";
        SceneManager.LoadScene(sceneName);
    }

    public void ResumeGame()
    {
        var managers = Object.FindObjectsByType<PauseManager>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        if (managers != null && managers.Length > 0)
        {
            foreach (var pm in managers)
            {
                pm.Resume();
            }
        }
    }

    private void OnEnable()
    {
        if (AuthManager.VR_ON) PositionMenu();

        PauseManager.OnPausedGlobal -= HandleGamePaused;
        PauseManager.OnResumedGlobal -= HandleGameResumed;
        PauseManager.OnPausedGlobal += HandleGamePaused;
        PauseManager.OnResumedGlobal += HandleGameResumed;
    }

    private void OnDisable()
    {
        PauseManager.OnPausedGlobal -= HandleGamePaused;
        PauseManager.OnResumedGlobal -= HandleGameResumed;
    }

    [Header("Debug")]
    public string lastStatus = "Idle";

    private void HandleGamePaused()
    {
        lastStatus = "Pause Received at " + Time.unscaledTime;
        if (vrMenuCanvas != null)
        {
            vrMenuCanvas.SetActive(true);
            
            Canvas canvas = vrMenuCanvas.GetComponent<Canvas>();
            if (AuthManager.VR_ON)
            {
                PositionMenu();
                lastStatus += " | VR Menu Positioned";
            }
            else
            {
                if (canvas != null)
                {
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    vrMenuCanvas.transform.localScale = Vector3.one;
                }
                lastStatus += " | PC Menu Shown (Overlay)";
            }
        }
    }

    private void HandleGameResumed()
    {
        if (vrMenuCanvas != null)
        {
            vrMenuCanvas.SetActive(false);
        }
    }

    public void ToggleMenu()
    {
        var managers = Object.FindObjectsByType<PauseManager>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        if (managers != null && managers.Length > 0)
        {
            foreach (var pm in managers)
            {
                pm.TogglePause();
            }
        }
        else
        {
            Debug.LogWarning("No PauseManagers found to toggle.");
        }
    }

    public void PositionMenu()
    {
        if (cameraTransform == null)
        {
            if (Camera.main != null) cameraTransform = Camera.main.transform;
            else {
                var cam = GameObject.FindWithTag("MainCamera");
                if (cam != null) cameraTransform = cam.transform;
            }
        }

        if (cameraTransform != null && vrMenuCanvas != null)
        {
            vrMenuCanvas.transform.SetParent(cameraTransform);

            Canvas canvas = vrMenuCanvas.GetComponent<Canvas>();
            RectTransform rectTransform = vrMenuCanvas.GetComponent<RectTransform>();
            
            if (canvas != null)
            {
                canvas.renderMode = RenderMode.WorldSpace;
                Camera camComp = cameraTransform.GetComponent<Camera>();
                if (camComp == null) camComp = Camera.main;
                canvas.worldCamera = camComp;
                canvas.sortingOrder = 100;
}

            if (rectTransform != null)
            {
                if (rectTransform.sizeDelta.x < 100)
                {
                    rectTransform.sizeDelta = new Vector2(800, 600);
                }
            }

            vrMenuCanvas.transform.localScale = new Vector3(0.0015f, 0.0015f, 0.0015f);
            vrMenuCanvas.transform.localPosition = new Vector3(horizontalOffset, heightOffset, distanceFromCamera);
            vrMenuCanvas.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnCompassToggleChanged(bool val)
    {
        PlayerPrefs.SetInt("CompassToggle", val ? 1 : 0);
        var compass = Object.FindFirstObjectByType<CompassController>(FindObjectsInactive.Include);
        if (compass != null) compass.UpdateVisibility();
    }
}
