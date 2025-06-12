using UnityEngine;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
    public AutoDialog autoDialog; // Ссылка на AutoDialog

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null && autoDialog != null)
        {
            button.onClick.AddListener(autoDialog.SkipText);
        }
        else
        {
            Debug.LogError("Button или AutoDialog не настроены!");
        }
    }
}
