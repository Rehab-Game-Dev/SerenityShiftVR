using UnityEngine;

public class VRAnimatorController : MonoBehaviour
{
    private Animator animator;
    private Vector3 previousPos;
    public float speedThreshold = 0.1f; // סף רגישות לתזוזה

    void Start()
    {
        animator = GetComponent<Animator>();
        previousPos = transform.position;
    }

    void Update()
    {
        // 1. חישוב המהירות של הדמות
        // אנחנו בודקים כמה מרחק עברנו מאז הפריים הקודם
        Vector3 velocity = (transform.position - previousPos) / Time.deltaTime;
        
        // אנחנו מתעלמים מתזוזה לגובה (קפיצות/ירידות) ומתמקדים רק בהליכה
        velocity.y = 0;
        
        float currentSpeed = velocity.magnitude;

        // 2. שליחת המהירות לאנימטור
        // השם "speed" חייב להיות זהה למה שכתוב בתוך ה-Animator
        animator.SetFloat("speed", currentSpeed);

        previousPos = transform.position;
    }
}