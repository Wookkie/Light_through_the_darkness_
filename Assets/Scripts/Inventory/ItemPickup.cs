using UnityEngine;
using System.Collections;  // Для корутин

public class ItemPickup : MonoBehaviour
{
    public Item item;
    private bool playerInRange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (item != null)
            {
                Debug.Log("Подобран предмет: " + item.Name);

                // Добавление предмета в инвентарь
                Inventory.instance.AddItem(item);

                // Проверка и ожидание полной инициализации InventoryUI перед обновлением
                if (InventoryUI.instance != null)
                {
                    StartCoroutine(WaitForUIInitialization());
                }
                else
                {
                    Debug.LogError("❌ Ошибка: InventoryUI не инициализировано!");
                }

                // Удаляем предмет с мира (или скрываем его)
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("❌ Ошибка: В ItemPickup не назначен предмет!");
            }
        }
    }

    private IEnumerator WaitForUIInitialization()
    {
        // Ожидание, пока InventoryUI и itemsParent не будут инициализированы
        while (InventoryUI.instance.itemsParent == null)
        {
            yield return null;
        }

        // Теперь можно безопасно обновить UI
        Debug.Log("UI инвентаря готово, обновляем...");
        InventoryUI.instance.UpdateUI();
    }
}
