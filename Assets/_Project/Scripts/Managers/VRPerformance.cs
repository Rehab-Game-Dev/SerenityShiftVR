using UnityEngine;

public class VRPerformance : MonoBehaviour
{
    void Start()
    {
        UnityEngine.XR.XRSettings.eyeTextureResolutionScale = 0.8f;
    }
}