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
    public float detectionDistance = 5f;
    public LayerMask obstacleLayers;
    public bool isStopped = false;
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
            if (agent.isOnNavMesh)
                agent.SetDestination(pathPoints[currentPointIndex].position);
        }
    }
    void Update()
    {
        if (!agent.isOnNavMesh) return;

        CheckForObstacles();

        if (isStopped) return;
        
        if (pathPoints == null || pathPoints.Count == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 4f)
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
        Vector3 sensorStart = transform.position + transform.forward * 1.5f + Vector3.up * 0.5f;
        
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
            agent.isStopped = true;       
            agent.velocity = Vector3.zero; 
            agent.angularSpeed = 0f;
            isStopped = true;
        }
        else
        {
            agent.isStopped = false;
            agent.angularSpeed = 120f;     
            isStopped = false;
        }
    }
}