using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject[] npcPrefabs;
    public int amount = 20;
    public float range = 50f;
    public Vector3 spawnScale = new Vector3(1, 1, 1);

    [Header("Obstacle Avoidance")]
    public LayerMask buildingLayer;
    public float clearanceRadius = 0.5f;

    void Start() => SpawnNPCs();

    [ContextMenu("Spawn NPCs Now")]
    public void SpawnNPCs()
    {
        if (npcPrefabs == null || npcPrefabs.Length == 0)
        {
            Debug.LogError("Please assign at least one NPC Prefab!");
            return;
        }

        GameObject parentGroup = new GameObject("Generated_NPCs");
        parentGroup.transform.position = transform.position;

        int spawned = 0;
        int attempts = 0;
        int maxAttempts = amount * 10;

        while (spawned < amount && attempts < maxAttempts)
        {
            attempts++;
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * range;

            NavMeshHit hit;
            if (!NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
                continue;

            Vector3 checkLow = hit.position + Vector3.up * 0.5f;
            Vector3 checkMid = hit.position + Vector3.up * 1.5f;

            if (Physics.CheckSphere(checkLow, clearanceRadius, buildingLayer) ||
                Physics.CheckSphere(checkMid, clearanceRadius, buildingLayer))
                continue;

            GameObject newNPC = Instantiate(
                npcPrefabs[Random.Range(0, npcPrefabs.Length)],
                hit.position,
                Quaternion.Euler(0, Random.Range(0, 360), 0)
            );
            newNPC.transform.localScale = spawnScale;
            newNPC.transform.parent = parentGroup.transform;
            spawned++;
        }

        if (spawned < amount)
            Debug.LogWarning($"NPCSpawner: Only spawned {spawned}/{amount} NPCs.");
    }
}