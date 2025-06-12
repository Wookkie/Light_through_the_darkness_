using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections; // Для IEnumerator и корутин


public class ButtonSound : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public AudioSource audioSource; // Ссылка на AudioSource
    public string levelName; // Имя уровня для загрузки


    private bool isPointerDown = false; // Флаг нажатия кнопки

    void Update()
    {
        // Если кнопка нажата и удерживается, запускаем звук и загружаем уровень
        if (isPointerDown)
        {
            // Проверка, нужно ли загрузить уровень
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play(); // Воспроизводим звук
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true; // Устанавливаем флаг нажатия
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false; // Сбрасываем флаг нажатия

        // Загружаем уровень после отпускания кнопки
        if (audioSource != null && audioSource.isPlaying)
        {
            // Можно подождать, пока звук закончит воспроизводиться (не обязательно)
            StartCoroutine(LoadLevelAfterDelay(audioSource.clip.length));
        }
        else
        {
            // Если звук не воспроизводится, загружаем уровень сразу
            LoadLevel();
        }
    }

    private IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Ждем завершения звука
        LoadLevel(); // Загружаем уровень
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(levelName); // Загружаем новую сцену
    }
}
