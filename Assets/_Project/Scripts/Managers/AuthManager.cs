using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;

public class AuthManager : MonoBehaviour
{
    public static bool VR_ON = true;

    [Header("UI References")]
    public GameObject inputPanel;
    public GameObject VersionSelectionPanel;
    public TMP_InputField userField;
    public TMP_InputField passField;
    public TextMeshProUGUI statusText;
    public GameObject initUIParent;
    public TextMeshProUGUI detailsText;
    public GameObject verSelCloseBtn;

    private bool _isSigningUp;

    private void Awake()
    {
        CheckForVR();
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    private void CheckForVR()
    {
        bool isXRActive = false;
        var xrSettings = UnityEngine.XR.Management.XRGeneralSettings.Instance;

        if (xrSettings != null && xrSettings.Manager != null && xrSettings.Manager.activeLoader != null)
        {
            isXRActive = true;
        }

        if (isXRActive || Application.platform == RuntimePlatform.Android)
        {
            VR_ON = true;
            Debug.Log("VR Mode auto-detected");
        }
    }

    public void SelectVRVersion()
    {
        SetVRTrue();
        LoadLevelData();
    }

    public void SelectPCVersion()
    {
        SetVRFalse();
        LoadLevelData();
    }

    public void SelectVRVersionGuest()
    {
        SetVRTrue();
    }

    public void SelectPCVersionGuest()
    {
        SetVRFalse();
    }

    private void SetVRTrue()
    {
        VR_ON = true;
        Debug.Log("VR Mode selected");
        SceneManager.LoadScene("EnvironmentMenu_VR");
    }

    private void SetVRFalse()
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
        _isSigningUp = true;
        inputPanel.SetActive(true);
        statusText.text = "Create Account:";
    }

    public void OpenSignIn()
    {
        verSelCloseBtn.SetActive(false);
        detailsText.fontSize = 100;
        initUIParent.SetActive(false);
        _isSigningUp = false;
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
        string username = userField.text;
        string password = passField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            detailsText.text = "Fields cannot be empty";
            return;
        }

        statusText.text = "Processing...";

        try
        {
            if (_isSigningUp)
            {
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
                SaveLevelData(1);
            }
            else
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            }

            inputPanel.SetActive(false);
            VersionSelectionPanel.SetActive(true);
        }
        catch (System.Exception e)
        {
            statusText.text = "Error: " + e.Message;

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
