using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float chaseRange = 5f;
    public float attackRange = 1f;
    public int attackDamage = 10;
    public float attackCooldown = 1f;
    public int maxHealth = 100;
    private int currentHealth;
    private Transform player;
    private bool isChasing = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float attackTimer = 0f;
    private Vector3 initialPosition;


    private void Start()
    {
        initialPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (player == null) return;

        attackTimer -= Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
            if (IsInAttackRange() && attackTimer <= 0f)
            {
                AttackPlayer();
            }
        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        spriteRenderer.flipX = direction.x > 0; // Поворот в зависимости от направления
    }

    private bool IsInAttackRange()
    {
        return Vector2.Distance(transform.position, player.position) <= attackRange;
    }

    private void AttackPlayer() //в листинге обновить
    {
        attackTimer = attackCooldown;
        animator.SetTrigger("Attack");

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died");
        // Логика смерти врага (например, удаление объекта)
        //Destroy(gameObject);
        gameObject.SetActive(false); // вместо Destroy
    }

    private void OnCollisionStay2D(Collision2D collision) //в листинге добавить
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.isDead)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }

    public void Revive() //в листинге добавить
    {
        Debug.Log("Revive вызван. Восстановление здоровья врага.");
        currentHealth = maxHealth;
        gameObject.SetActive(true);
        transform.position = initialPosition;
    

        // Можно также сбросить позицию, анимации, и т.д.
        // animator.Play("Idle"); // при необходимости
    }

}
