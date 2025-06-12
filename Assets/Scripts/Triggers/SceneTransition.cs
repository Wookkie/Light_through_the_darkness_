using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public CanvasGroup fadePanel;
    public float fadeDuration = 0.5f;
    public string sceneName;

    private static SceneTransition instance;

    private void Awake()
    {
        Debug.Log("SceneTransition Awake вызван!");

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Убеждаемся, что объект не уничтожится при переходе между сценами
            Debug.Log("Объект SceneTransition не будет уничтожен при загрузке новой сцены.");

            // Создаем fadePanel, если он не задан
            if (fadePanel == null)
            {
                CreateFadePanel();
            }
        }
        else
        {
            Debug.Log("Обнаружен дубликат SceneTransition. Уничтожаем...");
            Destroy(gameObject);
            return;
        }

        if (fadePanel != null)
        {
            fadePanel.alpha = 1; // Начинаем с темного экрана (полностью непрозрачного)
            fadePanel.blocksRaycasts = true;
        }
    }

    private void CreateFadePanel()
    {
        // Создаем новый Canvas для fadePanel
        GameObject canvasObject = new GameObject("FadeCanvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        // Создаем CanvasGroup для fadePanel
        GameObject panelObject = new GameObject("FadePanel");
        panelObject.transform.SetParent(canvasObject.transform);
        fadePanel = panelObject.AddComponent<CanvasGroup>();

        // Настройка fadePanel
        RectTransform rectTransform = panelObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        // Настройка цвета (например, черный)
        Image image = panelObject.AddComponent<Image>();
        image.color = Color.black;

        fadePanel.alpha = 1; // Начинаем с темного экрана
        fadePanel.blocksRaycasts = true;
    }

    private void Start()
    {
        // В старте, если панель существует, мы начнем с затемненного экрана (если требуется)
        if (fadePanel != null)
        {
            fadePanel.alpha = 1; // Начнем с черного экрана
            StartCoroutine(FadeCanvasGroup(fadePanel, 1, 0, () =>
            {
                fadePanel.blocksRaycasts = false; // Отключаем блокировку событий после завершения анимации
            }));
        }
    }

    public void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Ошибка: имя сцены не задано!");
            return;
        }
        StartCoroutine(TransitionScene(sceneName));
    }

    public void LoadScene(string sceneToLoad)
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("Ошибка: имя сцены не задано!");
            return;
        }
        sceneName = sceneToLoad;
        LoadScene();
    }

    private IEnumerator TransitionScene(string sceneToLoad)
    {
        Debug.Log($"Начинается переход сцены: {sceneToLoad}");

        if (fadePanel == null)
        {
            Debug.LogError("Ошибка: fadePanel не установлен!");
            yield break;
        }

        fadePanel.blocksRaycasts = true;

        // Сначала затемняем экран перед загрузкой сцены (чтобы создать эффект затмения)
        yield return FadeCanvasGroup(fadePanel, 0, 1); // Экран становится черным (0 -> 1)

        // Загружаем новую сцену асинхронно
        if (Application.CanStreamedLevelBeLoaded(sceneToLoad))
        {
            Debug.Log($"Сцена {sceneToLoad} загружается...");
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
        else
        {
            Debug.LogError($"Ошибка: сцена '{sceneToLoad}' не найдена! Проверьте Build Settings.");
            yield break;
        }

        yield return new WaitForSeconds(0.1f); // Немного подождем, чтобы сцена успела загрузиться

        // После загрузки сцены экран плавно проясняется
        StartCoroutine(FadeCanvasGroup(fadePanel, 1, 0, () =>
        {
            fadePanel.blocksRaycasts = false; // Отключаем блокировку событий после завершения анимации
        }));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, System.Action onComplete = null)
    {
        if (canvasGroup == null)
        {
            Debug.LogError("Ошибка: canvasGroup не задан!");
            yield break;
        }

        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        onComplete?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Объект {other.name} вошел в триггер!");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Игрок вошел в триггер, начинается загрузка сцены...");
            LoadScene();
        }
        else
        {
            Debug.Log("Объект не является игроком, сцена не загружается.");
        }
    }
}
