using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    private bool isAttacking = false;
    public bool hasSword = false;

    private PlayerController m_controller;

    private void Start()
    {
        m_controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Устанавливаем параметр CanAttack в аниматоре
        if (animator != null && m_controller != null)
        {
            animator.SetBool("CanAttack", m_controller.canAttack);
        }

        // Атака: если флаг canAttack равен true и игрок нажал кнопку атаки
        if (Input.GetMouseButtonDown(0) && hasSword && !isAttacking)
        {
            if (m_controller != null && m_controller.canAttack)
            {
                TryAttack();
            }
        }
    }

    public void TryAttack()
    {
        Debug.Log($"TryAttack() called. canAttack={m_controller.canAttack}, hasSword={hasSword}, isAttacking={isAttacking}");

        if (hasSword && !isAttacking && m_controller != null && m_controller.canAttack)
        {
            Debug.Log("Attack triggered");
            isAttacking = true;
            animator.SetTrigger("Attack");
        }
        else
        {
            Debug.Log("Attack blocked: conditions not met.");
        }
    }



    public void ResetAttack()
    {
        isAttacking = false;
    }

    public void EquipSword()
    {
        hasSword = true;
    }

    public void DropSword()
    {
        hasSword = false;
    }

    public void DealDamageToEnemies()
    {
        // Логируем перед атакой
        Debug.Log("Attempting to deal damage to enemies.");

        // Находим всех врагов в радиусе 1.5 метра от игрока
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1.5f, LayerMask.GetMask("Enemy"));

        // Для каждого найденного врага наносим урон
        foreach (Collider2D hit in hitEnemies)
        {
            // Проверяем, что объект является врагом
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Наносим урон врагу
                enemy.TakeDamage(20);  // В данном случае урон равен 20
                Debug.Log("Damage dealt to enemy: " + enemy.name);
            }
        }
    }

}
