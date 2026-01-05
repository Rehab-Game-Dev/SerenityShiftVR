using UnityEngine;

public class PlayerAnimationLinker : MonoBehaviour
{
    [Header("Connections")]
    public Animator bodyAnimator;          // הגוף שעושה את התנועות
    public CharacterController playerController; // הרכיב שמזיז את השחקן

    [Header("Settings")]
    public string speedParameterName = "Speed"; // השם שמצאנו בשלב 1
    public float animationSmoothTime = 0.1f;    // כדי שהמעבר יהיה חלק ולא קופצני

    void Update()
    {
        // 1. בדיקת המהירות הנוכחית של השחקן
        // אנחנו מתעלמים מציר ה-Y (קפיצות/נפילות) ורוצים רק מהירות אופקית
        Vector3 horizontalVelocity = playerController.velocity;
        horizontalVelocity.y = 0;
        
        float currentSpeed = horizontalVelocity.magnitude;

        // 2. שליחת המהירות לאנימטור
        // אם המהירות היא 0 -> הוא יעשה Idle
        // אם המהירות גדולה מ-0 -> הוא יעבור ל-Walk או Run
        bodyAnimator.SetFloat(speedParameterName, currentSpeed, animationSmoothTime, Time.deltaTime);
    }
}