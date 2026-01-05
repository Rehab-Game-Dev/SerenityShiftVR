using UnityEngine;

public class PCWand : MonoBehaviour
{
    public float range = 100f; // מרחק תפיסה
    private bool hasBeenCaught = false; // למנוע ספירה כפולה

    void Update()
    {
        // בדיקה: האם לחצו קליק שמאלי בעכבר?
        if (Input.GetMouseButtonDown(0)) 
        {
            ShootRay();
        }
    }

    void ShootRay()
    {
        RaycastHit hit;
        
        // יצירת קרן שיוצאת מהמצלמה (האובייקט עליו יושב הסקריפט) ישר קדימה
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            // בדיקה אם פגענו בציפור (לפי התגית שיצרנו קודם)
            if (hit.transform.CompareTag("Bird"))
            {
                // בדיקה שהציפור לא נתפסה כבר
                BirdCatchable birdScript = hit.transform.GetComponent<BirdCatchable>();
                if (birdScript != null && !birdScript.hasBeenCaught)
                {
                    birdScript.CatchBird();
                }
            }
        }
    }
}