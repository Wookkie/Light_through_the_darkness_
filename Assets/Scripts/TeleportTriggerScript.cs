using UnityEngine;
using UnityEngine.SceneManagement; // Для работы с переключением сцен

public class TeleportTriggerScript : MonoBehaviour
{
    [SerializeField] private string targetSceneName; //Имя сцены, в которую нужно перейти

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Проверяем, есть ли у объекта тег "Player"
        if (collision.CompareTag("Player"))
        {
            //Загружаем указанную сцену
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
