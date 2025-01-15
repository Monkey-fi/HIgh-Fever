using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerLocomotion playerLocomotion;

    public bool isInteracting;
    public bool isJumping;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }
    
    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement(); // changed FollowTarget to "" and public to "private" in cameramanager 

        isInteracting = animator.GetBool("isInteracting");
        this.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerLocomotion.isGrounded);
    }
}
