using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogData", menuName = "Dialog/DialogData")]
public class DialogData : ScriptableObject
{
    [TextArea]
    public string[] dialogueLines; // Массив строк для диалога
}
