using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VRMenuDropdownHandler : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    [Header("Elements to hide in VR")]
    public GameObject[] pcElements;

    void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    void Start()
    {
        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        // Hide PC elements if in VR
        if (AuthManager.VR_ON && pcElements != null)
        {
            foreach (var el in pcElements)
            {
                if (el != null) el.SetActive(false);
            }
        }
    }

    void OnValueChanged(int index)
    {
        if (index == 0) return; // Placeholder "Menu..."

        string option = dropdown.options[index].text;
        
        if (option == "Resume")
        {
            var pm = Object.FindFirstObjectByType<PauseManager>();
            if (pm != null) pm.Resume();
        }
        else if (option == "Main Menu")
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
        
        // Reset dropdown to placeholder
        dropdown.value = 0;
    }
}