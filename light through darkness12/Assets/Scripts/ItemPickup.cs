/*using System.Diagnostics;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public bool canBePickedUp = true; // ����, �����������, ����� �� ������� �������
    public float pickupRadius = 2f; // ������, � �������� �������� ����� ����� ��������� �������

    private void Update()
    {
        // ���� ������� ����� ���� �������� � ����� ��������� � ���� �������
        if (canBePickedUp && Vector2.Distance(transform.position, Player.Instance.transform.position) < pickupRadius)
        {
            // ���������� ����������� ������� ������� (��������, ����� ���������)
            ShowPickUpPrompt();
        }
        else
        {
            // ������ ���������, ���� ����� ������
            HidePickUpPrompt();
        }

        // �������� ������� ������� ��� �������� �������� (��������, F)
        if (Input.GetKeyDown(KeyCode.F) && Vector2.Distance(transform.position, Player.Instance.transform.position) < pickupRadius)
        {
            TryPickUpItem(); // ������� �������� ��������
        }
    }

    // ������� ��� ����������� ��������� � ����������� ��������
    private void ShowPickUpPrompt()
    {
        // ��������, ����� ���������� UI ���������
        UnityEngine.Debug.Log("Press F to pick up " + gameObject.name);
    }

    // ������� ��� ������� ���������
    private void HidePickUpPrompt()
    {
        // ������ UI ���������
    }

    // ����� ��� �������� ��������
    public void TryPickUpItem()
    {
        // ������ ��������
        if (canBePickedUp)
        {
            canBePickedUp = false; // ������� ������ ������ ���������
            PickUpItem();
        }
    }

    // �����, ������� ��������� �������� �� ������� ��������
    public void PickUpItem()
    {
        // ������� ����������� (��������, ������������ ������ ������)
        this.transform.SetParent(Player.Instance.itemHoldPosition);  // ���������� ������� � ���� ������
        this.transform.localPosition = Vector3.zero; // ������������� ������� ������������ ��� ������ (�� �������)

        // ����� �������� �������������� �������� (��������, �������� ��� ��������� UI)
        UnityEngine.Debug.Log("Picked up " + gameObject.name);

        // �������������� �������, ����� �� �� �����
        gameObject.SetActive(false); // �����������, ������� ����� ������
    }
}
*/