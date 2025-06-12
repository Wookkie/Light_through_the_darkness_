using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public bool isDead = false;

    private RespawnManager respawnManager;

    [Header("Damage Settings")]
    public float damageInvulnerabilityDuration = 1f; 
    private float invulnerabilityTimer = 0f;

    private void Start()
    {
        currentHealth = maxHealth;
        respawnManager = FindObjectOfType<RespawnManager>();
    }

    private void Update()
    {
        if (invulnerabilityTimer > 0)
            invulnerabilityTimer -= Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        if (isDead || invulnerabilityTimer > 0) return;

        currentHealth -= damage;
        invulnerabilityTimer = damageInvulnerabilityDuration; 

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        Debug.Log("Player has died!");

        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<SpriteRenderer>().enabled = false; 
        GetComponent<Collider2D>().enabled = false;

        if (respawnManager != null)
        {
            respawnManager.RespawnPlayer(this);
        }
    }

    public void Revive(Vector3 respawnPoint)
    {
        currentHealth = maxHealth;
        isDead = false;

        transform.position = respawnPoint;
        GetComponent<Rigidbody2D>().simulated = true;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }
}
