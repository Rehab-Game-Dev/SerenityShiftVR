using UnityEngine;
using UnityEngine.AI;

public class WheelSpinner : MonoBehaviour
{
    [Header("Drag the 4 wheels from the hierarchy here")]
    public Transform[] wheels; // list of wheel transforms to spin
    
    [Header("Settings")]
    public float spinSpeed = 100f; // how fast to spin

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // only spin the wheels if the agent is moving
        if (agent != null && agent.velocity.magnitude > 0.1f)
        {
            // calculate the spin speed based on the driving speed
            float currentSpeed = agent.velocity.magnitude * spinSpeed * Time.deltaTime;

            // iterate over all wheels in the list and rotate them
            foreach (Transform wheel in wheels)
            {
                if (wheel != null)
                {
                    // rotate around the X axis (forward/backward)
                    wheel.Rotate(Vector3.right * currentSpeed);
                }
            }
        }
    }
}