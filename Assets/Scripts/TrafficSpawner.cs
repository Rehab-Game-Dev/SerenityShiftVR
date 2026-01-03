using UnityEngine;
using System.Collections;

public class TrafficSpawner : MonoBehaviour
{
    // שים לב: אלו סוגריים מרובעים [] שאומרים שזו רשימה
    public GameObject[] carPrefabs; 
    
    public Transform destination;   
    public float spawnInterval = 3f; 

    void Start()
    {
        StartCoroutine(SpawnCarsRoutine());
    }

    IEnumerator SpawnCarsRoutine()
    {
        while (true) 
        {
            SpawnRandomCar(); // קריאה לפונקציה החדשה
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnRandomCar()
    {
        // ודא שיש בכלל מכוניות ברשימה כדי לא לקבל שגיאה
        if (carPrefabs.Length == 0) return;

        // 1. הגרלה: בחר מספר אקראי בין 0 למספר המכוניות שיש
        int randomIndex = Random.Range(0, carPrefabs.Length);
        GameObject selectedCar = carPrefabs[randomIndex];

        // 2. צור את הרכב שנבחר
        GameObject newCar = Instantiate(selectedCar, transform.position, transform.rotation);

        // 3. תן לו את היעד
        CarBrain brain = newCar.GetComponent<CarBrain>();
        if (brain != null)
        {
            brain.destination = this.destination;
        }
    }
}