using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour 
{
    private PlayerInput input;
    [Header("Button Check")]
    [SerializeField, ReadOnly] public bool isAttackPressed;
    [SerializeField, ReadOnly] public bool isMovementPressed;
    [SerializeField, ReadOnly] public bool isJumpPressed;
    [SerializeField, ReadOnly] public bool isDashPressed;
    private Player player;

    protected virtual void Awake()
    {
        input = new PlayerInput();
        player = GetComponent<Player>();
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
        player.movement.x = context.ReadValue<float>();
        isMovementPressed = player.movement.x != 0;
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
