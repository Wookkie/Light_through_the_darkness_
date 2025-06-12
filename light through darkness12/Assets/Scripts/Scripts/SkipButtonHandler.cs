using UnityEngine;

public class SkipButtonHandler : MonoBehaviour
{
    public AutoDialog autoDialog;

    public void OnSkipButtonClick()
    {
        if (autoDialog != null)
        {
            autoDialog.SkipText();
        }
        else
        {
            Debug.LogError("AutoDialog не назначен!");
        }
    }
}
