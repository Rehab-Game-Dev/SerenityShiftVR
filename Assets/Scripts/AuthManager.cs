using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Collections.Generic;

public class AuthManager : MonoBehaviour
{
    // Global VR mode flag - accessible from anywhere
    public static bool VR_ON = false; // Default to PC mode

    [Header("UI References")]
    public GameObject inputPanel;
    public GameObject VersionSelectionPanel;
    public TMP_InputField userField;
    public TMP_InputField passField;
    public TextMeshProUGUI statusText;
    public GameObject initUIParent;
    public TextMeshProUGUI detailsText;
    public GameObject verSelCloseBtn;

    

    private bool isSigningUp = false;

    async void Start()
    {
        await UnityServices.InitializeAsync();
        
    }
    public void SelectVRVersion()
    {
        setVR_true();
        LoadLevelData();
    }
    
    public void SelectPCVersion()
    {
        setVR_false();
        LoadLevelData();
    }

    public void SelectVRVersion_guest()
    {
        setVR_true();
    }
    
    public void SelectPCVersion_guest()
    {
        setVR_false();
    }

    public void setVR_true()
    {
        VR_ON = true;
        Debug.Log("VR Mode selected");
        SceneManager.LoadScene("Environment Menu");
    }

    public void setVR_false()
    {
        VR_ON = false;
        Debug.Log("PC Mode selected");
        SceneManager.LoadScene("Environment Menu");
    }


    public void OpenSignUp()
    {
        verSelCloseBtn.SetActive(false);
        detailsText.fontSize = 100;
        initUIParent.SetActive(false);
        isSigningUp = true;
        inputPanel.SetActive(true);
        statusText.text = "Create Account:";
    }

    public void OpenSignIn()
    {
        verSelCloseBtn.SetActive(false);
        detailsText.fontSize = 100;
        initUIParent.SetActive(false);
        isSigningUp = false;
        inputPanel.SetActive(true);
        statusText.text = "Welcome Back!";
    }

    public void GuestLogin()
    {
        verSelCloseBtn.SetActive(true);
        VersionSelectionPanel.SetActive(true);
        initUIParent.SetActive(false);
    }

    public void ClosePanel()
    {
        inputPanel.SetActive(false);
        VersionSelectionPanel.SetActive(false);
        initUIParent.SetActive(true);
    }

    public async void OnSubmitPressed()
    {
        string u = userField.text;
        string p = passField.text;

        if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p))
        {
            detailsText.text = "Fields cannot be empty";
            return;
        }
    

        statusText.text = "Processing...";

        try
        {
            if (isSigningUp)
            {
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(u, p);
                SaveLevelData(1);
                inputPanel.SetActive(false);
                VersionSelectionPanel.SetActive(true);
            }
            else
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(u, p);
                inputPanel.SetActive(false);
                VersionSelectionPanel.SetActive(true);
            }
        }
        catch (System.Exception e)
        {
            
            statusText.text = "Error: " + e.Message;

            // Display error in DetailsText with font size 40
            if (detailsText != null)
            {
                detailsText.text = e.Message;
                detailsText.fontSize = 40;
            }
    
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