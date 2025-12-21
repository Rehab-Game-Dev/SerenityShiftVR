using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RandomStreetWalker : MonoBehaviour
{
    [Header("Settings")]
    public float walkRadius = 20f; // כמה רחוק הוא יכול ללכת מהנקודה הנוכחית
    public float waitTime = 3f;    // כמה זמן לחכות כשהוא מגיע ליעד לפני שממשיך

    [Header("Animation")]
    public Animator animator;
    public string speedParam = "speed"; // השם של הפרמטר באנימטור שלך

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();
        
        // מתחיל את הטיימר כדי שיתחיל ללכת מיד
        timer = waitTime;
    }

    void Update()
    {
        // 1. טיפול באנימציה (עמידה מול הליכה)
        // מעביר את המהירות לאנימטור כדי שידע מתי לנגן Walk ומתי Idle
        if (animator != null)
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat(speedParam, speed);
        }

        // 2. טיפול בתנועה הרנדומלית
        timer += Time.deltaTime;

        // אם עבר זמן ההמתנה + הדמות הגיעה ליעד (או אין לה יעד)
        if (timer >= waitTime && (!agent.hasPath || agent.remainingDistance < 0.5f))
        {
            Vector3 newPos = RandomNavSphere(transform.position, walkRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    // פונקציה למציאת נקודה חוקית על ה-NavMesh (המשטח הכחול)
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        
        return navHit.position;
    }
}