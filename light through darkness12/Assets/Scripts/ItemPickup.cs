/*using System.Diagnostics;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public bool canBePickedUp = true; // Флаг, указывающий, можно ли поднять предмет
    public float pickupRadius = 2f; // Радиус, в пределах которого игрок может подобрать предмет

    private void Update()
    {
        // Если предмет может быть подобран и игрок находится в зоне радиуса
        if (canBePickedUp && Vector2.Distance(transform.position, Player.Instance.transform.position) < pickupRadius)
        {
            // Отображаем возможность поднять предмет (например, через интерфейс)
            ShowPickUpPrompt();
        }
        else
        {
            // Прячем подсказку, если игрок далеко
            HidePickUpPrompt();
        }

        // Проверка нажатия клавиши для поднятия предмета (например, F)
        if (Input.GetKeyDown(KeyCode.F) && Vector2.Distance(transform.position, Player.Instance.transform.position) < pickupRadius)
        {
            TryPickUpItem(); // Попытка поднятия предмета
        }
    }

    // Функция для отображения подсказки о возможности поднятия
    private void ShowPickUpPrompt()
    {
        // Например, можно отобразить UI подсказку
        UnityEngine.Debug.Log("Press F to pick up " + gameObject.name);
    }

    // Функция для скрытия подсказки
    private void HidePickUpPrompt()
    {
        // Прячем UI подсказку
    }

    // Метод для поднятия предмета
    public void TryPickUpItem()
    {
        // Подъем предмета
        if (canBePickedUp)
        {
            canBePickedUp = false; // Предмет больше нельзя поднимать
            PickUpItem();
        }
    }

    // Метод, который выполняет действие по подъему предмета
    public void PickUpItem()
    {
        // Предмет поднимается (например, родительский объект игрока)
        this.transform.SetParent(Player.Instance.itemHoldPosition);  // Перемещаем предмет в руки игрока
        this.transform.localPosition = Vector3.zero; // Устанавливаем позицию относительно рук игрока (по желанию)

        // Можно добавить дополнительные действия (например, анимацию или изменение UI)
        UnityEngine.Debug.Log("Picked up " + gameObject.name);

        // Деактивировать предмет, чтобы он не мешал
        gameObject.SetActive(false); // Опционально, предмет можно скрыть
    }
}
*/