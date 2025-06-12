using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager2 : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public GameObject skipButton;

    public string[] dialogueLines;
    public float typingSpeed = 0.05f;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool hasTriggered = false; // Чтобы диалог запускался один раз

    void Start()
    {
        dialoguePanel.SetActive(false); // Изначально диалог скрыт
        skipButton.SetActive(false);
    }

    public void StartDialogue()
    {
        if (hasTriggered) return; // Не запускать повторно
        hasTriggered = true;

        dialoguePanel.SetActive(true);
        skipButton.SetActive(true);
        currentLineIndex = 0;
        StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    public void SkipText()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = dialogueLines[currentLineIndex];
            isTyping = false;
        }
        else
        {
            currentLineIndex++;
            if (currentLineIndex < dialogueLines.Length)
            {
                StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
            }
            else
            {
                dialoguePanel.SetActive(false);
                skipButton.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        StartDialogue();
    }
}

}
