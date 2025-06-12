using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Параметры для передвижения и атаки
    public float moveSpeed = 3f;          // Скорость передвижения врага
    public float chaseRange = 5f;         // Дистанция для начала преследования
    public float attackRange = 1f;        // Дистанция атаки
    public int attackDamage = 10;         // Урон врага
    public float attackCooldown = 1f;     // Время между атаками

    // Параметры для здоровья
    public int maxHealth = 100;           // Максимальное здоровье врага
    private int currentHealth;            // Текущее здоровье врага

    // Параметры патруля
    public Transform[] patrolPoints;      // Точки патруля
    private int currentPatrolIndex = 0;   // Индекс текущей точки патруля
    private bool isPatrolling = true;     // Флаг патрулирования

    // Состояние и ссылки
    private Transform player;             // Ссылка на игрока
    private bool isChasing = false;       // Флаг преследования игрока
    private SpriteRenderer spriteRenderer; // Ссылка на SpriteRenderer

    // Спрайты для левый и правый положения
    public Sprite Mag_Left;  // Спрайт для левого положения
    public Sprite Mag_Right; // Спрайт для правого положения

    private void Start()
    {
        // Инициализация переменных
        player = GameObject.FindGameObjectWithTag("Player")?.transform;  // Ищем игрока по тегу
        spriteRenderer = GetComponent<SpriteRenderer>();  // Инициализируем SpriteRenderer
        currentHealth = maxHealth;  // Устанавливаем здоровье врага
    }

    private void Update()
    {
        if (player == null) return;

        // Если враг в пределах зоны преследования
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            isChasing = true;
            isPatrolling = false;  // Прекращаем патрулирование
        }
        else
        {
            isChasing = false;
            isPatrolling = true;  // Возвращаем патрулирование
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else if (isPatrolling)
        {
            Patrol();
        }
    }

    // Метод для патрулирования
    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        // Передвигаемся к следующей точке патруля
        Transform targetPatrolPoint = patrolPoints[currentPatrolIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetPatrolPoint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPatrolPoint.position) < 0.2f)
        {
            // Переход к следующей точке патруля
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    // Метод преследования игрока
    private void ChasePlayer()
    {
        // Получаем направление движения врага
        Vector2 direction = (player.position - transform.position).normalized;

        // Перемещаемся к игроку
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // Изменяем спрайт в зависимости от направления
        if (direction.x < 0) // Если игрок слева
        {
            spriteRenderer.sprite = Mag_Left;  // Маг слева
        }
        else if (direction.x > 0) // Если игрок справа
        {
            spriteRenderer.sprite = Mag_Right;  // Маг справа
        }
    }

    // Метод для получения урона
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;  // Уменьшаем здоровье

        UnityEngine.Debug.Log("Enemy takes damage: " + damage + ". Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();  // Если здоровье 0 или меньше, враг умирает
        }
    }

    // Метод для смерти врага
    private void Die()
    {
        UnityEngine.Debug.Log("Enemy has died!");
        Destroy(gameObject);  // Удаляем объект врага из сцены
    }
}
