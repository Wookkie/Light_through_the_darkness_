using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;           // Максимальное здоровье игрока
    private int currentHealth;            // Текущее здоровье игрока

    public bool isDead = false;           // Флаг, что игрок мертв

    private void Start()
    {
        currentHealth = maxHealth;       // Устанавливаем максимальное здоровье
    }

    // Метод для получения урона
    public void TakeDamage(int damage)
    {
        if (isDead) return;               // Если игрок мертв, не получаем урон

        currentHealth -= damage;          // Уменьшаем здоровье на величину урона

        if (currentHealth <= 0)
        {
            Die();                        // Если здоровье стало 0 или меньше, игрок умирает
        }
    }

    // Публичный метод для смерти игрока
    public void Die()
    {
        isDead = true;                    // Устанавливаем флаг смерти
        UnityEngine.Debug.Log("Player has died!");    // Выводим сообщение в консоль
        // Можно добавить анимацию смерти или другие действия (например, перезагрузка уровня)

        // Пример отключения физики игрока
        GetComponent<Rigidbody2D>().simulated = false;  // Если у вас есть Rigidbody2D, его можно отключить
        // Или можно уничтожить объект игрока
        // Destroy(gameObject);
    }
}
