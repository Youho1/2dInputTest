using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : InputManager
{
    Rigidbody2D rb;
    Animator animator;
    [Header("Player paramaters")]
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float jumpForce = 15f;

    [Header("Dash info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField, ReadOnly] private float dashTime;
    [SerializeField, ReadOnly] private bool isDashing;
    [SerializeField, ReadOnly] private bool isDashCoolTime;
    [SerializeField, ReadOnly] private float dashCoolTimer;
    [SerializeField] private float dashCoolTime;

    [Header("Attack info")]
    [SerializeField, ReadOnly] private bool isAttacking;

    private int facingDir;
    private bool facingRight = true;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField, ReadOnly] private bool isGrounded;

    private int isMovingHash;
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        isMovingHash = Animator.StringToHash("isMoving");
    }
    private void Update()
    {
        handleMovement();
        GroundCheck();
        handleAnimations();
        toJump();
        FlipController();
        DashController();
    }

    private void DashController()
    {
        if (isDashPressed && movement.x != 0 && !isDashCoolTime)
        {
            dashTime = dashDuration;
            dashCoolTimer = dashCoolTime;
        }

        dashTime -= Time.deltaTime;
        dashCoolTimer -= Time.deltaTime;
        isDashing = dashTime > 0 ? true : false;
        isDashCoolTime = dashCoolTimer > 0 ? true : false;
    }

    private void GroundCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }


    private void handleMovement()
    {
        if (isDashing && movement.x != 0)
        {
            rb.velocity = new Vector2(movement.x * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(movement.x * runSpeed, rb.velocity.y);
        }
    }

    private void toJump()
    {
        if (isJumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void handleAnimations()
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
        animator.SetBool("isDashing", isDashing);
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
