using UnityEngine;
using UnityEngine.XR; // Required for accessing XR controller data

public class VRWand : MonoBehaviour
{
    [Header("Settings")]
    // Defines which hand (Left/Right) this script should listen to
    public XRNode controllerNode = XRNode.RightHand; 
    
    // The maximum distance the ray can travel
    public float range = 100f; 
    
    // Optional: Filter to interact only with objects on the bird layer
    public LayerMask birdLayer; 

    [Header("Visuals")]
    // Optional: Visual effect spawned at the point of impact
    public GameObject hitEffect; 

    private bool isTriggerPressed = false;
    // Tracks the previous frame state to detect a "single click" instead of continuous firing
    private bool wasPressedLastFrame = false; 

    void Update()
    {
        // 1. Input Handling: Access the specific XR device (controller)
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        
        // 2. Feature Detection: Check if the trigger button is currently being pressed
        device.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPressed);

        // 3. Debouncing Logic: Execute only on the frame the button was first pressed (Button Down)
        if (isTriggerPressed && !wasPressedLastFrame)
        {
            ShootRay();
        }

        // Store current state for comparison in the next frame
        wasPressedLastFrame = isTriggerPressed;
    }

    void ShootRay()
    {
        RaycastHit hit;
        // Physics Raycast: Casts an invisible line from the controller's position forward
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            // Interaction Logic: Check if the object hit by the ray is a "Bird"
            
            // Method A: Check using Unity Tags (Standard practice)
            if (hit.transform.CompareTag("Bird")) 
            {
                CatchBird(hit.transform.gameObject);
            }
            // Method B: Fallback check using naming conventions
            else if (hit.transform.name.Contains("cardinal") || hit.transform.name.Contains("Bird"))
            {
                CatchBird(hit.transform.gameObject);
            }
        }
    }

    void CatchBird(GameObject bird)
    {
        Debug.Log("Bird Captured: " + bird.name);
        
        // Spawn visual feedback at the target's location
        if (hitEffect != null)
        {
            Instantiate(hitEffect, bird.transform.position, Quaternion.identity);
        }

        // Remove the target object from the scene
        Destroy(bird);
    }
}