using UnityEngine;
using UnityEngine.InputSystem;

public class AppController : MonoBehaviour
{
    void Update()
    {
        // Global quit shortcut for PC testing
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            QuitGame();
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
