using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CompassController : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform arrowTransform;

    [Header("Settings")]
    public float updateInterval = 0.2f;

    private Transform playerTransform;
    private float timer;
    private Transform closestTarget;
    private string currentSceneName;

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        FindPlayer();
    }

    void FindPlayer()
    {
        if (Camera.main != null)
        {
            playerTransform = Camera.main.transform;
        }
        else
        {
            GameObject player = GameObject.Find("Player_PC");
            if (player == null) player = GameObject.Find("XR Origin (XR Rig)");
            if (player != null) playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null || !playerTransform.gameObject.activeInHierarchy)
        {
            FindPlayer();
            return;
        }

        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            FindClosestTarget();
            timer = 0;
        }

        UpdateArrow();
    }

    void FindClosestTarget()
    {
        closestTarget = null;
        float minDistance = float.MaxValue;

        // 1. Scene-Specific Overrides
        if (currentSceneName.Contains("hard"))
        {
            // Point to Django (Street Performer)
            var performer = Object.FindFirstObjectByType<StreetPerformerDetection>();
            if (performer != null)
            {
                closestTarget = performer.transform;
                return;
            }
        }
        else if (currentSceneName.Contains("tutorial"))
        {
            // Point to Goal Zone
            var goal = Object.FindFirstObjectByType<GoalZoneTrigger>();
            if (goal != null)
            {
                closestTarget = goal.transform;
                return;
            }
        }

        // 2. Default Logic (NPCs then Birds)
        bool lookForBirds = false;

        if (BirdGameManager.Instance != null)
        {
            lookForBirds = true;
        }
        else if (GameManager.Instance != null)
        {
            lookForBirds = GameManager.Instance.caughtCount >= GameManager.Instance.totalNPCs;
        }
        else
        {
            if (Object.FindFirstObjectByType<BirdCatchable>() != null)
            {
                lookForBirds = true;
            }
        }

        if (lookForBirds)
        {
            BirdCatchable[] birds = Object.FindObjectsByType<BirdCatchable>(FindObjectsSortMode.None);
            foreach (var bird in birds)
            {
                if (bird.gameObject.activeInHierarchy)
                {
                    float dist = Vector3.Distance(playerTransform.position, bird.transform.position);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        closestTarget = bird.transform;
                    }
                }
            }
        }
        else
        {
            NPCCollision[] npcs = Object.FindObjectsByType<NPCCollision>(FindObjectsSortMode.None);
            foreach (var npc in npcs)
            {
                if (npc.gameObject.activeInHierarchy)
                {
                    float dist = Vector3.Distance(playerTransform.position, npc.transform.position);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        closestTarget = npc.transform;
                    }
                }
            }
        }
    }

    void UpdateArrow()
    {
        if (closestTarget == null || arrowTransform == null)
        {
            return;
        }

        Vector3 directionToTarget = closestTarget.position - playerTransform.position;
        directionToTarget.y = 0;

        if (directionToTarget.sqrMagnitude < 0.01f) return;

        Vector3 playerForward = playerTransform.forward;
        playerForward.y = 0;

        float angle = Vector3.SignedAngle(playerForward, directionToTarget, Vector3.up);
        arrowTransform.localRotation = Quaternion.Euler(0, 0, -angle);
    }
}