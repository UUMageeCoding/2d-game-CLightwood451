using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int attackDamage = 20;
    public float attackRange = 1f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;
    private float horizontalInput;
    private bool isGrounded;
    private bool isAttacking = false;
    private float lastAttackTime;
    private float attackCooldown = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded && !isAttacking)
        {
            Jump();
        }
        
        if ((Input.GetMouseButtonDown(0) && !isAttacking))
        {
            Attack();
        }
        
        FlipCharacter();
        
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        if (!isAttacking)
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
            
            Invoke("CheckForEnemyHit", 0.5f);
            Invoke("ResetAttack", 1.0f);
        }
    }

    void CheckForEnemyHit()
    {
        Vector2 attackDirection = isFacingRight ? Vector2.right : Vector2.left;
        Vector2 attackOrigin = (Vector2)transform.position + attackDirection * 0.5f;
        
        Debug.DrawRay(attackOrigin, attackDirection * attackRange, Color.red, 0.5f);
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(attackOrigin, attackDirection, attackRange);
        
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                SwordEnemy enemy = hit.collider.GetComponent<SwordEnemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                }
            }
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    void FlipCharacter()
    {
        if (isAttacking) return;

        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void UpdateAnimations()
    {
        animator.SetBool("IsWalking", Mathf.Abs(horizontalInput) > 0.1f && isGrounded && !isAttacking);
        
        animator.SetBool("IsJumping", !isGrounded);
    }
}