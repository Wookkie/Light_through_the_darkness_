using UnityEngine;

public class Player : MonoBehaviour
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
    public int maxHealth = 100;
    private int currentHealth;

    public Shield equippedShield = null; // Ссылка на активный щит игрока

    private Rigidbody2D rb;
    private int m_extraJumps;
    private bool isGrounded;
    private bool isFacingRight = true;
    private float moveInput;

    // Анимации
    private Animator anim;
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int JumpState = Animator.StringToHash("JumpState");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");

    public bool IsGrounded => isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        m_extraJumps = extraJumpCount;
        currentHealth = maxHealth; // Инициализация текущего здоровья
    }

    private void Update()
    {
        HandleMovement(); // Заменил Move() на HandleMovement()
        Jump();
        ManageShield();
        UpdateAnimations();
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
    }

    void HandleMovement() // Переименовал Move() в HandleMovement()
    {
        moveInput = Input.GetAxisRaw("Horizontal"); // Получаем нажатие клавиш A/D или стрелок
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        rb.velocity = new Vector2(moveInput * targetSpeed, rb.velocity.y);

        // Флип игрока
        if ((moveInput > 0 && !isFacingRight) || (moveInput < 0 && isFacingRight))
        {
            Flip();
        }
    }

    void Jump()
    {
        // Проверка нажатия пробела и нахождения на земле или наличии дополнительных прыжков
        if (Input.GetButtonDown("Jump") && (isGrounded || m_extraJumps > 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if (!isGrounded)
            {
                m_extraJumps--; // Используем дополнительный прыжок
            }
        }

        // Сбрасываем дополнительные прыжки, если игрок на земле
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

    void DropShield()
    {
        if (equippedShield != null)
        {
            equippedShield.DropShield();
            equippedShield = null; // Убираем ссылку на щит
        }
    }

    void TryPickUpShield()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out Shield shield) && equippedShield == null)
            {
                equippedShield = shield;
                equippedShield.EquipShield(transform); // Экипируем щит
                Debug.Log("Shield picked up: " + shield.gameObject.name);
                return;
            }
        }
        Debug.Log("No shield nearby");
    }

    public void TakeDamage(int damage)
{
    if (equippedShield != null)
    {
        // Если у игрока есть щит, блокируем весь урон
        Debug.Log("Shield blocked the attack");
        return;
    }

    // Уменьшаем здоровье игрока
    currentHealth -= damage;

    // Если здоровье упало до 0, игрок умирает
    if (currentHealth <= 0)
    {
        currentHealth = 0;
        Die();
    }

    Debug.Log("Player takes damage: " + damage + ". Current health: " + currentHealth);
}


    private void Die()
    {
        Debug.Log("Player died");
        // Здесь можно добавить анимацию смерти или другие действия при смерти игрока
    }

    private void UpdateAnimations()
    {
        // Анимация движения (Idle & Running)
        anim.SetFloat(Move, Mathf.Abs(rb.velocity.x));

        // Анимация падения и прыжка
        anim.SetFloat(JumpState, rb.velocity.y);

        // Анимация прыжка
        anim.SetBool(IsJumping, !isGrounded);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
