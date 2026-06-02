using UnityEngine;
using System.Collections.Generic;
public class TrafficSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs; 
    public List<Transform> myRoute; 
    
    [Header("Timing Settings")]
    public float spawnInterval = 5f; // spawn a car every X seconds
    public float startDelay = 1f;    // how long to wait before the first car! (new)
    void Start()
    {
        InvokeRepeating("SpawnCar", startDelay, spawnInterval);
    }
    void SpawnCar()
    {
        // (The rest of the code remains the same)
        int randomIndex = Random.Range(0, carPrefabs.Length);
        GameObject selectedCar = carPrefabs[randomIndex];
        GameObject newCar = Instantiate(selectedCar, transform.position, transform.rotation);
        CarBrain brain = newCar.GetComponent<CarBrain>();
        if (brain != null)
        {
            brain.SetPath(myRoute);
        }
    }
}