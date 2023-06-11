using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput input;
    [Header("Button Check")]
    [SerializeField, ReadOnly] protected bool isAttackPressed;
    [SerializeField, ReadOnly] protected bool isMovementPressed;
    [SerializeField, ReadOnly] protected bool isJumpPressed;
    [SerializeField, ReadOnly] protected bool isDashPressed;

    [SerializeField, ReadOnly] protected Vector2 movement;

    protected virtual void Awake()
    {
        input = new PlayerInput();

        input.CharacterControls.Movement.performed += onMovement;
        input.CharacterControls.Jump.started += onJump;
        input.CharacterControls.Jump.canceled += onJump;
        input.CharacterControls.Dash.started += onDash;
        input.CharacterControls.Dash.canceled += onDash;
        input.CharacterControls.Attack.started += onAttack;
        input.CharacterControls.Attack.canceled += onAttack;
    }

    protected void onAttack(InputAction.CallbackContext context)
    {
        isAttackPressed = context.ReadValueAsButton();
    }
    protected void onDash(InputAction.CallbackContext context)
    {
        isDashPressed = context.ReadValueAsButton();
    }
    protected void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }
    protected void onMovement(InputAction.CallbackContext context)
    {
        movement.x = context.ReadValue<float>();
        isMovementPressed = movement.x != 0;
    }

    protected virtual void OnEnable()
    {
        input.CharacterControls.Enable();
    }

    protected virtual void OnDisable()
    {
        input.CharacterControls.Disable();
    }
}
