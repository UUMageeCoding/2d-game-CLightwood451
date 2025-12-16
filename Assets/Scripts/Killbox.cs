using UnityEngine;

public class Killbox : MonoBehaviour
{
    [Header("KillBox Settings")]
    public Collider2D killboxCollider;
    public PlayerHealth playerHealth;

    public void Start()
    {
        killboxCollider = GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D killboxCollider) 
    {
        if (killboxCollider.CompareTag("Player"))
        {
            Debug.Log("You have died!");
            playerHealth.Die();
        }

    }
}
