using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuLoader : MonoBehaviour
{
    public void LoadTutorial()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneAsync("StreetScene - tutorial"));
    }
    public void LoadEasy()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneAsync("StreetScene - easy"));
    }
    public void LoadMedium()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneAsync("StreetScene - medium"));
    }
    public void LoadHard()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneAsync("StreetScene - hard"));
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        string sceneName = AuthManager.VR_ON ? "MainMenu_VR" : "MainMenu";
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private System.Collections.IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void Street()
    {
        LoadMainMenu();
    }
public void Train()
    {
        // Do nothing for now    
    }
}