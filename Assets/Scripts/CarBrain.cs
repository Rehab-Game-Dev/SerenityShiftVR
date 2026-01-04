using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class CarBrain : MonoBehaviour
{
    [Header("NavMesh Settings")]
    private NavMeshAgent agent;
    private List<Transform> pathPoints; 
    private int currentPointIndex = 0;

    [Header("Sensors (Collision Avoidance)")]
    public float detectionDistance = 5f; // למרחק כמה מטרים להסתכל קדימה
    public LayerMask obstacleLayers;     // במה אנחנו מתנגשים? (מכוניות, אנשים)
    public bool isStopped = false;       // האם אנחנו בבלימה כרגע

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetPath(List<Transform> newPath)
    {
        pathPoints = newPath;
        currentPointIndex = 0;
        
        if (pathPoints != null && pathPoints.Count > 0)
        {
            agent.SetDestination(pathPoints[currentPointIndex].position);
        }
    }

    void Update()
    {
        // 1. בדיקת חיישנים - האם יש משהו לפנינו?
        CheckForObstacles();

        // אם אנחנו בבלימה, אל תמשיך לחישובי המסלול
        if (isStopped) return;

        // 2. לוגיקת נסיעה רגילה (תחנות)
        if (pathPoints == null || pathPoints.Count == 0) return;

        if (agent.remainingDistance < 2f && !agent.pathPending)
        {
            currentPointIndex++;
            if (currentPointIndex < pathPoints.Count)
            {
                agent.SetDestination(pathPoints[currentPointIndex].position);
            }
            else
            {
                Destroy(gameObject); 
            }
        }
    }

    void CheckForObstacles()
    {
        RaycastHit hit;
        // יורה קרן לייזר בלתי נראית קדימה
        // אנחנו מתחילים אותה טיפה למעלה (0.5) וטיפה קדימה כדי לא לפגוע בעצמנו
        Vector3 sensorStart = transform.position + transform.forward * 1.5f + Vector3.up * 0.5f;

        // ציור הקרן ב-Scene כדי שתוכלי לראות אותה (קו אדום)
        Debug.DrawRay(sensorStart, transform.forward * detectionDistance, Color.red);

        if (Physics.Raycast(sensorStart, transform.forward, out hit, detectionDistance, obstacleLayers))
        {
            // אם פגענו במשהו, והמשהו הזה הוא לא אנחנו
            if (hit.collider.gameObject != gameObject)
            {
                agent.isStopped = true; // עצור!
                isStopped = true;
                return;
            }
        }

        // אם לא פגענו בכלום - שחרר את הבלמים
        agent.isStopped = false;
        isStopped = false;
    }
}