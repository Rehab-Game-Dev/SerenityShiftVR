using UnityEngine;

public class GuitarHandIK : MonoBehaviour
{
    public Animator animator;
    public Transform leftHandTarget;
    [Range(0,1)] public float weight = 1f;

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null || leftHandTarget == null)
            return;

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weight);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
    }
}
