using UnityEngine;

public class PCWand : MonoBehaviour
{
    public float range = 100f; // range of the raycast
    private bool hasBeenCaught = false; // prevent double counting

    void Update()
    {
        // Check: was the left mouse button clicked?
        if (Input.GetMouseButtonDown(0)) 
        {
            ShootRay();
        }
    }

    void ShootRay()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            Transform current = hit.transform;
            BirdCatchable birdScript = current.GetComponentInParent<BirdCatchable>();

            // 1. Check Birds
            bool isBird = current.CompareTag("Bird") || (current.parent != null && current.parent.CompareTag("Bird"));
            
            if (isBird || birdScript != null)
            {
                if (birdScript != null)
                {
                    birdScript.CatchBird();
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
}