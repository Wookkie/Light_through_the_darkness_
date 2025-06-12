using UnityEngine;

public class TeleportZone : MonoBehaviour
{
    public string sceneToLoad; // Название сцены, задаётся в инспекторе

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Проверяем, что вошёл игрок
        {
            SceneTransition transition = FindObjectOfType<SceneTransition>();
            if (transition != null)
            {
                transition.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogError("Ошибка: объект SceneTransition не найден в сцене!");
            }
        }
    }
}
