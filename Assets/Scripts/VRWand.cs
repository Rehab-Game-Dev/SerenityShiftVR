using UnityEngine;
using UnityEngine.XR;

public class VRWand : MonoBehaviour
{
    [Header("Settings")]
    public XRNode controllerNode = XRNode.RightHand; // Which hand? Right or Left
    public float range = 100f; // Ray distance
    public LayerMask birdLayer; // To hit only birds (optional)

    [Header("Visuals")]
    public GameObject hitEffect; // Hit effect (optional)

    private bool isTriggerPressed = false;
    private bool wasPressedLastFrame = false; // To prevent continuous firing

    void Update()
    {
        // 1. Check input from the controller (is the trigger pressed?)
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        device.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPressed);

        // We want this to happen only on the press moment (Down), not when the button is held
        if (isTriggerPressed && !wasPressedLastFrame)
        {
            ShootRay();
        }

        wasPressedLastFrame = isTriggerPressed;
    }

    void ShootRay()
    {
        RaycastHit hit;
        // The ray originates from the position of the object this script is attached to (the hand)
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            // Check if we hit a bird
            // Option A: by tag (like we did before)
            if (hit.transform.CompareTag("Bird")) 
            {
                CatchBird(hit.transform.gameObject);
            }
            // Option B: by name (in case you forgot to set the tag)
            else if (hit.transform.name.Contains("cardinal") || hit.transform.name.Contains("Bird"))
            {
                CatchBird(hit.transform.gameObject);
            }
        }
    }

    void CatchBird(GameObject bird)
    {
        Debug.Log("תפסתי ציפור! " + bird.name);
        
        // create hit effect
        if (hitEffect != null)
        {
            Instantiate(hitEffect, bird.transform.position, Quaternion.identity);
        }

        // destroy the bird
        Destroy(bird);
    }
}