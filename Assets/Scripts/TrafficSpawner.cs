using UnityEngine;
using System.Collections.Generic;

public class TrafficSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs; 
    public List<Transform> myRoute; 
    
    [Header("Timing Settings")]
    public float spawnInterval = 5f; // כל כמה זמן יוצאת מכונית
    public float startDelay = 1f;    // כמה זמן לחכות לפני המכונית הראשונה! (החדש)

    void Start()
    {
        // השינוי הוא כאן: במקום 1f קבוע, אנחנו משתמשים במשתנה startDelay
        InvokeRepeating("SpawnCar", startDelay, spawnInterval);
    }

    void SpawnCar()
    {
        // (שאר הקוד נשאר אותו דבר)
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