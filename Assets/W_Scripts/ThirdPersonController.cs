using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 6f;
    public float turnSpeed = 10f;
    public float jumpForce = 5f;
    public float crouchSpeed = 1f;

    public CinemachineVirtualCamera virtualCamera;
    public float cameraDistance = 5f;
    public float cameraHeight = 2f;

    // Mouse look variables
    public float mouseSensitivity = 100f;
    private float xRotation = 0f; // Vertical rotation (pitch)
    private float yRotation = 0f; // Horizontal rotation (yaw)

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    private float gravityValue = -9.81f;
    private Animator animator;
    private bool isRunning = false;
    private bool isCrouching = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        if (virtualCamera == null)
        {
            Debug.LogError("There's no Camera assigned to the ThirdPersonController.");
            return;
        }

        virtualCamera.Follow = this.transform;
        virtualCamera.LookAt = this.transform;

        CinemachineTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {
            transposer.m_FollowOffset = new Vector3(0, cameraHeight, -cameraDistance);
        }
    }

    void Update()
    {
        Movement();
        Jumping();
        Crouching();
        MouseLook();
        UpdateAnimations();
    }

    void Movement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0f;

        isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : (isCrouching ? crouchSpeed : walkSpeed);

        controller.Move(move * Time.deltaTime * currentSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = Vector3.Slerp(gameObject.transform.forward, move, Time.deltaTime * turnSpeed);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Jumping()
    {
        if (Input.GetButtonDown("Jump") && groundedPlayer && !isCrouching)
        {
            playerVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * gravityValue);
            animator.SetTrigger("Jump");
        }
    }

    void Crouching()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            animator.SetBool("Crouch", isCrouching);

            controller.height = isCrouching ? 1f : 2f;
            controller.center = new Vector3(0, controller.height / 2f, 0);
        }
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Horizontal rotation (yaw): Rotate the player around the Y axis
        yRotation += mouseX;
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);

        // Vertical rotation (pitch): Rotate the camera around the X axis
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit pitch to avoid camera flip
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void UpdateAnimations()
    {
        Vector3 horizontalMovement = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        float movementMagnitude = horizontalMovement.magnitude;

        animator.SetFloat("Speed", movementMagnitude);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsGrounded", groundedPlayer);
    }
}