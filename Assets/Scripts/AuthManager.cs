using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Collections.Generic;

public class AuthManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject inputPanel;
    public TMP_InputField userField;
    public TMP_InputField passField;
    public TextMeshProUGUI statusText;

    private bool isSigningUp = false;

    async void Start()
    {
        await UnityServices.InitializeAsync();
        inputPanel.SetActive(false);
    }

    public void OpenSignUp()
    {
        isSigningUp = true;
        inputPanel.SetActive(true);
        statusText.text = "Create New Account";
    }

    public void OpenSignIn()
    {
        isSigningUp = false;
        inputPanel.SetActive(true);
        statusText.text = "Welcome Back!";
    }

    public void GuestLogin()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ClosePanel()
    {
        inputPanel.SetActive(false);
    }

    public async void OnSubmitPressed()
    {
        string u = userField.text;
        string p = passField.text;

        if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p))
        {
            statusText.text = "Error: Fields cannot be empty";
            return;
        }

        statusText.text = "Processing...";

        try
        {
            if (isSigningUp)
            {
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(u, p);
                SaveLevelData(1);
                SceneManager.LoadScene("StreetScene - tutorial");
            }
            else
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(u, p);
                LoadLevelData();
            }
        }
        catch (System.Exception e)
        {
            statusText.text = "Error: " + e.Message;
            Debug.LogError(e.Message);
        }
    }

    private async void SaveLevelData(int level)
    {
        var data = new Dictionary<string, object> { { "saved_level", level } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }

    private async void LoadLevelData()
    {
        var data = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "saved_level" });

        if (data.ContainsKey("saved_level"))
        {
            int levelToLoad = data["saved_level"].Value.GetAs<int>();

            switch (levelToLoad)
            {
                case 1:
                    SceneManager.LoadScene("StreetScene - tutorial");
                    break;
                case 2:
                    SceneManager.LoadScene("StreetScene - easy");
                    break;
                case 3:
                    SceneManager.LoadScene("StreetScene - medium");
                    break;
                case 4:
                    SceneManager.LoadScene("StreetScene - hard");
                    break;
                default:
                    SceneManager.LoadScene("MainMenu");
                    break;
            }
        }
        else
        {
            SceneManager.LoadScene("StreetScene - tutorial");
        }
    }
}