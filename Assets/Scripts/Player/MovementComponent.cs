using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] float WalkSpeed;
    [SerializeField] float RunSpeed;
    [SerializeField] float JumpForce;

    //Controllers
    PlayerController playerController;
    Animator playerAnimator;
    Rigidbody playerRigidbody;

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
    }

    private void Update()
    {
        if (playerController.IsJumping) return; //ignore movement while jump

        if (!(movement.magnitude > 0)) { moveDir = Vector3.zero; return; } //no movement

        moveDir = transform.forward * movement.y + transform.right * movement.x;
        float speed = playerController.IsRunning ? RunSpeed : WalkSpeed;

        //Apply Movement
        transform.position += moveDir * (speed * Time.deltaTime);
    }

    public void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();
        playerAnimator.SetFloat(MovementXHash, movement.x);
        playerAnimator.SetFloat(MovementYHash, movement.y);
    }

    public void OnJump(InputValue value)
    {
        Debug.Log(value.Get());
        playerController.IsJumping = value.isPressed;
        playerAnimator.SetBool(IsJumpingHash, value.isPressed);

        playerRigidbody.AddForce((transform.up + moveDir) * JumpForce, ForceMode.Impulse);
    }

    public void OnSprint(InputValue value)
    {
        playerController.IsRunning = value.isPressed;
        playerAnimator.SetBool(IsRunningHash, value.isPressed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && playerController.IsJumping)
        {
            playerController.IsJumping = false;
            playerAnimator.SetBool(IsJumpingHash, false);
        }
    }
}
