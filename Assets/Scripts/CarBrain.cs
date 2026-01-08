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
            // שליחת המכונית לנקודה הראשונה ברגע שהיא נוצרת
            agent.SetDestination(pathPoints[currentPointIndex].position);
        }
    }

    void Update()
    {
        // תיקון 1: קריאה לפונקציית חיישן המרחק בכל פריים
        CheckForObstacles();

        // אם המכונית בבלימה, אל תמשיך ללוגיקה של התנועה
        if (isStopped) return;
        
        // בדיקה שהמסלול תקין
        if (pathPoints == null || pathPoints.Count == 0) return;

        // תיקון 2: שימוש בטווח של 4 מטרים כדי למנוע תקיעות ב-Waypoints
        if (!agent.pathPending && agent.remainingDistance < 4f)
        {
            currentPointIndex++;
            
            if (currentPointIndex < pathPoints.Count)
            {
                // מעבר ליעד הבא ברשימה
                agent.SetDestination(pathPoints[currentPointIndex].position);
            }
            else
            {
                // אם הגענו לסוף המסלול - השמדת המכונית
                Destroy(gameObject);
            }
        }
    }

    void CheckForObstacles()
    {
        RaycastHit hit;
        // מיקום החיישן - קצת מעל פני המכונית וקדימה
        Vector3 sensorStart = transform.position + transform.forward * 1.5f + Vector3.up * 0.5f;
        
        bool obstacleDetected = false;

        // שליחת קרן (Raycast) לבדיקת מכשולים קדימה
        if (Physics.Raycast(sensorStart, transform.forward, out hit, detectionDistance, obstacleLayers))
        {
            // וודא שלא פגענו בעצמנו
            if (hit.collider.gameObject != gameObject)
            {
                obstacleDetected = true;
            }
        }

        if (obstacleDetected)
        {
            // --- בלימת חירום ---
            agent.isStopped = true;       
            agent.velocity = Vector3.zero; 
            agent.angularSpeed = 0f;       // מונע מהמכונית להסתובב סביב עצמה בזמן עצירה
            isStopped = true;
        }
        else
        {
            // --- המשך נסיעה ---
            agent.isStopped = false;
            agent.angularSpeed = 120f;     
            isStopped = false;
        }
    }
}