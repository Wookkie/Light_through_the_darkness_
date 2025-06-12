using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonSound_1_Loc : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public AudioSource audioSource; // Ссылка на AudioSource

    private bool isPointerDown = false; // Флаг нажатия кнопки


    void Update()
    {
        // Если кнопка нажата и удерживается, запускаем звук
        if (isPointerDown)
        {
            // Проверка, нужно ли воспроизвести звук
            if (audioSource != null && !audioSource.isPlaying)
            {
                //audioSource.volume = 0.5f; // Установите желаемую громкость
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
        // Звук будет проигрываться, но ничего не происходит при отпускании кнопки
    }
}
