using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCWalker : MonoBehaviour
{
    [Header("Patrol points in order")]
    public Transform[] waypoints;

    [Header("Animation")]
    public Animator animator;
    [Tooltip("Name of the float parameter in the Animator")]
    public string speedParam = "speed";
    public float animDamp = 0.12f;

    private NavMeshAgent agent;
    private int index;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (!animator) animator = GetComponentInChildren<Animator>();

        // חשוב כשמשתמשים ב-NavMesh להזזה:
        if (animator) animator.applyRootMotion = false;
    }

    void Start()
    {
        if (waypoints != null && waypoints.Length > 0)
            GoTo(index);
    }

    void Update()
    {
        // לעדכן אנימציה לפי מהירות אמיתית
        if (animator)
        {
            float normalized = agent.speed > 0.01f ? agent.velocity.magnitude / agent.speed : 0f;
            animator.SetFloat(speedParam, normalized, animDamp, Time.deltaTime);
        }

        // להתקדם לנקודה הבאה כשהגענו
        if (waypoints == null || waypoints.Length == 0) return;
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.05f)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude < 0.01f)
            {
                index = (index + 1) % waypoints.Length;
                GoTo(index);
            }
        }
    }

    void GoTo(int i)
    {
        if (waypoints[i] != null)
            agent.SetDestination(waypoints[i].position);
    }
}
