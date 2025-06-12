using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TriggerDialogueManager1 : MonoBehaviour
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
            option1Button.GetComponentInChildren<TextMeshProUGUI>().text = "Серьезный";
        }
        if (option2Button != null)
        {
            option2Button.gameObject.SetActive(true);
            option2Button.GetComponentInChildren<TextMeshProUGUI>().text = "Ироничный";
        }
    }

    public void OnChooseListen()
    {
        Debug.Log("Выбрано: Серьезный");
        choseToListen = true;

        if (option1Button != null)
            option1Button.gameObject.SetActive(false);
        if (option2Button != null)
            option2Button.gameObject.SetActive(false);

        dialogueLines = new string[]
        {
            "Эльмар «Держись, сейчас вытащу. Но в следующий раз смотри, куда идёшь.»",
            "Шут (утирая грязь): Благодарю тебя, о великодушный герой без чувства юмора! Обещаю — отныне не шагу без палки и совести.",
            "Лягушка в очках: Он серьёзен, о-о… берегись, шут, тебе теперь придётся стать… ответственным.",
            "Шут: Слушай, ты — нормальный парень. Я чувствую — ты в пути.",
            "А я обожаю путь, особенно если кто-то идёт первым. Пойду с тобой. Буду шутить, мешать, спасать... когда получится. И не тонуть! Ну, почти не тонуть.",
            "Эльмар: Просто не попадайся мне под ноги."
        };
        currentLineIndex = 0;
        ContinueDialogue();
    }
    public void OnChooseIgnore()
    {
        Debug.Log("Выбрано: Иронично");
        choseToListen = false;

        if (option1Button != null)
            option1Button.gameObject.SetActive(false);
        if (option2Button != null)
            option2Button.gameObject.SetActive(false);

        dialogueLines = new string[]
        {
            "Эльмар: «Ты часто репетируешь смерть в трясине или это специальное выступление для лягушек?»",
            "Шут: Ты оценил драматизм! Знал, что найдётся зритель! Ты не просто спас меня — ты дал шанс продолжить турне!",
            "Зеленая лягушка: О, теперь у него появился друг. Мы это запишем как спектакль «Двое в болоте».",
            "Шут: Слушай, ты — нормальный парень. Я чувствую — ты в пути.",
            "А я обожаю путь, особенно если кто-то идёт первым. Пойду с тобой. Буду шутить, мешать, спасать... когда получится. И не тонуть! Ну, почти не тонуть.",
            "Эльмар: Просто не попадайся мне под ноги."
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