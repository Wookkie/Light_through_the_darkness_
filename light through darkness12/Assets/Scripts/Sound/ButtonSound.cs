using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections; // ��� IEnumerator � �������


public class ButtonSound : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public AudioSource audioSource; // ������ �� AudioSource
    public string levelName; // ��� ������ ��� ��������


    private bool isPointerDown = false; // ���� ������� ������

    void Update()
    {
        // ���� ������ ������ � ������������, ��������� ���� � ��������� �������
        if (isPointerDown)
        {
            // ��������, ����� �� ��������� �������
            if (audioSource != null && !audioSource.isPlaying)
            {
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

        // ��������� ������� ����� ���������� ������
        if (audioSource != null && audioSource.isPlaying)
        {
            // ����� ���������, ���� ���� �������� ���������������� (�� �����������)
            StartCoroutine(LoadLevelAfterDelay(audioSource.clip.length));
        }
        else
        {
            // ���� ���� �� ���������������, ��������� ������� �����
            LoadLevel();
        }
    }

    private IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // ���� ���������� �����
        LoadLevel(); // ��������� �������
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(levelName); // ��������� ����� �����
    }
}
