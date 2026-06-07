using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VRPauseMenu : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference jumpAction;
    
    [Header("UI References")]
    public GameObject vrMenuCanvas;
    public Toggle compassToggle;
    public Button resumeButton;

    [Header("Placement Settings")]
    public Transform cameraTransform;
    public float distanceFromCamera = 2.0f;
    public float heightOffset = 0.0f;
    public float horizontalOffset = -0.15f;

    private void Awake()
{
        if (compassToggle != null)
        {
            compassToggle.isOn = PlayerPrefs.GetInt("CompassToggle", 1) == 1;
            compassToggle.onValueChanged.AddListener(OnCompassToggleChanged);
        }
        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);
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
        if (jumpAction != null) jumpAction.action.performed += OnJumpPerformed;
        
        PauseManager.OnPausedGlobal -= HandleGamePaused;
        PauseManager.OnResumedGlobal -= HandleGameResumed;
        PauseManager.OnPausedGlobal += HandleGamePaused;
        PauseManager.OnResumedGlobal += HandleGameResumed;
    }

    private void OnDisable()
    {
        if (jumpAction != null) jumpAction.action.performed -= OnJumpPerformed;
        PauseManager.OnPausedGlobal -= HandleGamePaused;
        PauseManager.OnResumedGlobal -= HandleGameResumed;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        // Try to find any manager to toggle
        var pm = Object.FindFirstObjectByType<PauseManager>();
        if (pm != null) pm.TogglePause();
    }

    [Header("Debug")]
    public string lastStatus = "Idle";

    private void HandleGamePaused()
    {
        lastStatus = "Pause Received at " + Time.unscaledTime;
        if (AuthManager.VR_ON && vrMenuCanvas != null)
        {
            vrMenuCanvas.SetActive(true);
            PositionMenu();
            lastStatus += " | Menu Shown";
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
            // Lock to POV: Parent the canvas to the camera
            vrMenuCanvas.transform.SetParent(cameraTransform);

            Canvas canvas = vrMenuCanvas.GetComponent<Canvas>();
            RectTransform rectTransform = vrMenuCanvas.GetComponent<RectTransform>();
            
            if (canvas != null)
            {
                canvas.renderMode = RenderMode.WorldSpace;
                canvas.worldCamera = cameraTransform.GetComponent<Camera>();
            }

            if (rectTransform != null)
            {
                // Set a reasonable size for the VR menu if it's currently 0
                if (rectTransform.sizeDelta.x < 100)
                {
                    rectTransform.sizeDelta = new Vector2(800, 600);
                }
            }

            // Force a valid scale for world space.
            vrMenuCanvas.transform.localScale = new Vector3(0.0015f, 0.0015f, 0.0015f);

            // Set local position relative to camera
            vrMenuCanvas.transform.localPosition = new Vector3(horizontalOffset, heightOffset, distanceFromCamera);
            
            // In uGUI, the "front" of the UI faces the negative Z direction. 
            // When parented to the camera and placed at a positive Z offset, 
            // a local rotation of 0 makes it face the player correctly.
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