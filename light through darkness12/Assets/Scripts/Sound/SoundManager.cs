using System.Diagnostics;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource; // Ссылка на AudioSource
    public AudioClip location1Clip; // Звук для первой локации
    public AudioClip location2Clip; // Звук для второй локации

    // Метод для установки локации и воспроизведения звука
    public void SetLocationSound(int location)
    {
        switch (location)
        {
            case 1:
                PlayLocationSound(location1Clip); // Воспроизводим звук для первой локации
                break;
            case 2:
                PlayLocationSound(location2Clip); // Воспроизводим звук для второй локации
                break;
            default:
                UnityEngine.Debug.LogWarning("Локация не найдена!"); // Логируем, если локация не соответствует
                break;
        }
    }

    // Метод для воспроизведения звука
    public void PlayLocationSound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.Stop(); // Останавливаем текущий звук
            audioSource.clip = clip; // Устанавливаем новый звук
            audioSource.Play(); // Воспроизводим новый звук
        }
        else
        {
            UnityEngine.Debug.LogWarning("AudioSource или AudioClip не назначены в SoundManager!");
        }
    }
}
