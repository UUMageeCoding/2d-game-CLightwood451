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
        if (playerHealth != null)
        {
            UpdateHealthBar(playerHealth.currentHealth);
        }
        else
        {
            if (player != null)
            {
                playerHealth = player.GetComponent<PlayerHealth>();
            }
        }
    }

    void UpdateHealthBar(float currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }

        if (fillImage != null)
        {
            float healthPercent = currentHealth / maxHealth;
            fillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercent);
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }

    public void SetHealth(float currentHealth, float maxHealthValue)
    {
        maxHealth = maxHealthValue;
        UpdateHealthBar(currentHealth);
    }
}