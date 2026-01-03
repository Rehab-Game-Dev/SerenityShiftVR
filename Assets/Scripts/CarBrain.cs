using UnityEngine;
using UnityEngine.AI;

public class CarBrain : MonoBehaviour
{
    [Header("Settings")]
    public float sensorLength = 5f; // למרחק כמה מטרים המכונית בודקת מולה?
    public float destroyDistance = 2f; // באיזה מרחק מהסוף למחוק את המכונית?

    [Header("References")]
    public NavMeshAgent agent;
    public Transform destination; // יתקבל אוטומטית מה-Spawner

    void Start()
    {
        // אם לא חיברת ידנית, הוא מוצא לבד
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        
        // אם ה-Spawner נתן יעד, תתחיל לנסוע אליו
        if (destination != null)
        {
            agent.SetDestination(destination.position);
        }
    }

  void Update()
    {
        // --- בדיקת הגנה חדשה ---
        // אם המכונית לא קיימת, או שהיא עדיין לא התחברה לכביש - אל תעשה כלום
        if (agent == null || !agent.isOnNavMesh) return;

        // מכאן ממשיך הקוד הרגיל שלך...
        
        // --- חלק 1: החיישן ---
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position + Vector3.up * 0.5f; 
        bool obstacleDetected = false;
        
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("NPC"))
            {
                obstacleDetected = true;
            }
        }
        agent.isStopped = obstacleDetected;

        // --- חלק 2: מחיקה בסוף ---
        if (destination != null && !agent.pathPending && agent.remainingDistance < destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    // ציור הקרן האדומה ב-Editor כדי שתוכלי לראות אותה
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 sensorStartPos = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawRay(sensorStartPos, transform.forward * sensorLength);
    }
}