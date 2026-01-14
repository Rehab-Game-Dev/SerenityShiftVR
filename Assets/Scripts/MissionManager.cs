using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("Mission 1: People")]
    public GameObject peopleGroup; 
    public GameObject peopleUI;    

    [Header("Mission 2: Birds")]
    public GameObject birdsGroup;  
    public GameObject birdsUI;     

    [Header("Mission 3: Buttons")]
    public GameObject buttonsGroup; 
    public GameObject buttonsUI;    

    void Start()
    {
        SetMission(1);
    }

    public void SetMission(int missionNumber)
    {
        // כיבוי מוחלט של הכל
        peopleGroup.SetActive(false);
        peopleUI.SetActive(false);
        birdsGroup.SetActive(false);
        birdsUI.SetActive(false);
        buttonsGroup.SetActive(false);
        buttonsUI.SetActive(false);

        // הדלקת ה"חבילה" המתאימה
        if (missionNumber == 1) { peopleGroup.SetActive(true); peopleUI.SetActive(true); }
        else if (missionNumber == 2) { birdsGroup.SetActive(true); birdsUI.SetActive(true); }
        else if (missionNumber == 3) { buttonsGroup.SetActive(true); buttonsUI.SetActive(true); }
    }
}