using UnityEngine;

public class HealthKit : MonoBehaviour
{
    [Header("Health Kit Settings")]
    public int healthRestoreAmount = 20;
    public GameObject collectionEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                bool wasHealed = playerHealth.Heal(healthRestoreAmount);
                
                if (wasHealed)
                {
                    if (collectionEffect != null)
                    {
                        Instantiate(collectionEffect, transform.position, Quaternion.identity);
                    }
                    
                    Destroy(gameObject);
                    
                    Debug.Log("Health kit collected! Restored " + healthRestoreAmount + " health.");
                }
            }
        }
    }
}
