using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        // Update health bar on start
        UpdateHealthBar();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        Debug.Log($"Player took {damageAmount} damage! Current health: {currentHealth}");
        
        // Update health bar
        UpdateHealthBar();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Updated Heal method that returns bool to indicate if healing occurred
    public bool Heal(int healAmount)
    {
        if (isDead) return false;
        
        // Check if healing is needed
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
        
        // Update health bar
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
        
        // Disable player controls and movement
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }
        
        // Stop any movement
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
        
        // Disable collider to prevent further interactions
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }
        
        // Create blood puff effect if assigned
        if (bloodPuffEffect != null)
        {
            Instantiate(bloodPuffEffect, transform.position, Quaternion.identity);
        }
        
        // Destroy the player GameObject after a short delay
        Destroy(gameObject, 0.1f);
        
        // Restart the scene after the specified delay
        Invoke("RestartScene", restartDelay);
    }

    void RestartScene()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}