using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Опционально: DontDestroyOnLoad(gameObject);
            Debug.Log("✅ Inventory синглтон инициализирован в Awake");
        }
        else if (instance != this)
        {
            Destroy(this); // Удаляем дубликат компонента, но не весь GameObject
        }
    }

    public void AddItem(Item newItem)
    {
        items.Add(newItem);
        Debug.Log("📦 Добавлен предмет: " + newItem.Name);

        if (newItem is Sword)
        {
            var playerAttack = FindObjectOfType<PlayerAttack>();
            if (playerAttack != null)
            {
                playerAttack.EquipSword();
                Debug.Log("🗡️ Меч добавлен, состояние обновлено в PlayerAttack.");
            }
        }
    }

    public void RemoveItem(Item itemToRemove)
    {
        items.Remove(itemToRemove);
        Debug.Log("❌ Удален предмет: " + itemToRemove.Name);

        if (itemToRemove is Sword)
        {
            var playerAttack = FindObjectOfType<PlayerAttack>();
            if (playerAttack != null)
            {
                playerAttack.DropSword();
                Debug.Log("🗡️ Меч убран, состояние обновлено в PlayerAttack.");
            }
        }
    }
}
