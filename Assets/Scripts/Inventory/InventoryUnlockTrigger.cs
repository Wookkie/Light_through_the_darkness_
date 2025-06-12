using UnityEngine;
using System.Collections;

public class InventoryUnlockTrigger : MonoBehaviour
{
    public GameObject messageUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("🔲 Trigger entered by: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Player entered trigger zone.");

            InventoryManager.instance.canOpenInventory = true;
            Debug.Log("📦 canOpenInventory set to true!");

            if (messageUI != null)
            {
                Debug.Log("🪧 Показываем messageUI");
                messageUI.SetActive(true);
                StartCoroutine(HideAfterDelay());
            }
            else
            {
                Debug.LogWarning("⚠️ messageUI не назначен!");
            }
        }
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        if (messageUI != null)
            messageUI.SetActive(false);

        Destroy(gameObject);
    }
}
