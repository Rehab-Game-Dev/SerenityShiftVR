using UnityEngine;
using System.Collections;

public class BirdSpawner : MonoBehaviour
{
    [Header("הגדרות")]
    // שינינו את זה לרשימה (סוגריים מרובעים)
    public GameObject[] birdPrefabs; 
    
    public float spawnInterval = 5f; // כל כמה שניות תצא ציפור?
    public Vector3 spawnArea = new Vector3(20f, 2f, 20f); // גודל האזור ממנו הן יוצאות
    public float birdLifetime = 20f; // זמן חיים קצת יותר ארוך

    void Start()
    {
        StartCoroutine(SpawnBirds());
    }

    IEnumerator SpawnBirds()
    {
        while (true)
        {
            SpawnRandomBird();
            // מחכה זמן אקראי כדי שזה ירגיש טבעי
            yield return new WaitForSeconds(spawnInterval + Random.Range(0f, 3f));
        }
    }

    void SpawnRandomBird()
    {
        // בדיקת בטיחות: אם אין ציפורים ברשימה, אל תעשה כלום
        if (birdPrefabs.Length == 0) return;

        // 1. הגרלת מיקום
        Vector3 randomPos = transform.position + new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            Random.Range(-spawnArea.y, spawnArea.y),
            Random.Range(-spawnArea.z, spawnArea.z)
        );

        // 2. הגרלת כיוון (סיבוב)
        Quaternion randomRot = Quaternion.Euler(0, Random.Range(0, 360), 0);

        // 3. בחירת ציפור אקראית מתוך הרשימה
        int randomIndex = Random.Range(0, birdPrefabs.Length);
        GameObject selectedBird = birdPrefabs[randomIndex];

        // 4. יצירת הציפור
        GameObject newBird = Instantiate(selectedBird, randomPos, randomRot);

        // 5. מחיקה אוטומטית אחרי X שניות
        Destroy(newBird, birdLifetime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, spawnArea * 2);
    }
}