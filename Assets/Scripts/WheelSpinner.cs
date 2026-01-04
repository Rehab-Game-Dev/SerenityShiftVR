using UnityEngine;
using UnityEngine.AI;

public class WheelSpinner : MonoBehaviour
{
    [Header("גררי לכאן את 4 הגלגלים מההיררכיה")]
    public Transform[] wheels; // רשימה של הגלגלים
    
    [Header("הגדרות")]
    public float spinSpeed = 100f; // כמה מהר לסובב

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // אם למכונית יש מהירות (היא זזה)
        if (agent != null && agent.velocity.magnitude > 0.1f)
        {
            // חשב את מהירות הסיבוב לפי מהירות הנסיעה
            float currentSpeed = agent.velocity.magnitude * spinSpeed * Time.deltaTime;

            // עבור על כל הגלגלים ברשימה וסובב אותם
            foreach (Transform wheel in wheels)
            {
                if (wheel != null)
                {
                    // מסובב על ציר ה-X (קדימה/אחורה)
                    wheel.Rotate(Vector3.right * currentSpeed);
                }
            }
        }
    }
}