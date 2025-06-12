using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TriggerDialogueManager : MonoBehaviour
{
    [Header("UI Элементы")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public GameObject skipButton;
    public Button option1Button;
    public Button option2Button;

    [Header("Диалог")]
    [TextArea]
    public string[] dialogueLines;
    public string[] option1Text;
    public string[] option2Text;
    public float typingSpeed = 0.05f;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool isPlayerInTrigger = false;
    private GameObject player;

    private PlayerController playerController;

    private bool choseToListen = false;
    private bool hasDialogueStarted = false;

    private bool hasSpokenTruth = false;

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (skipButton != null)
            skipButton.SetActive(false);

        if (option1Button != null)
            option1Button.gameObject.SetActive(false);

        if (option2Button != null)
            option2Button.gameObject.SetActive(false);

        if (option1Button != null)
            option1Button.onClick.AddListener(OnChooseListen);
        if (option2Button != null)
            option2Button.onClick.AddListener(OnChooseIgnore);

        if (skipButton != null)
            skipButton.GetComponent<Button>().onClick.AddListener(SkipText);
    }

    public void StartDialogue()
    {
        if (hasDialogueStarted) return;

        Debug.Log("Диалог начат.");
        hasDialogueStarted = true;
        currentLineIndex = 0;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (skipButton != null)
            skipButton.SetActive(true);

        if (dialogueLines.Length > 0)
            StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
        else
            Debug.LogError("Массив dialogueLines пуст!");

        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false;
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.velocity = Vector2.zero;
            }
        }
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;

        if (dialogueText != null)
            dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            if (dialogueText != null)
                dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        if (currentLineIndex == 1 && !choseToListen)
            ShowChoices();
    }

    void ShowChoices()
    {
        Debug.Log("Показываем кнопки для выбора.");

        if (option1Button != null)
        {
            option1Button.gameObject.SetActive(true);
            option1Button.GetComponentInChildren<TextMeshProUGUI>().text = "Выслушать";
        }
        if (option2Button != null)
        {
            option2Button.gameObject.SetActive(true);
            option2Button.GetComponentInChildren<TextMeshProUGUI>().text = "Нет времени";
        }
    }

    public void OnChooseListen()
    {
        Debug.Log("Выбрано: Выслушать");
        choseToListen = true;

        if (option1Button != null)
            option1Button.gameObject.SetActive(false);
        if (option2Button != null)
            option2Button.gameObject.SetActive(false);

        dialogueLines = new string[]
        {
            "Эльмар (сухо):«Вглубь.»",
            "Дерево (с легким вздохом): «Гм... Вглубь, значит. Значит, тебе есть что терять. Никто не идет в Ноктум без причины. Здесь давно погиб свет. Земля полна гнилью, воспоминаниями о войне, которая изменила всё. Проклятие не отпускает даже корни.»",
            "Эльмар (думает): Так, значит, это правда. Здесь велись бои. Всё, что осталось от прошлого... всё, что я слышал в детстве, было правдой.",
            "Дерево: «Слушай, путник, ты можешь идти дальше, но без защиты ты не продержишься долго. Старинные воины оставили мне дар, спрятанный в моих корнях. Это оружие для тех, кто отважен или безумен.»"
        };
        currentLineIndex = 0;
        ContinueDialogue();
    }
    public void OnChooseIgnore()
    {
        Debug.Log("Выбрано: Проигнорировать");
        choseToListen = false;

        if (option1Button != null)
            option1Button.gameObject.SetActive(false);
        if (option2Button != null)
            option2Button.gameObject.SetActive(false);

        dialogueLines = new string[]
        {
            "Эльмар (думает): «Нет времени слушать деревья. Я не могу терять ни минуты.»",
            "Дерево (тихо вздыхает, затем говорит): «Если ты так торопишься... возьми этот меч. Он станет тебе опорой. Проклятие на этих землях не отпустит тебя так просто.»"
        };
        currentLineIndex = 0;
        ContinueDialogue();
    }

    public void SkipText()
    {
        if (isTyping)
        {
            StopAllCoroutines();

            if (dialogueText != null && currentLineIndex < dialogueLines.Length)
                dialogueText.text = dialogueLines[currentLineIndex];
            isTyping = false;
        }
        else
        {
            currentLineIndex++;

            if (currentLineIndex < dialogueLines.Length)
                StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
            else
                EndDialogue();
        }
    }

    public void ContinueDialogue()
    {
        if (currentLineIndex >= dialogueLines.Length)
            EndDialogue();
        else
            StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
    }

    public void EndDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (playerController != null)
            playerController.enabled = true;

        currentLineIndex = 0;
        choseToListen = false;
        hasDialogueStarted = false;

        if (skipButton != null)
            skipButton.SetActive(false);

        // Ищем объект меча и появляем его
        DialogIconController dialogIconController = FindObjectOfType<DialogIconController>();
        if (dialogIconController != null)
        {
            dialogIconController.SpawnSword();
            Debug.Log("Меч появился после диалога!");
        }
        else
        {
            Debug.LogError("DialogIconController не найден! Убедись, что он есть в сцене.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Игрок вошел в триггер.");
            isPlayerInTrigger = true;
            player = collision.gameObject;
            StartDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Игрок вышел из триггера.");
            if (!isTyping && currentLineIndex >= dialogueLines.Length)
            {
                isPlayerInTrigger = false;
                EndDialogue();
            }
        }
    }
}