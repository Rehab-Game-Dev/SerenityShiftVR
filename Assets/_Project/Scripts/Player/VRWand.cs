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
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            // 1. Check Birds
            if (hit.transform.CompareTag("Bird") || hit.transform.name.Contains("cardinal"))
            {
                BirdCatchable bird = hit.transform.GetComponent<BirdCatchable>();
                if (bird != null)
                {
                    bird.CatchBird();
                }
                else
                {
                    // Fallback for simple bird objects
                    CatchBirdFallback(hit.transform.gameObject);
                }
            }
            // 2. Check NPCs
            else if (hit.transform.CompareTag("NPC") || hit.transform.GetComponent<NPCCollision>() != null)
            {
                NPCCollision npc = hit.transform.GetComponent<NPCCollision>();
                if (npc != null && npc.isCatchable)
                {
                    npc.CatchNPC();
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