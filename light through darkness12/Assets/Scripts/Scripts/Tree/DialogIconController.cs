using UnityEngine;

public class DialogIconController : MonoBehaviour
{
    public GameObject dialogueIcon;  // ���������� ��� ������ �� ������ �������
    public Canvas canvas;            // ������ �� Canvas

    private Transform playerTransform;  // ��������� ������
    private Transform treeTransform;    // ��������� ������

    void Start()
    {
        // ������� �������� ������
        dialogueIcon.SetActive(false);
    }

    public void StartDialog(Transform talkingObject)
    {
        
        playerTransform = GameObject.FindWithTag("Player").transform;
        treeTransform = talkingObject;  // ��������� ������, � ������� ������� ������

        // ���������� ����� ��� ������������ ������
        Vector3 targetPosition = Vector3.zero;

        // ���������, � ��� ���� ������ (����� ��� ������)
        if (talkingObject == playerTransform)
        {
            // ������� ������ ������������ ������
            targetPosition = playerTransform.position + new Vector3(-0.5f, 1f, 0);  // ���� ����� � ����
        }
        else if (talkingObject == treeTransform)
        {
            // ������� ������ ������������ ������
            targetPosition = treeTransform.position + new Vector3(-0.5f, 1f, 0);  // ���� ����� � ����
        }

        // ����������� ������� ���������� � ��������
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPosition);

        // ������������� ������� ������ �� Canvas
        dialogueIcon.transform.position = screenPos;

        // ���������� ������
        dialogueIcon.SetActive(true);
    }


    public void EndDialog()
    {
        // �������� ������, ����� ������ ��������
        dialogueIcon.SetActive(false);
    }
}
