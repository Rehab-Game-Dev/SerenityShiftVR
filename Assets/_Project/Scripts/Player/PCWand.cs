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
            // 1. Check Birds
            if (hit.transform.CompareTag("Bird"))
            {
                BirdCatchable birdScript = hit.transform.GetComponent<BirdCatchable>();
                if (birdScript != null)
                {
                    birdScript.CatchBird();
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
}