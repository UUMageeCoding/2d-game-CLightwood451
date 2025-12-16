using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Death Settings")]
    public GameObject bloodPuffEffect;
    public float restartDelay = 2f;

    [Header("UI Reference")]
    public HealthBarUI healthBarUI;

    private Movement movementScript;
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        movementScript = GetComponent<Movement>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        
        UpdateHealthBar();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        Debug.Log($"Player took {damageAmount} damage! Current health: {currentHealth}");
        
        UpdateHealthBar();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public bool Heal(int healAmount)
    {
        if (isDead) return false;
        
        if (currentHealth >= maxHealth)
        {
            Debug.Log("Player is already at full health!");
            return false;
        }
        
        int oldHealth = currentHealth;
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        int actualHealAmount = currentHealth - oldHealth;
        Debug.Log($"Player healed {actualHealAmount}! Current health: {currentHealth}");
        
        UpdateHealthBar();
        
        return true;
    }

    void UpdateHealthBar()
    {
        if (healthBarUI != null)
        {
            healthBarUI.SetHealth(currentHealth, maxHealth);
        }
    }

    public void Die()
    {
        isDead = true;
        Debug.Log("Player has died!");
        
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }
        
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
        
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }
        
        if (bloodPuffEffect != null)
        {
            Instantiate(bloodPuffEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
        
        Invoke("RestartScene", restartDelay);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}