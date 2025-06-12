using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [TextArea] public string[] dialogueLines; // Массив строк с диалогом
    [TextArea] public string[] option1Text;  // Массив строк для первой кнопки
    [TextArea] public string[] option2Text;  // Массив строк для второй кнопки
}
