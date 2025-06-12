using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RespawnManager : MonoBehaviour
{
    public Transform respawnPointTransform;
    public float respawnDelay = 2f;
    public Image fadeImage;
    public Enemy enemy;

    private void Start()
    {
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 0);

        if (enemy == null)
        {
            enemy = FindObjectOfType<Enemy>();
            if (enemy == null)
            {
                Debug.LogError("Враг (Enemy) не найден на сцене!");
            }
        }
    }


    public void RespawnPlayer(PlayerHealth player)
    {
        StartCoroutine(RespawnCoroutine(player));
    }

    private IEnumerator RespawnCoroutine(PlayerHealth player)
    {
        fadeImage.gameObject.SetActive(true); // Включаем объект перед затемнением

        yield return StartCoroutine(Fade(0, 1, 1f));

        yield return new WaitForSeconds(respawnDelay);

        if (enemy != null)
        {
            enemy.Revive();
        }

        Vector3 point = respawnPointTransform != null ? respawnPointTransform.position : Vector3.zero;
        player.Revive(point);

        yield return StartCoroutine(Fade(1, 0, 1f));

        fadeImage.gameObject.SetActive(false); // Выключаем, если нужно скрыть
    }


    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, to);
    }
}

