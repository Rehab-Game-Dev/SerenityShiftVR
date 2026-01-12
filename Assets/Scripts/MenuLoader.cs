using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoader : MonoBehaviour
{
    // טעינת שלב הדרכה
    public void LoadTutorial()
    {
        SceneManager.LoadScene("StreetScene - tutorial");
    }

    // טעינת שלב קל
    public void LoadEasy()
    {
        SceneManager.LoadScene("StreetScene - easy");
    }

    // טעינת שלב בינוני
    public void LoadMedium()
    {
        SceneManager.LoadScene("StreetScene - medium");
    }

    // טעינת שלב קשה
    public void LoadHard()
    {
        SceneManager.LoadScene("StreetScene - hard");
    }

    // פונקציה כללית לחזרה לתפריט ה-VR (מעודכן לשם הסצנה החדש שלך)
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // טעינת תפריט ה-VR באופן ספציפי
    public void LoadMainMenuVR()
    {
        SceneManager.LoadScene("MainMenu_VR");
    }

    // פונקציה לטעינת תפריט המחשב (למקרה שתצטרך בעתיד)
    public void LoadMainMenuPC()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadTrain()
    {
        // Do nothing for now    
    }
}