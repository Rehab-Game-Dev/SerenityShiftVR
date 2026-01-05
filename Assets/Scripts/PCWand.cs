using UnityEngine;

public class PCWand : MonoBehaviour
{
    public float range = 100f; // מרחק תפיסה

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
                Debug.Log("PC Player caught a bird!");
                Destroy(hit.transform.gameObject); // השמדת הציפור
            }
        }
    }
}