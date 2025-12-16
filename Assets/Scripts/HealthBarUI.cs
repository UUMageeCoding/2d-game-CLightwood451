using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    public Slider healthSlider;
    public Image fillImage;
    public Text healthText;
    public GameObject player;

    [Header("Color Settings")]
    public Color fullHealthColor = Color.green;
    public Color lowHealthColor = Color.red;
    public float lowHealthThreshold = 0.3f; // 30% health

    private PlayerHealth playerHealth;
    private float maxHealth;

    void Start()
    {
        // If player is not assigned, try to find it
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                maxHealth = playerHealth.maxHealth;
                UpdateHealthBar(playerHealth.currentHealth);
            }
        }
    }

    void Update()
    {
        // Update health bar if player health component exists
        if (playerHealth != null)
        {
            UpdateHealthBar(playerHealth.currentHealth);
        }
        else
        {
            // Try to find player health component if we lost reference
            if (player != null)
            {
                playerHealth = player.GetComponent<PlayerHealth>();
            }
        }
    }

    void UpdateHealthBar(float currentHealth)
    {
        // Update slider value
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }

        // Update fill color based on health percentage
        if (fillImage != null)
        {
            float healthPercent = currentHealth / maxHealth;
            fillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercent);
        }

        // Update text if assigned
        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }

    // Public method to update health bar from external scripts
    public void SetHealth(float currentHealth, float maxHealthValue)
    {
        maxHealth = maxHealthValue;
        UpdateHealthBar(currentHealth);
    }
}