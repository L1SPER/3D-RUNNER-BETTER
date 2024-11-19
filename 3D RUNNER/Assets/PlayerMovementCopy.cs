using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovementCopy : MonoBehaviour
{
    //declare reference variables
    [SerializeField] Animator animator;
    Rigidbody rb;

    //movement variables
    Vector3 currentMovement;
    Vector3 currentRunMovement;

    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float walkMultiplier;
    [SerializeField]
    float runMultiplier;

    bool isMovementPressed = false;
    bool isRunPressed = false;

    //rotation variables
    [SerializeField]
    float rotationFactorPerFrame;

    //hashes
    int isWalkingHash = Animator.StringToHash("isWalking");
    int isRunningHash = Animator.StringToHash("isRunning");

    //jumping variables
    [SerializeField] float maxJumpHeight;
    [SerializeField] float upTime;
    [SerializeField] float downTime;

    [SerializeField] float timeToJumpAgain;
    [SerializeField] float fallMultiplier;
    float initialJumpVelocity;

    bool isJumpPressed = false;
    bool isJumping = false;

    //gravities
    float groundedGravity = -0.05f;
    [SerializeField] float gravityUp;
    [SerializeField] float gravityDown;

    //isGrounded
    bool isGrounded = false;
    [SerializeField] float groundCheckRadius;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        HandleJumpVariables();
    }
    private void Update()
    {

        HandleRotation();
        HandleAnimation();
        if (isRunPressed && isMovementPressed)
        {
            rb.velocity = currentRunMovement * moveSpeed * Time.deltaTime;
        }
        else if (isMovementPressed)
        {
            rb.velocity = currentMovement * moveSpeed * Time.deltaTime;
        }
        HandleGravity();
        HandleJump();

    }
    private bool isGrounding()
    {
        return isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundCheckRadius, groundLayer);
    }
    private void OnDrawGizmos()
    {
        if (isGrounding())
        {
            Color color = Color.red;
            Gizmos.DrawSphere(groundCheck.transform.position, groundCheckRadius);
        }
    }
    private void HandleJumpVariables()
    {
        gravityUp = 2 * maxJumpHeight / Mathf.Pow(upTime, 2);
        initialJumpVelocity = gravityUp * upTime;

        gravityDown = 2 * maxJumpHeight / Mathf.Pow(downTime, 2);
    }
    private void HandleJump()
    {
        if (isJumpPressed && isGrounded && !isJumping)
        {
            currentMovement.y = initialJumpVelocity * Time.deltaTime;
            currentRunMovement.y = initialJumpVelocity * Time.deltaTime;
            WaitJumpingAgain();
        }
    }
    IEnumerator WaitJumpingAgain()
    {
        isJumping = true;
        yield return new WaitForSeconds(timeToJumpAgain);
        isJumping = false;
    }
    private void HandleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        if (isGrounded)
        {
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            currentMovement.y += gravityDown * Time.deltaTime;
            currentRunMovement.y += gravityDown * Time.deltaTime;
        }
        else
        {
            currentMovement.y += gravityUp * Time.deltaTime;
            currentRunMovement.y += gravityUp * Time.deltaTime;
        }
    }
    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void HandleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }
    public void SetCurrentMovementInput(Vector2 _movementInput)
    {
        currentMovement.x = _movementInput.x;
        currentMovement.z = _movementInput.y;

        currentRunMovement.x = currentMovement.x * runMultiplier;
        currentRunMovement.z = currentMovement.z * runMultiplier;

        isMovementPressed = _movementInput.x != 0 || _movementInput.y != 0;
    }
    public void SetIsRunningPressed(bool _isRunning)
    {
        isRunPressed = _isRunning;
    }
    public void SetIsJumpingPressed(bool _isJumping)
    {
        isJumpPressed = _isJumping;
    }
}
