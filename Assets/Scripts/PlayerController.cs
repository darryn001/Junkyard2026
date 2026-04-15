using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;

    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Use this for movement math
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        
    }

    
    void Update()
    {
        if (controller == null || !controller.enabled) return;

        // Reset gravity if on the ground
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get the camera's forward and right vectors
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Calculate direction based on camera orientation
        float forwardInput = Mathf.Clamp(moveInput.y, 0f, 1f);
        Vector3 moveDirection = (forward * forwardInput + right * moveInput.x).normalized;

        // Move the player using moveSpeed (the float)
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Rotate the player to face the direction they are moving
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1f);
        }

        // Apply Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void OnCrouch(InputValue value)
    {
        bool isHolding = value.isPressed;

        if (isHolding)
        {
            controller.height = crouchHeight;
            controller.center = new Vector3(0, crouchHeight / 2, 0);
        }
        else
        {
            controller.height = standingHeight;
            controller.center = new Vector3(0, standingHeight / 2, 0);
        }
    }
}