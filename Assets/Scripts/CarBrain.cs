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
        // ... (הבדיקה של המכשולים נשארת כאן) ...

        if (isStopped) return;
        if (pathPoints == null || pathPoints.Count == 0) return;

        // התיקון: הגדלנו את הטווח ל-4 מטרים
        // המכונית לא תנסה להגיע בול לאמצע, אלא "תחתוך" יפה לנקודה הבאה
        if (!agent.pathPending && agent.remainingDistance < 4f)
        {
            currentPointIndex++;
            if (currentPointIndex < pathPoints.Count)
            {
                agent.SetDestination(pathPoints[currentPointIndex].position);
            }
            else
            {
                Destroy(gameObject); // או ה-Warp שדיברנו עליו
            }
        }
    }

   void CheckForObstacles()
    {
        RaycastHit hit;
        Vector3 sensorStart = transform.position + transform.forward * 1.5f + Vector3.up * 0.5f;
        
        // בדיקה האם יש מכשול
        bool obstacleDetected = false;

        if (Physics.Raycast(sensorStart, transform.forward, out hit, detectionDistance, obstacleLayers))
        {
            if (hit.collider.gameObject != gameObject)
            {
                obstacleDetected = true;
            }
        }

        if (obstacleDetected)
        {
            // --- בלימת חירום ---
            agent.isStopped = true;       // עצור תנועה
            agent.velocity = Vector3.zero; // אפס את המהירות מיידית
            agent.angularSpeed = 0f;       // שים את ההגה ישר! (מונע סיבוב במקום)
            isStopped = true;
        }
        else
        {
            // --- שחרור בלמים ---
            agent.isStopped = false;
            agent.angularSpeed = 120f;     // החזר את יכולת ההיגוי (120 זה הסטנדרט)
            isStopped = false;
        }
    }
}