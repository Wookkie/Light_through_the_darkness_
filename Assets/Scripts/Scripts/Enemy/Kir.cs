using UnityEngine;

public class Kir : MonoBehaviour
{
    public float moveSpeed = 2f;          // Скорость движения
    public float chaseRange = 5f;         // Дистанция для преследования игрока

    private Transform player;              // Ссылка на игрока
    private SpriteRenderer spriteRenderer; // Ссылка на SpriteRenderer
    private Animator animator;              // Ссылка на Animator

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            // Останавливаем анимацию, если игрок вне диапазона
            animator.SetBool("isMovingLeft", false);
            animator.SetBool("isMovingRight", false);
        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        UpdateAnimation(direction);
    }

    private void UpdateAnimation(Vector2 direction)
    {
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true; // Движение влево
            animator.SetBool("isMovingLeft", true);
            animator.SetBool("isMovingRight", false);
        }
        else if (direction.x > 0)
        {
            spriteRenderer.flipX = false; // Движение вправо
            animator.SetBool("isMovingRight", true);
            animator.SetBool("isMovingLeft", false);
        }
    }
}
