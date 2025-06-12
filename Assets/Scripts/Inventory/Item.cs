using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName; // Название предмета
    public Sprite icon; // Иконка предмета
    public bool isStackable; // Можно ли складывать в стопки
    public int maxStack = 1; // Максимальное количество в стопке

    public Item(string name, Sprite sprite, bool stackable, int maxStack)
    {
        itemName = name;
        icon = sprite;
        isStackable = stackable;
        this.maxStack = maxStack;
    }

    // Добавляем свойство для получения имени
    public string Name
    {
        get { return itemName; }
    }
}
