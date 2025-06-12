using UnityEngine;
using TMPro;
using System.Collections;

public class AutoDialog : MonoBehaviour
{
    [Header("Диалоговые строки")]
    public string[] dialogueLines; // Массив строк для настройки через инспектор

    [Header("Элементы UI")]
    public GameObject dialogBox; // Панель с диалогом
    public TextMeshProUGUI dialogText; // Текст диалога
    public GameObject skipButton; // Кнопка пропуска текста

    [Header("Настройки")]
    public float letterPauseTime = 0.05f; // Скорость печати
    public float roleSwitchDelay = 0.5f; // Задержка перед сменой роли

    private int currentLineIndex = 0; // Текущая строка
    private bool isDialogPrinting = false; // Флаг печати текста

    void Start()
    {
        // Отключаем диалоговые элементы в начале
        dialogBox.SetActive(false);
        skipButton.SetActive(false);
    }

    public void StartDialog()
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogError("Диалоговые строки не настроены!");
            return;
        }

        currentLineIndex = 0;
        dialogBox.SetActive(true); // Активируем диалоговую панель
        skipButton.SetActive(true); // Активируем кнопку
        ShowNextLine();
    }

    public void SkipText()
    {
        if (isDialogPrinting)
        {
            // Если текст печатается, выводим его полностью
            StopAllCoroutines();
            dialogText.text = dialogueLines[currentLineIndex];
            isDialogPrinting = false;
        }
        else
        {
            // Если текст завершен, переключаемся на следующую строку
            currentLineIndex++;
            if (currentLineIndex < dialogueLines.Length)
            {
                ShowNextLine();
            }
            else
            {
                EndDialog(); // Завершаем диалог, если строки закончились
            }
        }
    }

    private void ShowNextLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            StartCoroutine(DisplayText(dialogueLines[currentLineIndex]));
        }
        else
        {
            EndDialog();
        }
    }

    private IEnumerator DisplayText(string line)
    {
        isDialogPrinting = true; // Устанавливаем флаг печати
        dialogText.text = ""; // Очищаем текст

        foreach (char letter in line)
        {
            dialogText.text += letter; // Добавляем по одному символу
            yield return new WaitForSeconds(letterPauseTime);
        }

        isDialogPrinting = false; // Печать завершена
    }


    
public void PlayerMadeChoice()
{
    Debug.Log("Игрок сделал выбор.");
    SkipText(); // Переключаемся на следующий текст или завершаем диалог
}
    public void EndDialog()
    {
        dialogBox.SetActive(false); // Скрываем диалоговую панель
        skipButton.SetActive(false); // Скрываем кнопку
        Debug.Log("Диалог завершен.");
    }
}
