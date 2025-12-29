using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementPC : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.2f;

    [Header("Look")]
    public Transform cameraTransform;
    public float mouseSensitivity = 0.12f;
    public float pitchMin = -80f;
    public float pitchMax = 80f;

    [Header("Options")]
    public bool lockCursor = true;

    private CharacterController controller;
    private float verticalVelocity;
    private float pitch;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (cameraTransform == null)
        {
            var cam = Camera.main;
            if (cam != null) cameraTransform = cam.transform;
        }
    }

    void Update()
    {
        HandleLook();
        HandleMove();
    }

    private void HandleLook()
    {
        if (Mouse.current == null || cameraTransform == null) return;

        Vector2 delta = Mouse.current.delta.ReadValue();
        float yaw = delta.x * mouseSensitivity;
        float pitchDelta = delta.y * mouseSensitivity;

        pitch -= pitchDelta;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        transform.Rotate(Vector3.up * yaw);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void HandleMove()
    {
        if (Keyboard.current == null) return;

        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) input.y += 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) input.y -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) input.x += 1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) input.x -= 1f;

        input = Vector2.ClampMagnitude(input, 1f);

        bool sprint = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
        float speed = sprint ? sprintSpeed : moveSpeed;

        Vector3 move = (transform.right * input.x + transform.forward * input.y) * speed;

        bool grounded = controller.isGrounded;
        if (grounded && verticalVelocity < 0f) verticalVelocity = -2f;

        if (grounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }
}