using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject[] npcPrefabs; 
    public int amount = 20;
    public float range = 50f;
    
    // Optional: Set a uniform scale for all spawned NPCs
    public Vector3 spawnScale = new Vector3(1, 1, 1); 

    void Start()
    {
        SpawnNPCs();
    }

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

        for (int i = 0; i < amount; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * range;
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
            {
                int randomIndex = Random.Range(0, npcPrefabs.Length);
                GameObject selectedPrefab = npcPrefabs[randomIndex];

                GameObject newNPC = Instantiate(selectedPrefab, hit.position, Quaternion.identity);
                
                // determine scale
                newNPC.transform.localScale = spawnScale; 
                
                newNPC.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                newNPC.transform.parent = parentGroup.transform;
            }
        }
    }
}