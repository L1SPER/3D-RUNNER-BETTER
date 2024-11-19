using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInputs playerInputs;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInputs = new PlayerInputs();
        playerInputs.Player.Enable();

        playerInputs.Player.Walk.started += Walk;
        playerInputs.Player.Walk.performed += Walk;
        playerInputs.Player.Walk.canceled += Walk;

        playerInputs.Player.Run.started += Run;
        playerInputs.Player.Run.performed += Run;
        playerInputs.Player.Run.canceled += Run;

        playerInputs.Player.Jump.started += Jump;
        playerInputs.Player.Jump.performed += Jump;
        playerInputs.Player.Jump.canceled += Jump;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        playerMovement.SetJumpButtonPressed(context.ReadValueAsButton());
    }

    private void Run(InputAction.CallbackContext context)
    {
        playerMovement.SetRunButtonPressed(context.ReadValueAsButton());
    }
    private void Walk(InputAction.CallbackContext context)
    {
        playerMovement.SetCurrentMovementInput(context.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        playerInputs.Player.Enable();
    }
    private void OnDisable()
    {
        playerInputs.Player.Disable();
    }
}
