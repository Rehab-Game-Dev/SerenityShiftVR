using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject birdPrefab; // הציפור שאת רוצה ליצור (חייב להיות פריפאב)
    public List<Transform> spawnPoints; // רשימה של כל ה-15 מקומות
    public int amountToSpawn = 3; // כמה ציפורים ליצור (ביקשת 3)

    void Start()
    {
        SpawnRandomBirds();
    }

    void SpawnRandomBirds()
    {
        // 1. יצירת רשימה זמנית כדי לא להרוס את הרשימה המקורית
        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < amountToSpawn; i++)
        {
            if (availablePoints.Count == 0) break; // הגנה למקרה שאין מספיק נקודות

            // 2. בחירת אינדקס רנדומלי
            int randomIndex = Random.Range(0, availablePoints.Count);
            Transform selectedPoint = availablePoints[randomIndex];

            // 3. יצירת הציפור בנקודה שנבחרה
            Instantiate(birdPrefab, selectedPoint.position, selectedPoint.rotation);

            // 4. מחיקת הנקודה מהרשימה הזמנית כדי שלא תיווצר שם עוד ציפור
            availablePoints.RemoveAt(randomIndex);
        }
    }
}