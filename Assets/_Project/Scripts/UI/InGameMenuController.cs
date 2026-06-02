using UnityEngine;
using UnityEngine.InputSystem;

public class InGameMenuController : MonoBehaviour
{
    public GameObject menuObject; 
    public InputActionReference menuButtonAction; // reference to the controller button

    void Start()
    {
        // Close menu by default as requested
        if (menuObject != null)
        {
            menuObject.SetActive(false);
        }
    }

    void OnEnable() {
        if (menuButtonAction != null && menuButtonAction.action != null)
        {
            menuButtonAction.action.Enable();
            menuButtonAction.action.performed += ToggleMenu;
        }
    }

    void OnDisable() {
        if (menuButtonAction != null && menuButtonAction.action != null)
        {
            menuButtonAction.action.performed -= ToggleMenu;
        }
    }

    void Update()
    {
        // PC Fallback: M key
        if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu(InputAction.CallbackContext context) 
    {
        ToggleMenu();
    }

    public void ToggleMenu() {
        if (menuObject == null) return;

        bool isActive = !menuObject.activeSelf;
        menuObject.SetActive(isActive);
        
        if(isActive) {
            // Position the menu in front of the player
            // VRUIAnchor might already be handling this if attached to the menuObject
            // but we ensure it's positioned reasonably.
            Transform camTransform = Camera.main != null ? Camera.main.transform : null;
            if (camTransform != null && AuthManager.VR_ON)
            {
                menuObject.transform.position = camTransform.position + camTransform.forward * 1.5f;
                menuObject.transform.LookAt(camTransform);
                menuObject.transform.Rotate(0, 180, 0);
            }
        }

        // Handle cursor for PC mode
        if (!AuthManager.VR_ON)
        {
            Cursor.visible = isActive;
            Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
