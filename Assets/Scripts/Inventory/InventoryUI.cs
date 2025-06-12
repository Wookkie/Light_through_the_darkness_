using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    public GameObject inventoryPanel;
    public Transform itemsParent;
    public GameObject slotPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Опционально: DontDestroyOnLoad(gameObject);
            Debug.Log("✅ InventoryUI синглтон инициализирован в Awake");
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("❌ Ошибка: inventoryPanel не назначен в инспекторе!");
        }

        if (itemsParent == null)
        {
            Debug.LogError("❌ Ошибка: itemsParent не назначен в инспекторе!");
        }
    }

    public void UpdateUI()
    {
        if (itemsParent == null || Inventory.instance == null)
        {
            Debug.LogError("❌ itemsParent или Inventory не инициализирован!");
            return;
        }

        //if (!inventoryPanel.activeSelf)
        //{
        //    Debug.Log("⚠️ Инвентарь скрыт, обновление UI не выполняется.");
        //    return;
        //}

        Debug.Log("🔁 Обновление UI инвентаря...");

        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in Inventory.instance.items)
        {
            GameObject newSlot = Instantiate(slotPrefab, itemsParent);
            Image slotImage = newSlot.GetComponent<Image>();

            if (item.icon != null)
            {
                slotImage.sprite = item.icon;
                slotImage.color = Color.white;
            }
            else
            {
                slotImage.color = new Color(0, 0, 0, 0);
            }

            Debug.Log("🧱 Добавлена иконка: " + item.Name);
        }
    }
}
