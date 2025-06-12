using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public GameObject inventoryUI; // Панель инвентаря
    [HideInInspector] public bool canOpenInventory = false;

    private bool isOpen = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Опционально: DontDestroyOnLoad(gameObject);
            Debug.Log("✅ InventoryManager instance создан: " + gameObject.name);
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("⌨️ Кнопка I нажата.");
        }

        if (canOpenInventory && Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("✅ canOpenInventory = true и I нажата");
            isOpen = !isOpen;
            inventoryUI.SetActive(isOpen);
            Debug.Log("📂 Inventory toggled: " + isOpen);
        }
    }
}
