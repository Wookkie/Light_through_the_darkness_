using UnityEngine;
using UnityEngine.UI;

public class DeathZone : MonoBehaviour
{
    public GameObject deathPanel; // UI панель с текстом и кнопкой
    public Transform respawnPoint; // Точка респавна
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Находим игрока по тегу
        if (player == null)
        {
            Debug.LogError("Игрок не найден! Убедитесь, что у персонажа установлен тег 'Player'.");
        }
        if (deathPanel == null)
        {
            Debug.LogError("Панель смерти не назначена! Укажите её в Inspector.");
        }

        deathPanel.SetActive(false); // Скрываем панель смерти в начале
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Обнаружено столкновение с: " + collision.gameObject.name); // Проверяем, с кем столкнулись
        if (collision.CompareTag("Player"))
        {
            Death();
        }
    }

    void Death()
{
    Debug.Log("Игрок умер! Включаем панель смерти...");
    deathPanel.SetActive(true); // Показываем сообщение о смерти
    player.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Останавливаем движение
    player.SetActive(false); // Отключаем игрока
}

    public void Respawn()
    {
        Debug.Log("Респавн!"); // Проверяем, вызывается ли функция
        player.transform.position = respawnPoint.position; // Перемещаем игрока на старт
        player.SetActive(true); // Включаем игрока обратно
        deathPanel.SetActive(false); // Скрываем панель смерти
    }
}
