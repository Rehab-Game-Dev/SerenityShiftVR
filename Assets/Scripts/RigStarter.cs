using UnityEngine;
using UnityEngine.Animations.Rigging; // חובה בשביל התיקון

public class RigStarter : MonoBehaviour
{
    void Start()
    {
        // אנחנו נותנים למערכת חלקיק שנייה לנשום, ואז מפעילים מחדש
        Invoke("RestartRig", 0.1f);
    }

    void RestartRig()
    {
        // מציאת הרכיב
        RigBuilder rigBuilder = GetComponent<RigBuilder>();
        
        if (rigBuilder != null)
        {
            // הפקודה הזו בונה מחדש את הקשרים ומעלימה את השגיאות
            rigBuilder.Build(); 
        }
    }
}