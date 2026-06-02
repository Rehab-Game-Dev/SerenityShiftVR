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
    public Vector3 bodyOffset = new Vector3(0, -1.5f, 0); // constant offset to position the body lower

    void LateUpdate() // use LateUpdate to ensure VR targets have updated
    {
        // Move the body according to the camera + the constant downward offset
        transform.position = head.vrTarget.position + bodyOffset;

        // Rotate the body to be upright
        transform.forward = Vector3.ProjectOnPlane(head.vrTarget.forward, Vector3.up).normalized;

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}