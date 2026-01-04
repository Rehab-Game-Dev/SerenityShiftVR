using UnityEngine;

public class BirdFly : MonoBehaviour
{
    [Header("הגדרות טיסה")]
    public float flySpeed = 15f;     // מהירות הטיסה
    public float wobbleAmount = 1f; // רעידות קלות למעלה למטה (שלא ייראה כמו טיל)
    
    private float randomOffset;

    void Start()
    {
        // נותן לכל ציפור נקודת התחלה שונה לרעידות
        randomOffset = Random.Range(0f, 10f);
    }

    void Update()
    {
        // 1. תזוזה קדימה בלבד (לכיוון שהיא מסתכלת)
        transform.Translate(Vector3.forward * flySpeed * Time.deltaTime);

        // (הורדנו את שורת ה-Rotate שהייתה כאן קודם)

        // 2. תנועה גלית קלה למעלה ולמטה (Wobble) כדי שירגיש חי
        float wobble = Mathf.Sin(Time.time + randomOffset) * wobbleAmount * Time.deltaTime;
        transform.Translate(Vector3.up * wobble);
    }
}