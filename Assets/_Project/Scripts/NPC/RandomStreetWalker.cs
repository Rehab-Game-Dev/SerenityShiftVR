using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RandomStreetWalker : MonoBehaviour
{
    [Header("Settings")]
    public float walkRadius = 20f; // how far it can walk from the current point
    public float waitTime = 3f;    // how long to wait when it reaches the destination before continuing

    [Header("Animation")]
    public Animator animator;
    public string speedParam = "speed"; // the name of the parameter in your Animator

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();
        
        // Start the timer so it begins walking immediately
        timer = waitTime;
    }

    void Update()
    {
        // 1. Handle animation (Idle vs Walk)
        // Send the speed to the Animator so it knows when to play Walk and when to play Idle
        if (animator != null)
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat(speedParam, speed);
        }

        // 2. Handle random movement
        timer += Time.deltaTime;

        // if wait time has passed + the character reached the destination (or has no destination)
        if (timer >= waitTime && (!agent.hasPath || agent.remainingDistance < 0.5f))
        {
            Vector3 newPos = RandomNavSphere(transform.position, walkRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    // navmesh random position generator
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        
        return navHit.position;
    }
}