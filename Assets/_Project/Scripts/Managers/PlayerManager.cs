using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player References")]
    public GameObject pcPlayer;
    public GameObject xrRig;
    public GameObject PC_Controls_Panel;

    [Header("VR Settings")]
    public float vrCameraHeight = 1.87f;

    private void Start()
    {
        InitializePlayers();
        SetCameraHeight();
    }

    public void InitializePlayers()
    {
        Debug.Log($"InitializePlayers called. VR_ON: {AuthManager.VR_ON}");

        // Untag all to start clean
        var allCameras = Object.FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var cam in allCameras)
        {
            if (cam.CompareTag("MainCamera")) cam.tag = "Untagged";
        }

        if (AuthManager.VR_ON)
        {
            if (pcPlayer != null) pcPlayer.SetActive(false);
            if (xrRig != null)
            {
                xrRig.SetActive(true);
                Camera vrCam = xrRig.GetComponentInChildren<Camera>(true);
                if (vrCam != null)
                {
                    vrCam.tag = "MainCamera";
                    vrCam.enabled = true;
                }
            }
            if (PC_Controls_Panel != null) PC_Controls_Panel.SetActive(false);
        }
        else
        {
            if (xrRig != null) xrRig.SetActive(false);
            if (pcPlayer != null)
            {
                pcPlayer.SetActive(true);
                Camera pcCam = pcPlayer.GetComponentInChildren<Camera>(true);
                if (pcCam != null)
                {
                    pcCam.tag = "MainCamera";
                    pcCam.enabled = true;
                }
            }
            if (PC_Controls_Panel != null) PC_Controls_Panel.SetActive(true);
        }
    }

    private void SetCameraHeight()
    {
        if (xrRig == null) return;
        Transform cameraOffset = xrRig.transform.Find("Camera Offset");
        if (cameraOffset != null)
            cameraOffset.localPosition = new Vector3(0, vrCameraHeight, 0);
    }
}