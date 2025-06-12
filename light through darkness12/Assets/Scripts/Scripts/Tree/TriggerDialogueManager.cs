using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TriggerDialogueManager : MonoBehaviour
{
    [Header("UI Элементы")]
    public GameObject dialoguePanel; // Панель с диалогом
    public TextMeshProUGUI dialogueText; // TextMeshPro для отображения текста
    public GameObject skipButton; // Кнопка для пропуска текста
    public Button option1Button; // Кнопка для выбора первого варианта (выслушать)
    public Button option2Button; // Кнопка для выбора второго варианта (проигнорировать)

    [Header("Диалог")]
    [TextArea]
    public string[] dialogueLines; // Массив строк с диалогом
    public string[] option1Text; // Тексты для кнопки 1 (выслушать)
    public string[] option2Text; // Тексты для кнопки 2 (проигнорировать)
    public float typingSpeed = 0.05f; // Скорость печати текста

    private int currentLineIndex = 0; // Индекс текущей строки
    private bool isTyping = false; // Флаг, идет ли печать текста
    private bool isPlayerInTrigger = false; // Флаг, находится ли игрок в триггере
    private GameObject player; // Игрок, чтобы отключить его движение

    private PlayerController playerController; // Ссылка на контроллер игрока

    private bool choseToListen = false; // Флаг для выбора игрока
    private bool hasDialogueStarted = false; // Флаг для того, чтобы диалог не начинался повторно

    private bool isWeaponChoiceMade = false; // Флаг для выбора оружия
    private bool choseSword = false; // Флаг для выбора меча
    private bool choseBow = false; // Флаг для выбора лука

    private bool hasSpokenTruth = false; // Флаг, чтобы фраза Эльмара не повторялась

    void Start()
    {
        // Инициализация UI элементов
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false); // Скрываем панель

        if (skipButton != null)
            skipButton.SetActive(false); // Скрываем кнопку

        if (option1Button != null)
            option1Button.gameObject.SetActive(false); // Скрываем кнопку выбора

        if (option2Button != null)
            option2Button.gameObject.SetActive(false); // Скрываем кнопку выбора

        // Привязываем методы к кнопкам
        if (option1Button != null)
            option1Button.onClick.AddListener(OnChooseListen);
        if (option2Button != null)
            option2Button.onClick.AddListener(OnChooseIgnore);

        if (skipButton != null)
            skipButton.GetComponent<Button>().onClick.AddListener(SkipText); // Подключаем кнопку скипа
    }

    public void StartDialogue()
    {
        if (hasDialogueStarted) return; // Проверка, чтобы диалог не начинался повторно

        Debug.Log("Диалог начат.");
        hasDialogueStarted = true; // Устанавливаем флаг, что диалог начался
        currentLineIndex = 0;

        // Показываем панель и кнопку пропуска
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true); // Показываем панель
        }

        if (skipButton != null)
        {
            skipButton.SetActive(true); // Показываем кнопку
        }

        if (dialogueLines.Length > 0)
        {
            StartCoroutine(TypeText(dialogueLines[currentLineIndex])); // Начинаем печатать текст
        }
        else
        {
            Debug.LogError("Массив dialogueLines пуст!");
        }

        // Отключаем управление игроком
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>(); // Получаем компонент PlayerController
            if (playerController != null)
            {
                playerController.enabled = false; // Отключаем движение
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.velocity = Vector2.zero; // Обнуляем скорость
            }
        }
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        if (dialogueText != null)
            dialogueText.text = ""; // Очищаем текст

        foreach (char letter in line.ToCharArray())
        {
            if (dialogueText != null)
                dialogueText.text += letter; // Добавляем буквы по одной
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        // После определённых строк показываем кнопки выбора
        if (currentLineIndex == 1 && !choseToListen) // После фразы "Слушай, путник, ты не продержишься без защиты..."
        {
            ShowChoices(); // Показываем кнопки для выбора
        }
        else if (currentLineIndex == 4 && choseToListen && !hasSpokenTruth) // После фразы о даре дерева и оружии
        {
            hasSpokenTruth = true; // Обновляем флаг, что фраза уже была показана
            ShowWeaponChoices(); // Показываем кнопки выбора оружия
        }
        else if (currentLineIndex == 2 && !hasSpokenTruth) // После фразы "Эльмар (думает): Это дерево... живое?"
        {
            ContinueDialogue(); // Продолжаем диалог, не завершаем его
        }
        else if (currentLineIndex == 3 && !hasSpokenTruth) // После фразы "Эльмар (думает): Так, значит, это правда..."
        {
            ContinueDialogue(); // Продолжаем диалог после этой фразы
        }
        else if (currentLineIndex >= dialogueLines.Length)
        {
            EndDialogue(); // Если диалог завершён
        }

        currentLineIndex++; // Переходим к следующей строке
    }

    void ShowChoices()
    {
        Debug.Log("Показываем кнопки для выбора.");

        // Убедитесь, что кнопки не были скрыты до этого
        if (option1Button != null)
            option1Button.gameObject.SetActive(true); // Показываем кнопку "Выслушать"
        if (option2Button != null)
            option2Button.gameObject.SetActive(true); // Показываем кнопку "Нет времени"

        // Обновляем текст кнопок в зависимости от текущего индекса
        option1Button.GetComponentInChildren<TextMeshProUGUI>().text = "Выслушать";
        option2Button.GetComponentInChildren<TextMeshProUGUI>().text = "Нет времени";
    }

    void ShowWeaponChoices()
    {
        Debug.Log("Показываем кнопки для выбора оружия."); // Логируем, что метод вызван

        // Показываем кнопки выбора оружия
        if (option1Button != null)
            option1Button.gameObject.SetActive(true); // Показываем кнопку "Меч"
        if (option2Button != null)
            option2Button.gameObject.SetActive(true); // Показываем кнопку "Лук"

        // Обновляем текст кнопок выбора оружия
        option1Button.GetComponentInChildren<TextMeshProUGUI>().text = "Меч";
        option2Button.GetComponentInChildren<TextMeshProUGUI>().text = "Лук";
    }

    public void OnChooseListen()
    {
        Debug.Log("Выбрано: Выслушать");
        choseToListen = true;

        // Убираем кнопки после выбора
        if (option1Button != null)
            option1Button.gameObject.SetActive(false); // Скрываем кнопку "Выслушать"
        if (option2Button != null)
            option2Button.gameObject.SetActive(false); // Скрываем кнопку "Нет времени"

        // Обновляем диалог
        dialogueLines = new string[] 
        {
            "Эльмар (сухо):«Вглубь.»",
            "Дерево (с легким вздохом): «Гм... Вглубь, значит. Значит, тебе есть что терять. Никто не идет в Ноктум без причины. Здесь давно погиб свет. Земля полна гнилью, воспоминаниями о войне, которая изменила всё. Проклятие не отпускает даже корни.»",
            "Эльмар (думает): «Так, значит, это правда. Здесь велись бои. Всё, что осталось от прошлого... всё, что я слышал в детстве, было правдой.»",
            "Дерево: «Слушай, путник, ты можешь идти дальше, но без защиты ты не продержишься долго. Старинные воины оставили мне дар, спрятанный в моих корнях. Это оружие для тех, кто отважен или безумен.»"
        };
        currentLineIndex = 0; // Перезапускаем диалог с новой последовательностью
        ContinueDialogue();
    }

    public void OnChooseIgnore()
    {
        Debug.Log("Выбрано: Проигнорировать");
        choseToListen = false;

        // Убираем кнопки после выбора
        if (option1Button != null)
            option1Button.gameObject.SetActive(false); // Скрываем кнопку "Выслушать"
        if (option2Button != null)
            option2Button.gameObject.SetActive(false); // Скрываем кнопку "Нет времени"

        // Обновляем диалог
        dialogueLines = new string[] 
        {
            "Эльмар (думает): «Нет времени слушать деревья. Я не могу терять ни минуты.»",
            "Дерево (тихо вздыхает, затем говорит): «Если ты так торопишься... возьми этот меч. Он станет тебе опорой. Проклятие на этих землях не отпустит тебя так просто.»"
        };
        currentLineIndex = 0; // Перезапускаем диалог с новой последовательностью
        ContinueDialogue();
    }

    public void OnChooseSword()
    {
        choseSword = true;
        if (option1Button != null)
            option1Button.gameObject.SetActive(false); // Скрываем кнопку "Меч"
        if (option2Button != null)
            option2Button.gameObject.SetActive(false); // Скрываем кнопку "Лук"

        // Обновляем диалог для меча
        dialogueLines = new string[]
        {
            "Дерево: «Меч, говоришь? Простой выбор, но надежный. Он требует храбрости и стойкости. Научись им орудовать как следует. Твоя жизнь теперь зависит от твоей руки.»",
            "Эльмар (думает): «Не знаю, зачем дерево заботится обо мне, но его совет... дельный.»",
            "Дерево: «Иди. Но запомни: сила оружия - ничто без разума. Битвы тебя только закалят, но каждое поражение - это шрам на твоей душе.»"
        };
        currentLineIndex = 0;
        ContinueDialogue();
    }

    public void OnChooseBow()
    {
        choseBow = true;
        if (option1Button != null)
            option1Button.gameObject.SetActive(false); // Скрываем кнопку "Меч"
        if (option2Button != null)
            option2Button.gameObject.SetActive(false); // Скрываем кнопку "Лук"

        // Обновляем диалог для лука
        dialogueLines = new string[]
        {
            "Дерево: «Лук, говоришь? Умный выбор. Не подходи слишком близко к врагам, но помни: каждая стрела бесценна. Пусть твой выстрел будет точен.»",
            "Эльмар (думает): «Не знаю, зачем дерево заботится обо мне, но его совет... дельный.»",
            "Дерево: «Иди. Но запомни: сила оружия - ничто без разума. Битвы тебя только закалят, но каждое поражение - это шрам на твоей душе.»"
        };
        currentLineIndex = 0;
        ContinueDialogue();
    }

    public void SkipText()
    {
        if (isTyping)
        {
            // Останавливаем текущую печать текста
            StopAllCoroutines();
            // Показываем оставшийся текст сразу
            if (dialogueText != null && currentLineIndex < dialogueLines.Length)
            {
                dialogueText.text = dialogueLines[currentLineIndex]; // Показываем текущую строку
            }
            isTyping = false; // Останавливаем печать
        }
        else
        {
            ContinueDialogue(); // Переходим к следующему фрагменту текста
        }
    }

    public void ContinueDialogue()
    {
        // Если диалог завершен
        if (currentLineIndex >= dialogueLines.Length)
        {
            EndDialogue();
        }
        else
        {
            StartCoroutine(TypeText(dialogueLines[currentLineIndex])); // Печатаем следующую строку
        }
    }

    public void EndDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false); // Скрываем панель

        // Включаем управление игроком после окончания диалога
        if (playerController != null)
            playerController.enabled = true;

        // Обнуляем текущие индексы и флаги
        currentLineIndex = 0;
        choseToListen = false;
        hasDialogueStarted = false;
        hasSpokenTruth = false;

        if (skipButton != null)
            skipButton.SetActive(false); // Скрываем кнопку пропуска
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