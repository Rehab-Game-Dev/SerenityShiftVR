using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Add this for TextMeshPro

public class KeysDropdown : MonoBehaviour
{
    [Header("Panels to Toggle")]
    public GameObject keysInstructionPanel;
    public GameObject keysPanel2;
    
    [Header("Button")]
    public Button keysButton;
    
    [Header("Text to Change")]
    public TextMeshProUGUI keysText;  // For TextMeshPro
    // OR use this if you're using regular Text:
    // public Text keysText;
    
    private bool isOpen = false;

    void Start()
    {
        // Start with panels HIDDEN
        isOpen = false;
        
        if (keysInstructionPanel != null)
            keysInstructionPanel.SetActive(false);  // Changed to false
        if (keysPanel2 != null)
            keysPanel2.SetActive(false);  // Changed to false
        
        // Set initial text to show closed state
        if (keysText != null)
            keysText.text = "Keys ▼";
        
        // Add click listener to button
        if (keysButton != null)
            keysButton.onClick.AddListener(ToggleDropdown);
    }

    public void ToggleDropdown()
    {
        isOpen = !isOpen;
        
        // Toggle panel visibility
        if (keysInstructionPanel != null)
            keysInstructionPanel.SetActive(isOpen);
        if (keysPanel2 != null)
            keysPanel2.SetActive(isOpen);
        
        // Change text based on state
        if (keysText != null)
            keysText.text = isOpen ? "Keys ▲" : "Keys ▼";
    }
}