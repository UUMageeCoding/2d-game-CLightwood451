using UnityEngine;

public class SwordEnemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float moveSpeed = 2f;
    public float attackRange = 1.5f;
    public float chaseRange = 5f;
    public int attackDamage = 20;
    public float attackCooldown = 2f;
    public int maxHealth = 100;

    [Header("Death Effect")]
    public GameObject bloodPuffEffect;
    
    [Header("References")]
    public Transform player;
    
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D enemyCollider;
    private bool isFacingRight = true;
    private float lastAttackTime;
    private bool isAttacking = false;
    private Vector2 movementDirection;
    private bool isChasing = false;
    private int currentHealth;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        currentHealth = maxHealth;
        
        if (player == null)
        {
            FindPlayer();
        }
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            if (player == null)
            {
                StopMoving();
                return;
            }
        }

        if (isAttacking || isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange)
        {
            isChasing = true;
            ChasePlayer();
        }
        else if (distanceToPlayer <= attackRange)
        {
            isChasing = false;
            AttackPlayer();
        }
        else
        {
            isChasing = false;
            StopMoving();
        }

        FlipTowardsPlayer();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        if (player == null || isAttacking || isDead) return;

        if (isChasing)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
        }
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void ChasePlayer()
    {
        // Empty for now
    }

    void StopMoving()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    void AttackPlayer()
    {
        StopMoving();

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;

            Invoke("DealDamage", 0.5f);
            Invoke("ResetAttack", 1f);
        }
    }

    void DealDamage()
    {
        if (player == null) 
        {
            ResetAttack();
            return;
        }

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    void FlipTowardsPlayer()
    {
        if (player == null) return;

        bool playerIsRight = player.position.x > transform.position.x;
        
        if (!isAttacking && playerIsRight != isFacingRight)
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
        animator.SetBool("IsWalking", isChasing && !isAttacking && !isDead);
    }

    // Health system for the enemy
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        Debug.Log($"SwordEnemy took {damage} damage! Current health: {currentHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("SwordEnemy has died!");
        
        StopMoving();
        CancelInvoke();

        if (bloodPuffEffect != null)
        {
            Instantiate(bloodPuffEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}