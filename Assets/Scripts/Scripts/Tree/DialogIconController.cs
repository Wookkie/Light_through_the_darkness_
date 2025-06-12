using UnityEngine;

public class DialogIconController : MonoBehaviour
{
    public GameObject dialogueIcon;
    public Canvas canvas;

    private Transform playerTransform;
    private Transform treeTransform;

    public GameObject swordPrefab; // Префаб меча
    private Transform swordSpawnPoint; // Точка спавна меча

    void Start()
    {
        dialogueIcon.SetActive(false);

        // Ищем точку спавна меча
        GameObject spawnPointObject = GameObject.Find("SwordSpawnPoint");
        if (spawnPointObject != null)
        {
            swordSpawnPoint = spawnPointObject.transform;
            Debug.Log("SwordSpawnPoint найден: " + swordSpawnPoint.position);
        }
        else
        {
            Debug.LogError("Ошибка: SwordSpawnPoint не найден в сцене!");
        }
    }

    public void StartDialog(Transform talkingObject)
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        treeTransform = talkingObject;

        Vector3 targetPosition = talkingObject.position + new Vector3(-0.5f, 1f, 0);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPosition);

        dialogueIcon.transform.position = screenPos;
        dialogueIcon.SetActive(true);
    }

    public void EndDialog()
    {
        Debug.Log("EndDialog() вызван!"); // Проверяем, вызывается ли метод
        dialogueIcon.SetActive(false);

        // Проверяем, что префаб и точка спавна заданы
        if (swordSpawnPoint != null && swordPrefab != null)
        {
            GameObject spawnedSword = Instantiate(swordPrefab, swordSpawnPoint.position, Quaternion.identity);
            Debug.Log("Меч успешно заспавнен на позиции: " + swordSpawnPoint.position);
        }
        else
        {
            if (swordSpawnPoint == null)
                Debug.LogError("Ошибка: SwordSpawnPoint не найден!");

            if (swordPrefab == null)
                Debug.LogError("Ошибка: Префаб меча не назначен!");
        }
    }

    public void SpawnSword()
    {
        GameObject swordSpawnPoint = GameObject.Find("SwordSpawnPoint");
        if (swordSpawnPoint != null)
        {
            GameObject sword = GameObject.Find("Sword");
            if (sword != null)
            {
                sword.transform.position = swordSpawnPoint.transform.position;
                sword.SetActive(true);
                Debug.Log("Sword spawned at position: " + sword.transform.position);
                Debug.Log("Sword active: " + sword.activeSelf); // Проверка активности
            }
            else
            {
                Debug.LogError("Sword object not found in the scene! Check its presence and name.");
            }
        }
        else
        {
            Debug.LogError("SwordSpawnPoint not found! Make sure it exists in the scene.");
        }
    }



}