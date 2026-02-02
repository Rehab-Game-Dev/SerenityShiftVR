using UnityEngine;
using UnityEngine.InputSystem;

public class InGameMenuController : MonoBehaviour
{
    public GameObject menuObject; 
    public InputActionReference menuButtonAction; // reference to the controller button

    void OnEnable() {
        // Register for the button press event
        menuButtonAction.action.performed += ToggleMenu;
    }

    void OnDisable() {
        // Unregister to prevent errors
        menuButtonAction.action.performed -= ToggleMenu;
    }

    void ToggleMenu(InputAction.CallbackContext context) {
        // Toggle the display state (if on - turn off, if off - turn on)
        bool isActive = !menuObject.activeSelf;
        menuObject.SetActive(isActive);
        
        // Bonus: position the menu in front of the player when it opens
        if(isActive) {
            menuObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
            menuObject.transform.LookAt(Camera.main.transform);
            menuObject.transform.Rotate(0, 180, 0);
        }
    }
}