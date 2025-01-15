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
    public float stepHeight = 2;
    public float stepDistance = 1; 

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

    //private void HandleMovement()
    // {
    //     moveDirection = cameraObject.forward * inputManager.verticalInput;
    //     moveDirection += cameraObject.right * inputManager.horizontalInput;
    //     moveDirection.Normalize();
    //     moveDirection.y = 0;

    //     float speed = isSprinting ? sprintingSpeed : (inputManager.moveAmount >= 0.5f ? runningSpeed : walkingSpeed);
    //     Vector3 movementVelocity = moveDirection * speed;

    //     // Step handling
    //     if (isGrounded && movementVelocity != Vector3.zero)
    //     {
    //         RaycastHit hit;
    //         Vector3 origin = transform.position + Vector3.up * stepHeight; // stepHeight is the maximum height of a step the player can climb
    //         if (Physics.Raycast(origin, moveDirection, out hit, stepDistance)) // stepDistance is how far ahead to check for a step
    //         {
    //             float stepHeightDifference = hit.point.y - transform.position.y;
    //             if (stepHeightDifference > 0 && stepHeightDifference <= stepHeight)
    //             {
    //                 // Move player up to the step height
    //                 playerRigidbody.position += Vector3.up * stepHeightDifference;
    //             }
    //         }
    //     }

    //     playerRigidbody.velocity = new Vector3(movementVelocity.x, playerRigidbody.velocity.y, movementVelocity.z);
    // }
    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection += cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = isSprinting ? sprintingSpeed : (inputManager.moveAmount >= 0.5f ? runningSpeed : walkingSpeed);
        Vector3 movementVelocity = moveDirection * speed;

        // Step handling
        if (isGrounded && movementVelocity != Vector3.zero)
        {
            RaycastHit hitLower;
            RaycastHit hitUpper;
            Vector3 originLower = transform.position + Vector3.up * 0.1f; // Slightly above ground
            Vector3 originUpper = transform.position + Vector3.up * stepHeight; // At step height

            if (Physics.Raycast(originLower, moveDirection, out hitLower, stepDistance) &&
                !Physics.Raycast(originUpper, moveDirection, out hitUpper, stepDistance))
            {
                // Smoothly move up to the step height
                float stepAdjustment = Mathf.MoveTowards(playerRigidbody.velocity.y, stepHeight, Time.deltaTime * speed);
                playerRigidbody.velocity = new Vector3(movementVelocity.x, stepAdjustment, movementVelocity.z);
            }
            else
            {
                playerRigidbody.velocity = new Vector3(movementVelocity.x, playerRigidbody.velocity.y, movementVelocity.z);
            }
        }
        else
        {
            playerRigidbody.velocity = new Vector3(movementVelocity.x, playerRigidbody.velocity.y, movementVelocity.z);
        }
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
        Vector3 targetPosition;
       // raycastOrigin.y = raycastOrigin.y + rayCastHeightOffSet;
        targetPosition = transform.position;

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
            if (!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);  
            }

            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;   
            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        //if (isGrounded && !isJumping)
        //{
        //    if (playerManager.isInteracting || inputManager.moveAmount > 0)
        //    {
        //        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
        //    }

        //    else 
        //    {
        //        transform.position = targetPosition;
        //    }
        //}
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
