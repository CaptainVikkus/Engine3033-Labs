using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] float WalkSpeed;
    [SerializeField] float RunSpeed;
    [SerializeField] float JumpForce;
    [SerializeField] float LandThreshold = 0.2f;
    [SerializeField] LayerMask groundMask;

    //Controllers
    PlayerController playerController;
    Animator playerAnimator;
    Rigidbody playerRigidbody;
    NavMeshAgent playerNav;

    //Movement
    Vector2 movement = Vector2.zero;
    Vector3 moveDir = Vector3.zero;

    //Animator Hashes
    public readonly int MovementXHash = Animator.StringToHash("MovementX");
    public readonly int MovementYHash = Animator.StringToHash("MovementY");
    public readonly int IsJumpingHash = Animator.StringToHash("IsJumping");
    public readonly int IsRunningHash = Animator.StringToHash("IsRunning");

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerNav = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (playerController.IsJumping) return; //ignore movement while jump

        if (!(movement.magnitude > 0)) { moveDir = Vector3.zero; return; } //no movement

        moveDir = transform.forward * movement.y + transform.right * movement.x;
        float speed = playerController.IsRunning ? RunSpeed : WalkSpeed;

        ///Apply Movement
        //transform.position += moveDir * (speed * Time.deltaTime);
        Vector3 finalMovement = moveDir * (speed * Time.deltaTime);

        playerNav.Move(finalMovement);
    }

    public void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();
        playerAnimator.SetFloat(MovementXHash, movement.x);
        playerAnimator.SetFloat(MovementYHash, movement.y);
    }

    public void OnJump(InputValue value)
    {
        if (playerController.IsJumping) return;

        playerNav.isStopped = true;
        playerNav.enabled = false;


        playerController.IsJumping = value.isPressed;
        playerAnimator.SetBool(IsJumpingHash, value.isPressed);
        playerRigidbody.AddForce((transform.up + moveDir) * JumpForce, ForceMode.Impulse);

        InvokeRepeating(nameof(LandingCheck), 0.5f, 0.1f);
    }
    private void LandingCheck()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, LandThreshold, groundMask))
        {
            playerNav.enabled = true;
            playerNav.isStopped = false;

            playerController.IsJumping = false;
            playerAnimator.SetBool(IsJumpingHash, false);
            CancelInvoke(nameof(LandingCheck));
            playerRigidbody.velocity = Vector3.zero;
        }
    }
    public void OnSprint(InputValue value)
    {
        playerController.IsRunning = value.isPressed;
        playerAnimator.SetBool(IsRunningHash, value.isPressed);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground") && playerController.IsJumping)
    //    {
    //        playerController.IsJumping = false;
    //        playerAnimator.SetBool(IsJumpingHash, false);
    //    }
    //}
}
