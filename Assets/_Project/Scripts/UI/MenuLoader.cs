using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuLoader : MonoBehaviour
{
    public void LoadTutorial()
    {
        SceneManager.LoadScene("StreetScene - tutorial");
    }
    public void LoadEasy()
    {
        SceneManager.LoadScene("StreetScene - easy");
    }
    public void LoadMedium()
    {
        SceneManager.LoadScene("StreetScene - medium");
    }
    public void LoadHard()
    {
        SceneManager.LoadScene("StreetScene - hard");
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (AuthManager.VR_ON)
        {
            SceneManager.LoadScene("MainMenu_VR");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    public void Street()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (AuthManager.VR_ON)
        {
            SceneManager.LoadScene("MainMenu_VR");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    public void Train()
    {
        // Do nothing for now    
    }
}