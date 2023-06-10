using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    [Header("Player Paramaters")]
    [SerializeField] float runSpeed = 6f;
    [SerializeField] float jumpForce = 6f;
    [SerializeField, ReadOnly] Vector2 movement;
    private int facingDir;
    private bool facingRight = true;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField, ReadOnly] private bool isGrounded;
    bool isMovementPressed;
    bool isJumpPressed;
    private int isMovingHash;
    PlayerInput input;
    private void Awake()
    {
        input = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        isMovingHash = Animator.StringToHash("isMoving");
        input.CharacterControls.Movement.performed += onMovement;
        input.CharacterControls.Jump.started += onJump;
        input.CharacterControls.Jump.canceled += onJump;
    }
    private void Update()
    {          
        handleMovement();
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        handleAnimation();
        toJump();
        
        FlipController();
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }
    void onMovement(InputAction.CallbackContext context)
    {

        movement.x = context.ReadValue<float>() * runSpeed;
        isMovementPressed = movement.x != 0;
    }

    void handleMovement()
    {
        rb.velocity = new Vector2(movement.x, rb.velocity.y);
    }


    void toJump()
    {
        if (isJumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void handleAnimation()
    {
        bool isMoving = animator.GetBool(isMovingHash);
        if (isMovementPressed && !isMoving)
        {
            animator.SetBool(isMovingHash, true);
        }
        else if (!isMovementPressed && isMoving)
        {
            animator.SetBool(isMovingHash, false);
        }
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    private void OnEnable()
    {
        input.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        input.CharacterControls.Disable();
    }

    private void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
