using System.Collections;
using UnityEngine;

public class TriggerFadeText : MonoBehaviour
{
    public CanvasGroup textPanel; // Ссылка на UI-плашку
    public float fadeDuration = 1f; // Время плавного появления

    private void Start()
    {
        if (textPanel != null)
            textPanel.alpha = 0; // Начально невидимая плашка
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Проверяем, что вошел игрок
        {
            StopAllCoroutines();
            StartCoroutine(FadeCanvasGroup(textPanel, 0, 1));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Когда игрок уходит
        {
            StopAllCoroutines();
            StartCoroutine(FadeCanvasGroup(textPanel, 1, 0));
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha)
    {
        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }
}
