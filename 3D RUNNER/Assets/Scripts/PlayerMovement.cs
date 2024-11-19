using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    //References
    Rigidbody rb;
    [SerializeField] Animator animator;
    //Movement variables
    [SerializeField] float moveSpeed;
    [SerializeField] float walkMultiplier;
    [SerializeField] float runMultiplier;

    bool isMoveButtonPressed;
    bool isRunButtonPressed;
    bool isJumpButtonPressed;

    bool isWalking;
    bool isRunning;
    bool isJumping;

    Vector3 movementSpeed;
    Vector3 runningMovementSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Debug.Log("run =" + isRunButtonPressed);
        Debug.Log("move =" + isMoveButtonPressed);

        HandleMovement();
    }

    private void HandleMovement()
    {
        if(isRunButtonPressed && isMoveButtonPressed)
        {
            rb.velocity=runningMovementSpeed*Time.fixedDeltaTime;
        }
        else if (isMoveButtonPressed)
        {
            rb.velocity=movementSpeed*Time.fixedDeltaTime;
        }
    }

    public void SetCurrentMovementInput(Vector2 _movementInput)
    {
        movementSpeed.x = _movementInput.x;
        movementSpeed.z = _movementInput.y;

        runningMovementSpeed.x = _movementInput.x* runMultiplier;
        runningMovementSpeed.z = _movementInput.y* runMultiplier;

        isMoveButtonPressed = _movementInput.x != 0 || _movementInput.y != 0;
    }
    public void SetMoveButtonPressed(bool _isMoveButtonPressed)
    {
        isMoveButtonPressed = _isMoveButtonPressed;
    }
    public void SetRunButtonPressed(bool _isRunButtonPressed)
    {
        isRunButtonPressed = _isRunButtonPressed;
    }
    public void SetJumpButtonPressed(bool _isJumpButtonPressed)
    {
        isJumpButtonPressed = _isJumpButtonPressed;
    }
}