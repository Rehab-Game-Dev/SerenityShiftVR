using UnityEngine;

public class VRAnimatorController : MonoBehaviour
{
    private Animator animator;
    private Vector3 previousPos;
    public float speedThreshold = 0.1f; // threshold to consider as "moving"

    void Start()
    {
        animator = GetComponent<Animator>();
        previousPos = transform.position;
    }

    void Update()
    {
        // 1. Calculate the character's speed
        // We check how much distance we've covered since the last frame
        Vector3 velocity = (transform.position - previousPos) / Time.deltaTime;
        
        // We ignore vertical movement (jumps/falls) and focus only on walking
        velocity.y = 0;
        
        float currentSpeed = velocity.magnitude;

        // 2. Send the speed to the Animator
        // The name "speed" must match what is written inside the Animator
        animator.SetFloat("speed", currentSpeed);

        previousPos = transform.position;
    }
}