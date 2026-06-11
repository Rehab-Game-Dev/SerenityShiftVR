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
        // Use the birdLayer mask and ignore triggers if necessary
        if (Physics.Raycast(transform.position, transform.forward, out hit, range, birdLayer))
        {
            Transform current = hit.transform;
            BirdCatchable bird = current.GetComponentInParent<BirdCatchable>();
            
            // 1. Check Birds (check current or any parent for tag)
            bool isBird = current.CompareTag("Bird") || (current.parent != null && current.parent.CompareTag("Bird")) || current.name.Contains("cardinal");
            
            if (isBird || bird != null)
            {
                if (bird != null)
                {
                    bird.CatchBird();
                }
                else
                {
                    // Fallback for simple bird objects
                    CatchBirdFallback(current.gameObject);
                }
            }
            // 2. Check NPCs
            else 
            {
                NPCCollision npc = current.GetComponentInParent<NPCCollision>();
                if (npc != null && (current.CompareTag("NPC") || (current.parent != null && current.parent.CompareTag("NPC"))))
                {
                    if (npc.isCatchable)
                    {
                        npc.CatchNPC();
                    }
                }
            }
        }
    }

    void CatchBirdFallback(GameObject bird)
    {
        Debug.Log("Caught bird (fallback): " + bird.name);
        if (hitEffect != null) Instantiate(hitEffect, bird.transform.position, Quaternion.identity);
        Destroy(bird);
    }
}