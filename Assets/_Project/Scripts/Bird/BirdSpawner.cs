using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject[] birdPrefabs; 
    
    public float spawnInterval = 5f; // How often to spawn a bird?
    public Vector3 spawnArea = new Vector3(20f, 2f, 20f); // Size of the area from which birds spawn
    public float birdLifetime = 20f; // How long before a bird is destroyed
    private bool isSpawning = false; // Flag indicating whether we should spawn birds

    private Dictionary<int, List<GameObject>> birdPool = new Dictionary<int, List<GameObject>>();

    void Start()
    {
        // Pre-initialize pools
        for (int i = 0; i < birdPrefabs.Length; i++)
        {
            birdPool[i] = new List<GameObject>();
        }
        StartCoroutine(SpawnBirds());
    }
    
    // Call this method to start spawning birds
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnBirds());
            Debug.Log("Bird spawning started!");
        }
    }

    IEnumerator SpawnBirds()
    {
        while (true)
        {
            SpawnRandomBird();
            // Wait for a random time to make it feel natural
            yield return new WaitForSeconds(spawnInterval + Random.Range(0f, 3f));
        }
    }

    void SpawnRandomBird()
    {
        // safety check to avoid errors (no bird prefabs assigned)
        if (birdPrefabs.Length == 0) return;

        // 1. Randomize position
        Vector3 randomPos = transform.position + new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            Random.Range(-spawnArea.y, spawnArea.y),
            Random.Range(-spawnArea.z, spawnArea.z)
        );

        // 2. Randomize rotation (direction)
        Quaternion randomRot = Quaternion.Euler(0, Random.Range(0, 360), 0);

        // 3. Randomly select a bird from the list
        int randomIndex = Random.Range(0, birdPrefabs.Length);
        GameObject selectedBirdPrefab = birdPrefabs[randomIndex];

        // 4. Get from pool or instantiate
        GameObject bird = GetBirdFromPool(randomIndex, randomPos, randomRot);

        // 5. Start lifetime timer
        StartCoroutine(ReturnToPoolAfterTime(bird, randomIndex, birdLifetime));
    }

    GameObject GetBirdFromPool(int prefabIndex, Vector3 position, Quaternion rotation)
    {
        List<GameObject> pool = birdPool[prefabIndex];
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                GameObject bird = pool[i];
                bird.transform.position = position;
                bird.transform.rotation = rotation;
                bird.SetActive(true);
                return bird;
            }
        }

        GameObject newBird = Instantiate(birdPrefabs[prefabIndex], position, rotation);
        pool.Add(newBird);
        return newBird;
    }

    IEnumerator ReturnToPoolAfterTime(GameObject bird, int prefabIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (bird != null)
        {
            bird.SetActive(false);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, spawnArea * 2);
    }
}