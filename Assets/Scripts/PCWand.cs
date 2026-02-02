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
        
        // Create a ray that shoots straight forward from the camera (the object this script is attached to)
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            // Check if we hit a bird (by the tag we created earlier)
            if (hit.transform.CompareTag("Bird"))
            {
                // Check that the bird hasn't been caught already
                BirdCatchable birdScript = hit.transform.GetComponent<BirdCatchable>();
                if (birdScript != null && !birdScript.hasBeenCaught)
                {
                    birdScript.CatchBird();
                }
            }
        }
    }
}