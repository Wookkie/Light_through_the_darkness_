using System.Diagnostics;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource; // ������ �� AudioSource
    public AudioClip location1Clip; // ���� ��� ������ �������
    public AudioClip location2Clip; // ���� ��� ������ �������

    // ����� ��� ��������� ������� � ��������������� �����
    public void SetLocationSound(int location)
    {
        switch (location)
        {
            case 1:
                PlayLocationSound(location1Clip); // ������������� ���� ��� ������ �������
                break;
            case 2:
                PlayLocationSound(location2Clip); // ������������� ���� ��� ������ �������
                break;
            default:
                UnityEngine.Debug.LogWarning("������� �� �������!"); // ��������, ���� ������� �� �������������
                break;
        }
    }

    // ����� ��� ��������������� �����
    public void PlayLocationSound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.Stop(); // ������������� ������� ����
            audioSource.clip = clip; // ������������� ����� ����
            audioSource.Play(); // ������������� ����� ����
        }
        else
        {
            UnityEngine.Debug.LogWarning("AudioSource ��� AudioClip �� ��������� � SoundManager!");
        }
    }
}
