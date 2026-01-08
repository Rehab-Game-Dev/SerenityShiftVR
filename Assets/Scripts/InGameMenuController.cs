using UnityEngine;
using UnityEngine.InputSystem;

public class InGameMenuController : MonoBehaviour
{
    public GameObject menuObject; // גרור לכאן את Interactive Controls
    public InputActionReference menuButtonAction; // הפניה לכפתור השלט

    void OnEnable() {
        // הרשמה לאירוע הלחיצה
        menuButtonAction.action.performed += ToggleMenu;
    }

    void OnDisable() {
        // הסרת הרשמה למניעת שגיאות
        menuButtonAction.action.performed -= ToggleMenu;
    }

    void ToggleMenu(InputAction.CallbackContext context) {
        // הפיכת מצב התצוגה (אם דולק - כבה, אם כבוי - הדלק)
        bool isActive = !menuObject.activeSelf;
        menuObject.SetActive(isActive);
        
        // בונוס: מיקום התפריט מול השחקן כשהוא נפתח
        if(isActive) {
            menuObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
            menuObject.transform.LookAt(Camera.main.transform);
            menuObject.transform.Rotate(0, 180, 0);
        }
    }
}