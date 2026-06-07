using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player References")]
    public GameObject pcPlayer;
    public GameObject xrRig;
    public GameObject PC_Controls_Panel;

    [Header("VR Settings")]
    public float vrCameraHeight = 1.36f;

    // Player switching is handled manually in the Inspector
    // Enable/disable pcPlayer or xrRig before pressing Play
}