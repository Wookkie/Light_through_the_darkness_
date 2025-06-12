using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName; // �������� ��������
    public Sprite icon; // ������ ��������
    public bool isStackable; // ����� �� ���������� � ������
    public int maxStack = 1; // ������������ ���������� � ������

    public Item(string name, Sprite sprite, bool stackable, int maxStack)
    {
        itemName = name;
        icon = sprite;
        isStackable = stackable;
        this.maxStack = maxStack;
    }

    // ��������� �������� ��� ��������� �����
    public string Name
    {
        get { return itemName; }
    }
}
