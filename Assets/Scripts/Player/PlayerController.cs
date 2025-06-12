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

    private Rigidbody2D rb;
    private int m_extraJumps;
    private bool isGrounded;
    public bool IsGrounded => isGrounded;
    private bool isFacingRight = true;
    private float moveInput;
    private bool isMovementFrozen = false;

    private Animator anim;
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int JumpState = Animator.StringToHash("JumpState");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");

    public bool canAttack = false;

    private bool isAttacking = false; 
    private bool attackTriggered = false; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        m_extraJumps = extraJumpCount;
    }

    private void Update()
    {
        if (isMovementFrozen) return;

        HandleMovement();
        HandleJumping();
        UpdateAnimations();

        if (Input.GetMouseButtonDown(0) && canAttack && !isAttacking)  
        {
            isAttacking = true;  
            attackTriggered = true; 
            anim.SetTrigger("Attack");  
            Debug.Log("Attack triggered by player, canAttack is true.");
        }

        if (isAttacking && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            isAttacking = false;
            attackTriggered = false; 
            Debug.Log("Attack animation finished.");
        }

        if (attackTriggered && !canAttack)
        {
            Debug.Log("Attack continues after leaving the trigger zone.");
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (rb.velocity.y < 0f)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void HandleMovement()
    {
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
            if (!isGrounded) m_extraJumps--;
        }

        if (isGrounded) m_extraJumps = extraJumpCount;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void UpdateAnimations()
    {
        anim.SetFloat(Move, Mathf.Abs(rb.velocity.x));
        anim.SetFloat(JumpState, rb.velocity.y);
        anim.SetBool(IsJumping, !isGrounded);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "AttackTriggerPoint")  
        {
            canAttack = true;
            Debug.Log("Player entered attack zone. Attack unlocked");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "AttackTriggerPoint") 
        {
            Debug.Log("Player exited attack zone.");
        }
    }
}
