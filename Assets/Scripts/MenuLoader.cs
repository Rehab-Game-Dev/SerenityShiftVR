using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoader : MonoBehaviour
{
    public void LoadStreetScene()
    {
        SceneManager.LoadScene("StreetScene");
    }
}