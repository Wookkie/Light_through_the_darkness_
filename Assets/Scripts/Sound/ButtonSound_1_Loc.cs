using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonSound_1_Loc : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public AudioSource audioSource; // ������ �� AudioSource

    private bool isPointerDown = false; // ���� ������� ������


    void Update()
    {
        // ���� ������ ������ � ������������, ��������� ����
        if (isPointerDown)
        {
            // ��������, ����� �� ������������� ����
            if (audioSource != null && !audioSource.isPlaying)
            {
                //audioSource.volume = 0.5f; // ���������� �������� ���������
                audioSource.Play(); // ������������� ����
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true; // ������������� ���� �������
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false; // ���������� ���� �������
        // ���� ����� �������������, �� ������ �� ���������� ��� ���������� ������
    }
}
