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
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadTrain()
    {
        // Do nothing for now    
    }
    
    public void LoadStreet()
    {
        SceneManager.LoadScene("MainMenu");
    }

}