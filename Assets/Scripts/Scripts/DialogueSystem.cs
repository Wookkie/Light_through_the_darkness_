using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public Text dialogueText; // Ссылка на текстовое поле
    public string[] dialogueLines; // Массив строк для диалога
    public float textSpeed = 0.05f; // Скорость появления текста

    private int currentLine = 0; // Индекс текущей строки
    private bool isTyping = false; // Проверка, идёт ли печать текста
    private bool canProceed = false; // Проверка, можно ли перейти к следующей строке

    void Start()
    {
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canProceed)
        {
            if (!isTyping)
            {
                NextLine();
            }
            else
            {
                // Если текст ещё печатается, завершить его мгновенно
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLine];
                isTyping = false;
                canProceed = true;
            }
        }
    }

    public void StartDialogue()
    {
        currentLine = 0;
        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (currentLine < dialogueLines.Length - 1)
        {
            currentLine++;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueText.text = ""; // Очистить текст в конце диалога
            canProceed = false; // Диалог окончен
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        canProceed = false;
        dialogueText.text = "";

        foreach (char c in dialogueLines[currentLine])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
        canProceed = true;
    }
}