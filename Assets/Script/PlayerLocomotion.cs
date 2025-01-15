using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    [Header("falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public LayerMask groundLayer;
    public float rayCastHeightOffSet = 0.5f;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5;
    public float sprintingSpeed = 7;
    public float rotationSpeed = 5;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = 9.81f; // always negative 

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform; // scan scene that is tagged main camaera
    }

    public void HandleAllMovement()
    {
        HandleFallingandLanding();

        if (playerManager.isInteracting)
            return;

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting)
        {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else{
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDirection = moveDirection * runningSpeed;
            }
            else
            {
                moveDirection = moveDirection * walkingSpeed;
            }
        }

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        if (isJumping)
            return;

        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = playerRotation;
    }

    private void HandleFallingandLanding()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y = raycastOrigin.y + rayCastHeightOffSet;

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }
            inAirTimer = inAirTimer + Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(raycastOrigin, 0.2f, -Vector3.up, out hit, rayCastHeightOffSet + 0.1f, groundLayer))//added rayCastHeightOffSet + 0.1f //for  jump error
        {
            if (!isGrounded && !playerManager.isInteracting){
                animatorManager.PlayTargetAnimation("Land", true);  
            }

            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void HandleJumping()
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = playerRigidbody.velocity;// changes moveDirection; to playerRigidbody.velocity; // for jump error
            playerVelocity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelocity;
        }
    }
}
