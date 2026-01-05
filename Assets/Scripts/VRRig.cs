using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class VRRig : MonoBehaviour
{
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Transform headConstraint;
    public Vector3 bodyOffset = new Vector3(0, -1.5f, 0); // מרחק קבוע לגוף (תשחקי עם ה-Y הזה)

    void LateUpdate() // שינינו את זה כך שלא יסתמך על Start
    {
        // הזזת הגוף לפי המצלמה + התיקון הקבוע למטה
        transform.position = head.vrTarget.position + bodyOffset;

        // סיבוב הגוף שיהיה ישר
        transform.forward = Vector3.ProjectOnPlane(head.vrTarget.forward, Vector3.up).normalized;

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}