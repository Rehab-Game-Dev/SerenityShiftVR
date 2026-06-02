using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class CarBrain : MonoBehaviour
{
    [Header("NavMesh Settings")]
    private NavMeshAgent agent;
    private List<Transform> pathPoints; 
    private int currentPointIndex = 0;

    [Header("Movement Settings")]
    public float rotationSpeed = 5f;
    public float arrivalThreshold = 1.0f;

    [Header("Sensors (Collision Avoidance)")]
    public float detectionDistance = 5f;
    public LayerMask obstacleLayers;
    public bool isStopped = false;
    private float obstacleCheckTimer = 0f;
    private const float obstacleCheckInterval = 0.1f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.updateRotation = false; // We will handle rotation ourselves for smoothness
        }
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

        obstacleCheckTimer -= Time.deltaTime;
        if (obstacleCheckTimer <= 0f)
        {
            CheckForObstacles();
            obstacleCheckTimer = obstacleCheckInterval;
        }

        // Smooth Rotation
        if (!isStopped && agent.velocity.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (isStopped) return;
        
        if (pathPoints == null || pathPoints.Count == 0) return;

        // "Straight lines": Head to waypoints more precisely by reducing threshold
        if (!agent.pathPending && agent.remainingDistance < arrivalThreshold)
        {
            currentPointIndex++;
            
            if (currentPointIndex < pathPoints.Count)
            {
                agent.SetDestination(pathPoints[currentPointIndex].position);
            }
            else
            {
                // Reached the end
                Destroy(gameObject);
            }
        }
    }

    void CheckForObstacles()
    {
        RaycastHit hit;
        // Sensor position slightly in front and up
        Vector3 sensorStart = transform.position + transform.forward * 1.5f + Vector3.up * 0.5f;
        
        bool obstacleDetected = false;
        if (Physics.Raycast(sensorStart, transform.forward, out hit, detectionDistance, obstacleLayers))
        {
            // Ignore ourselves
            if (hit.collider.gameObject != gameObject)
            {
                obstacleDetected = true;
            }
        }

        if (obstacleDetected)
        {
            if (!isStopped)
            {
                agent.isStopped = true;       
                agent.velocity = Vector3.zero; 
                isStopped = true;
            }
        }
        else
        {
            if (isStopped)
            {
                agent.isStopped = false;
                isStopped = false;
            }
        }
    }
}