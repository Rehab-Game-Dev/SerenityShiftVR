using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VRLevelSelectionDropdown : MonoBehaviour
{
    private TMP_Dropdown dropdown;

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
    }

    void OnValueChanged(int index)
    {
        if (index == 0) return; // Placeholder

        string sceneName = "";
        switch (dropdown.options[index].text)
        {
            case "Tutorial": sceneName = "StreetScene - tutorial"; break;
            case "Easy": sceneName = "StreetScene - easy"; break;
            case "Medium": sceneName = "StreetScene - medium"; break;
            case "Hard": sceneName = "StreetScene - hard"; break;
        }

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}