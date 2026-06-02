using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player References")]
    public GameObject pcPlayer;
    public GameObject xrRig;
    public GameObject PC_Controls_Panel;

    private void Start()
    {
        SetupPlayer();
    }

    private void SetupPlayer()
    {
        if (AuthManager.VR_ON)
        {
            // VR Mode
            pcPlayer.SetActive(false);
            xrRig.SetActive(true);
            if (PC_Controls_Panel != null)
            {
                PC_Controls_Panel.SetActive(false); 
            }
            Debug.Log("VR Mode activated");
        }
        else
        {
            // PC Mode
            pcPlayer.SetActive(true);
            xrRig.SetActive(false);
            if (PC_Controls_Panel != null)
            {
                PC_Controls_Panel.SetActive(true); 
            }
            Debug.Log("PC Mode activated");
        }
    }
}