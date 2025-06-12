using System.Collections;
using UnityEngine;
using TMPro; // Для работы с TextMeshPro

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel; // Панель с диалогом
    public TextMeshProUGUI dialogueText; // TextMeshPro элемент
    public GameObject skipButton; // Кнопка для пропуска текста

    public string[] dialogueLines; // Массив строк с фразами
    public float typingSpeed = 0.05f; // Скорость печати текста

    private int currentLineIndex = 0; // Текущая строка
    private bool isTyping = false; // Флаг, идет ли печать текста

    void Start()
{
    currentLineIndex = 0;
    dialoguePanel.SetActive(true); // Активируем панель
    skipButton.SetActive(true); // Показываем кнопку
    StartCoroutine(TypeText(dialogueLines[currentLineIndex])); // Начинаем диалог
}


    public void StartDialogue()
{
    Debug.Log("StartDialogue called"); // Это выведется в консоль
    dialoguePanel.SetActive(true);
    skipButton.SetActive(true);
    currentLineIndex = 0;
    StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
}

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        dialogueText.text = ""; // Очищаем текст
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter; // Добавляем по одному символу
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    public void SkipText()
{
    if (isTyping)
    {
        // Если текст печатается, моментально выводим всю строку
        StopAllCoroutines();
        dialogueText.text = dialogueLines[currentLineIndex]; // Показываем текущую строку
        isTyping = false;
    }
    else
    {
        // Если текст завершен, переходим к следующей строке
        currentLineIndex++;
        if (currentLineIndex < dialogueLines.Length)
        {
            StartCoroutine(TypeText(dialogueLines[currentLineIndex])); // Печатаем следующий текст
        }
        else
        {
            // Если текстов больше нет, скрываем панель и кнопку
            dialoguePanel.SetActive(false);
            skipButton.SetActive(false);
        }
    }
}

}
