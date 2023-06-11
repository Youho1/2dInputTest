using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Player paramaters")]
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float jumpForce = 15f;
    [SerializeField, ReadOnly] public Vector2 movement;

    [Header("Dash info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField, ReadOnly] private float dashTime;
    [SerializeField, ReadOnly] private bool isDashing;
    [SerializeField, ReadOnly] private bool isDashCoolTime;
    [SerializeField, ReadOnly] private float dashCoolTimer;
    [SerializeField] private float dashCoolTime;

    private InputManager input;
    protected override void Awake()
    {
        base.Awake();
        input = GetComponent<InputManager>();
    }

    protected override void Update()
    {
        base.Update();
        toJump();
        DashController();
    }
    protected override void handleAttack()
    {
        if (input.isAttackPressed && !isAttacking && isGrounded && !isDashing)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
        }
    }

    private void DashController()
    {
        if (input.isDashPressed && !isDashCoolTime && !isAttacking)
        {
            dashTime = dashDuration;
            dashCoolTimer = dashCoolTime;
        }

        dashTime -= Time.deltaTime;
        dashCoolTimer -= Time.deltaTime;
        isDashing = dashTime > 0 ? true : false;
        isDashCoolTime = dashCoolTimer > 0 ? true : false;
    }

    protected override void handleMovement()
    {
        if (isAttacking)
        {
            rb.velocity = Vector2.zero;
        }
        else if (isDashing)
        {
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(movement.x * runSpeed, rb.velocity.y);
        }
    }

    private void toJump()
    {
        if (isAttacking) return;
        if (input.isJumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    protected override void handleAnimations()
    {
        bool isMoving = animator.GetBool(isMovingHash);
        if (input.isMovementPressed && !isMoving)
        {
            animator.SetBool(isMovingHash, true);
        }
        else if (!input.isMovementPressed && isMoving)
        {
            animator.SetBool(isMovingHash, false);
        }
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isDashing", isDashing);
    }
    protected override void FlipController()
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
}
