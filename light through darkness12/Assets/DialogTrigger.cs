using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public AutoDialog autoDialog; // Ссылка на AutoDialog
    //public DialogData dialogData; // Данные диалога

    private bool playerInRange = false; // Флаг для отслеживания игрока в зоне триггера

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что вошел игрок
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered trigger zone.");

            // Активируем диалоговую панель
        // if (autoDialog != null)
          //  {
            //    autoDialog.dialogueLines = dialogData.dialogueLines; // Передаем данные диалога
              //  autoDialog.StartDialog(); // Запускаем диалог
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Проверяем, что игрок вышел из зоны триггера
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player exited trigger zone.");

            // Отключаем диалоговую панель, если необходимо
            if (autoDialog != null)
            {
                autoDialog.EndDialog();
            }
        }
    }
}
