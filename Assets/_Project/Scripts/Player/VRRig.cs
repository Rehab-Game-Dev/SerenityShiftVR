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
    public Vector3 bodyOffset = new Vector3(0, 1.2f, 0);

    void LateUpdate()
    {
        // Only follow head horizontally, let gravity handle Y
        Vector3 newPos = head.vrTarget.position + bodyOffset;
        transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);

        transform.forward = Vector3.ProjectOnPlane(head.vrTarget.forward, Vector3.up).normalized;
        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}