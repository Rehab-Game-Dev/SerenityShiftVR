using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private bool isMenuOpen = false;
    
    void Start()
    {
        // Start with cursor locked for gameplay
        LockCursor();
    }

    void Update()
    {
        // Press Tab to toggle menu mode
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isMenuOpen = !isMenuOpen;
            
            if (isMenuOpen)
            {
                UnlockCursor();
                Debug.Log("Menu opened - Cursor visible");
            }
            else
            {
                LockCursor();
                Debug.Log("Menu closed - Cursor locked");
            }
        }
        
        // Force cursor state every frame while menu is open
        if (isMenuOpen)
        {
            // This will override anything trying to lock it
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
                Debug.LogWarning("Something tried to lock cursor while menu is open!");
            }
            if (!Cursor.visible)
            {
                Cursor.visible = true;
            }
        }
    }
    
    void LateUpdate()
    {
        // Extra enforcement in LateUpdate (runs after all Update methods)
        if (isMenuOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}