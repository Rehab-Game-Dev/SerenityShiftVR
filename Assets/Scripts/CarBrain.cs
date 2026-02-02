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
    public float detectionDistance = 5f; // distance to detect obstacles
    public LayerMask obstacleLayers;     // What are we colliding with? (cars, people)
    public bool isStopped = false;       // Are we currently braking?

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
            // Send the car to the first point as soon as it is created
            agent.SetDestination(pathPoints[currentPointIndex].position);
        }
    }

    void Update()
    {
        // Fix 1: Call the distance sensor function every frame
        CheckForObstacles();

        // If the car is braking, do not continue with the movement logic
        if (isStopped) return;
        
        // Check if the path is valid
        if (pathPoints == null || pathPoints.Count == 0) return;

        // Fix 2: Use a range of 4 meters to prevent getting stuck at Waypoints
        if (!agent.pathPending && agent.remainingDistance < 4f)
        {
            currentPointIndex++;
            
            if (currentPointIndex < pathPoints.Count)
            {
                // Move to the next destination in the list
                agent.SetDestination(pathPoints[currentPointIndex].position);
            }
            else
            {
                // If we reached the end of the path - destroy the car
                Destroy(gameObject);
            }
        }
    }

    void CheckForObstacles()
    {
        RaycastHit hit;
        // Sensor position - slightly above the car and forward
        Vector3 sensorStart = transform.position + transform.forward * 1.5f + Vector3.up * 0.5f;
        
        bool obstacleDetected = false;

        // Send a raycast to check for obstacles ahead
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
            // --- Emergency braking ---
            agent.isStopped = true;       
            agent.velocity = Vector3.zero; 
            agent.angularSpeed = 0f;       // Prevent the car from spinning around itself while stopping
            isStopped = true;
        }
        else
        {
            // --- Continue driving ---
            agent.isStopped = false;
            agent.angularSpeed = 120f;     
            isStopped = false;
        }
    }
}