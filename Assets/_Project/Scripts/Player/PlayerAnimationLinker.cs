using UnityEngine;

public class PlayerAnimationLinker : MonoBehaviour
{
    [Header("Connections")]
    public Animator bodyAnimator;          // the Animator component of the player body
    public CharacterController playerController; // the CharacterController component that moves the player

    [Header("Settings")]
    public string speedParameterName = "Speed"; // speed parameter name in the Animator
    public float animationSmoothTime = 0.1f;    // to make the transition smooth and not jumpy
    void Update()
    {
        // 1. Check the current speed of the player
        // We ignore the Y axis (jumps/falls) and want only horizontal speed
        Vector3 horizontalVelocity = playerController.velocity;
        horizontalVelocity.y = 0;
        
        float currentSpeed = horizontalVelocity.magnitude;

        // 2. Send the speed to the Animator
        // If speed is 0 -> it will do Idle
        // If speed is greater than 0 -> it will transition to Walk or Run
        bodyAnimator.SetFloat(speedParameterName, currentSpeed, animationSmoothTime, Time.deltaTime);
    }
}