using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Update()
    {
        // Press Tab to unlock cursor for UI
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        // Press left mouse button while playing to lock cursor again
        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
        {
            // Only lock if not clicking on UI
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}