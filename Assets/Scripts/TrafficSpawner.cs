using UnityEngine;
using System.Collections.Generic;

public class TrafficSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs; // רשימת סוגי מכוניות
    public List<Transform> myRoute; // המסלול שהספונר הזה אחראי עליו
    public float spawnInterval = 5f;

    void Start()
    {
        InvokeRepeating("SpawnCar", 1f, spawnInterval);
    }

    void SpawnCar()
    {
        // בחר מכונית אקראית
        int randomIndex = Random.Range(0, carPrefabs.Length);
        GameObject selectedCar = carPrefabs[randomIndex];

        // צור את המכונית במיקום של הספונר
        GameObject newCar = Instantiate(selectedCar, transform.position, transform.rotation);

        // תן למכונית את המסלול שמוגדר בספונר הזה
        CarBrain brain = newCar.GetComponent<CarBrain>();
        if (brain != null)
        {
            brain.SetPath(myRoute);
        }
    }
}