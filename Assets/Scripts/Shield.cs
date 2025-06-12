using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public int durability = 100;      //Стойкость щита (поглощаемый урон)
    private Transform playerTransform;  //Переменная для ссылки на игрока
    private bool isEquipped = false;    //Переменная для проверки одет ли щит

    // Метод экипировки щита
    public void EquipShield(Transform player)
{
    playerTransform = player; //Сохраняем ссылку на трансформ игрока
    transform.SetParent(playerTransform); //Делаем щит дочерним объектом игрока
    transform.localPosition = Vector3.zero; //Устанавливаем локальную позицию в (0, 0, 0)
    transform.localRotation = Quaternion.identity; //Убеждаемся, что вращение щита сброшено
    isEquipped = true; //Устанавливаем что щит экипирован
    Debug.Log("Shield equipped.");
}


    private void Update()
    {
        if (isEquipped)
        {
            SetShieldPosition(); //Обновляем позицию щита каждый кадр
        }
    }

    private void SetShieldPosition()
    {
        if (playerTransform != null) //Проверяем, есть ли ссылка на игрока
        {
            transform.localPosition = new Vector3(0.5f * Mathf.Sign(playerTransform.localScale.x), 0, 0);
            transform.localRotation = Quaternion.identity; //Обнуляем вращение щита относительно игрока
        }
    }

    //Метод для снятия щита
    public void DropShield()
{
    if (isEquipped)
    {
        transform.SetParent(null); //Отсоединяем щит от игрока
        
        //Проверяем, добавлен ли Rigidbody2D
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>(); //Добавляем Rigidbody2D, чтобы щит мог падать
        }

        rb.velocity = Vector2.zero; //Устанавливаем начальную скорость
        rb.gravityScale = 1; //Включаем гравитацию для щита
        playerTransform = null; //Убираем ссылку на трансформ игрока
        isEquipped = false; //Устанавливаем флаг, что щит не экипирован
        Debug.Log("Shield dropped.");
    }
    else
    {
        Debug.LogWarning("Shield is not equipped, cannot drop.");
    }
}


    //Метод для получения урона щитом
    public void TakeDamage(int damage)
    {
        durability -= damage;
        Debug.Log("Shield takes " + damage + " damage. Durability left: " + durability);

        if (durability <= 0)
        {
            BreakShield(); //Вызываем метод разрушения щита
        }
    }

    private void BreakShield()
    {
        Destroy(gameObject); //Разрушаем щит, если его стойкость упала до нуля
        Debug.Log("Shield is destroyed!");
    }
}
