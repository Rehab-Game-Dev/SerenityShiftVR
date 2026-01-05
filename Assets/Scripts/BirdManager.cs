using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    // במקום נקודות ריקות, אנחנו גוררים לפה את הציפורים האמיתיות מהסצנה
    public List<GameObject> birdsInScene; 
    public int amountToSpawn = 3;

    void Start()
    {
        ActivateRandomBirds();
    }

    void ActivateRandomBirds()
    {
        // מעתיקים לרשימה זמנית כדי לא להוציא את אותה ציפור פעמיים
        List<GameObject> availableBirds = new List<GameObject>(birdsInScene);

        for (int i = 0; i < amountToSpawn; i++)
        {
            if (availableBirds.Count == 0) break;

            int randomIndex = Random.Range(0, availableBirds.Count);
            
            // במקום ליצור - אנחנו פשוט מדליקים את הציפור
            GameObject selectedBird = availableBirds[randomIndex];
            selectedBird.SetActive(true);

            // ומוציאים אותה מהרשימה הזמנית
            availableBirds.RemoveAt(randomIndex);
        }
    }
}