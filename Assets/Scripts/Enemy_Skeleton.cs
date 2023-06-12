using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    [Header("Move Info")] 
    [SerializeField] private float moveSpeed;

    private RaycastHit2D isPlayerDetected;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        RunAI();
    }

    protected override void RunAI()
    {
        if (isPlayerDetected)
        {
                    if (isPlayerDetected.distance > 1)
                    {
                        rb.velocity = new Vector2(moveSpeed * 1.5f * facingDir, rb.velocity.y);
                        Debug.Log("I see the player.");
                        isAttacking = false;
                    }
                    else
                    {
                        Debug.Log("Attack!" +isPlayerDetected.collider.name);
                        isAttacking = true;
                    }
        }
        else
        {
            isAttacking = false;
        }
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();
        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right, playerCheckDistance * facingDir, whatIsPlayer);
    }

    protected override void handleAnimations()
    {
        
    }

    protected override void handleAttack()
    {
        
    }

    protected override void handleMovement()
    {
        if (!isAttacking)
            rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }

    protected override void FlipController()
    {
        if (!isGrounded || isWallDetected)
            Flip();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + playerCheckDistance * facingDir, transform.position.y) );
    }
}