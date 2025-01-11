using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool CanMove = true;
    public CharacterController PlayerController;

    [Header("Camera")]
    [SerializeField] private Camera playerCamera;
    public float MouseSensivity = 5f;
    private float XRotation = 0;

    [Header("Walking Params")]
    public float WalkSpeed = 4f;

    [Header("Jump Params")]
    [SerializeField] private float gravity = -9.18f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundCheckMask;
    private bool isGrounded = true;
    [SerializeField] float jumpHieght = 4f;
    private Vector3 velocity;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        Move();
        HandleCamera();
        Jump();
        GravityHandle();    
    }



    private void Move()
    {
        if (!CanMove)
            return;
        Vector3 moveDirection = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
        PlayerController.Move(moveDirection*WalkSpeed*Time.deltaTime);
        
    }
    public void HandleCamera()
    {
        XRotation -= Input.GetAxis("Mouse Y")* MouseSensivity;
        XRotation = Mathf.Clamp(XRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(XRotation, 0f, 0f);
        float YRotation = Input.GetAxis("Mouse X")*MouseSensivity;
        transform.Rotate(new Vector3(0f, YRotation, 0f));
    }
    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHieght * -2 * gravity);
                Debug.Log("Jump Velocity: " + velocity.y);
            }
        }

    }
    private void GravityHandle()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundCheckMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f; // Ensure this is slightly negative to keep the player grounded.
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity.
        PlayerController.Move(velocity * Time.deltaTime); // Apply the velocity.
    }

}
