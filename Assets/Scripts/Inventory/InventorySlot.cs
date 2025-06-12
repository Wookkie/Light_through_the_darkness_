using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;

    public void SetItem(Item item)
    {
        if (item != null && item.icon != null)
        {
            icon.sprite = item.icon;
            icon.color = Color.white;
        }
        else
        {
            icon.color = new Color(0, 0, 0, 0); // Прозрачный
        }
    }

    public void ClearSlot()
    {
        icon.sprite = null;
        icon.color = new Color(0, 0, 0, 0); // Прозрачный
    }
}
