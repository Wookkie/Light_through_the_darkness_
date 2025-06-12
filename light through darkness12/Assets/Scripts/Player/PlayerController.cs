using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;



    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 8f;

        [Header("Jumping")]
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float fallMultiplier = 2.5f;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private int extraJumpCount = 1;

        [Header("Health")]
        [SerializeField] private int maxHealth = 100;
        private int currentHealth;

        [Header("Shield")]
        public Shield equippedShield = null; // Ссылка на активный щит игрока

        // Взаимодействие с оружием
        public Sword equippedSword = null;   // Меч
        private bool isNearSword = false;    // Флаг, указывающий, находится ли меч рядом
        private bool hasCompletedDialog = false; // Флаг, указывающий, завершён ли диалог
        private bool isAttacking = false; // Флаг состояния атаки

        private Rigidbody2D rb;
        private int m_extraJumps;
        private bool isGrounded;
        private bool isFacingRight = true;
        private float moveInput;
        private bool isMovementFrozen = false;

        // Анимации
        private Animator anim;
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int JumpState = Animator.StringToHash("JumpState");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");

        private List<Enemy> enemiesInRange = new List<Enemy>(); // Список врагов в радиусе атаки

        public bool IsGrounded => isGrounded;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponentInChildren<Animator>();
            m_extraJumps = extraJumpCount;
            currentHealth = maxHealth; // Инициализация текущего здоровья

            // Начально скрываем меч
            if (equippedSword != null)
            {
                equippedSword.gameObject.SetActive(false); // Сделаем меч невидимым до завершения диалога
            }
        }

        public void FreezeMovement()
    {
        isMovementFrozen = true;
        rb.velocity = Vector2.zero; // Останавливаем движение
        anim.SetFloat(Move, 0); // Останавливаем анимацию
    }

     public void UnfreezeMovement()
    {
        isMovementFrozen = false;
    }

        private void Update()
        {

            // Если движение заморожено, не обрабатываем ввод
        if (isMovementFrozen)
            return;

            HandleMovement();
            HandleJumping();
            ManageShield();
            UpdateAnimations();
            HandleAttack(); // Обработка атак в Update

            // Проверка на взятие меча
            if (Input.GetKeyDown(KeyCode.F))
            {
                TryPickUpSword();
            }

            anim.SetBool("HasSword", equippedSword != null);
        }

        private void FixedUpdate()
        {
            // Проверка земли с использованием OverlapCircle
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

            // Падение с ускорением
            if (rb.velocity.y < 0f)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }

            // Для отладки: выводим состояние isGrounded
            UnityEngine.Debug.Log($"isGrounded: {isGrounded}");
        }

        private void HandleMovement()
        {
        if (isMovementFrozen) return; // Если движение заморожено, игнорируем обработку

        moveInput = Input.GetAxisRaw("Horizontal");
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        rb.velocity = new Vector2(moveInput * targetSpeed, rb.velocity.y);

        if ((moveInput > 0 && !isFacingRight) || (moveInput < 0 && isFacingRight))
        {
            Flip();
        }
    }

        private void HandleJumping()
        {
            if (Input.GetButtonDown("Jump") && (isGrounded || m_extraJumps > 0))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                if (!isGrounded)
                {
                    m_extraJumps--;
                }
            }

            if (isGrounded)
            {
                m_extraJumps = extraJumpCount;
            }
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        private void ManageShield()
        {
            if (equippedShield != null && Input.GetKeyDown(KeyCode.Q))
            {
                DropShield();
            }

            if (equippedShield == null && Input.GetButtonDown("PickUpShield"))
            {
                TryPickUpShield();
            }
        }

        private void DropShield()
        {
            if (equippedShield != null)
            {
                equippedShield.DropShield();
                equippedShield = null;
            }
        }

        private void TryPickUpShield()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out Shield shield) && equippedShield == null)
                {
                    equippedShield = shield;
                    equippedShield.EquipShield(transform);
                    UnityEngine.Debug.Log("Shield picked up: " + shield.gameObject.name);
                    return;
                }
            }
            UnityEngine.Debug.Log("No shield nearby");
        }

        private void TryPickUpSword()
        {
            if (!isNearSword || equippedSword != null) return; // Проверяем, может ли игрок взять меч

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Sword"))
                {
                    equippedSword = collider.GetComponent<Sword>();
                    equippedSword.EquipSword(transform);  // Передаем персонажа как родителя для меча

                    // Установка слоя и порядка отрисовки для меча
                    SpriteRenderer swordRenderer = equippedSword.GetComponent<SpriteRenderer>();
                    if (swordRenderer != null)
                    {
                        swordRenderer.sortingLayerName = "Foreground"; // Убедитесь, что это имя слоя действительно существует
                        swordRenderer.sortingOrder = 53; // Установите порядок выше, чем у игрока (например, 52)
                    }

                    UnityEngine.Debug.Log("Sword picked up: " + equippedSword.gameObject.name);
                    return;
                }
            }
        }

        public void TakeDamage(int damage)
        {
            if (equippedShield != null)
            {
                UnityEngine.Debug.Log("Shield blocked the attack");
                return;
            }

            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }

            UnityEngine.Debug.Log("Player takes damage: " + damage + ". Current health: " + currentHealth);
        }

        private void Die()
        {
            UnityEngine.Debug.Log("Player died");
        }

        public void OnDialogCompleted()
        {
            hasCompletedDialog = true;

            // Показать меч только после завершения диалога
            if (equippedSword != null)
            {
                equippedSword.gameObject.SetActive(true); // Активируем меч
            }
            UnityEngine.Debug.Log("Dialog completed. Player can now act.");
        }

        private void UpdateAnimations()
        {
            anim.SetFloat(Move, Mathf.Abs(rb.velocity.x));
            anim.SetFloat(JumpState, rb.velocity.y);
            anim.SetBool(IsJumping, !isGrounded);
        }

        private void HandleAttack()
        {
            if (Input.GetMouseButtonDown(0) && equippedSword != null) // Атака только с мечом
            {
                foreach (var enemy in enemiesInRange)
                {
                    enemy.TakeDamage(equippedSword.damage); // Наносим урон врагу
                    UnityEngine.Debug.Log($"Attacking {enemy.name}");
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Sword"))
            {
                isNearSword = true;
            }

            if (collision.CompareTag("Enemy")) // Если игрок вошёл в зону врага
            {
                var enemy = collision.GetComponent<Enemy>();
                if (enemy != null && !enemiesInRange.Contains(enemy))
                {
                    enemiesInRange.Add(enemy); // Добавляем врага в список
                    UnityEngine.Debug.Log($"Enemy {enemy.name} entered attack range.");
                }
            }

            if (collision.gameObject.TryGetComponent(out Shield shield) && equippedShield == null)
            {
                equippedShield = shield;
                equippedShield.EquipShield(transform);
                UnityEngine.Debug.Log("Shield picked up: " + shield.gameObject.name);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Sword"))
            {
                isNearSword = false; // Устанавливаем флаг в false, когда покидаем триггер
            }

            if (collision.CompareTag("Enemy")) // Если игрок покинул зону врага
            {
                var enemy = collision.GetComponent<Enemy>();
                if (enemy != null && enemiesInRange.Contains(enemy))
                {
                    enemiesInRange.Remove(enemy); // Удаляем врага из списка
                    UnityEngine.Debug.Log($"Enemy {enemy.name} exited attack range.");
                }
            }

            if (collision.gameObject.TryGetComponent(out Shield shield) && equippedShield == shield)
            {
                equippedShield = null; // Удаляем щит, если игрок вышел из триггера
            }
        }
    }

