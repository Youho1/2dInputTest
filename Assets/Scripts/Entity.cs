using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;
    protected int isMovingHash;

    [Header("Collision info")] 
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField, ReadOnly] protected bool isGrounded;

    [Header("Attack info")]
    [SerializeField, ReadOnly] protected bool isAttacking = false;

    protected int facingDir = 1;
    protected bool facingRight = true;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        isMovingHash = Animator.StringToHash("isMoving");
    }

    protected virtual void Update()
    {
        handleMovement();
        handleAttack();
        GroundCheck();
        handleAnimations();
        FlipController();
    }
    protected virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    // abstract method
    protected abstract void handleAttack();
    protected abstract void handleMovement();
    protected abstract void handleAnimations();
    protected abstract void FlipController();
    // Attack Animation End
    public void AttackOver()
    {
        isAttacking = false;
    }
    // ground check
    protected virtual void GroundCheck()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position,Vector2.down, groundCheckDistance, whatIsGround);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
    }
}
