using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int damage = 25; // Урон от меча
    public bool isEquipped = false; // Проверка, экипирован ли меч
    private Transform playerTransform; // Ссылка на трансформ игрока
    private Collider2D swordCollider; // Коллайдер меча
    private Rigidbody2D swordRb; // Rigidbody меча

    public float attackDuration = 0.2f; // Продолжительность атаки
    private bool isAttacking = false; // Флаг, указывающий, что атака идет
    public Vector3 swordOffset = new Vector3(0.5f, 0.5f, 0); // Положение меча относительно игрока

    private bool isRotating = false; // Флаг для проверки, происходит ли вращение меча
    private bool isSwordTaken = false; // Флаг, указывающий, был ли меч взят
    private bool isPlayerMoving = false; // Флаг для проверки, движется ли игрок

    void Start()
    {
        swordCollider = GetComponent<Collider2D>();
        swordCollider.enabled = true;
        swordRb = GetComponent<Rigidbody2D>();

        swordRb.isKinematic = true;
        swordRb.gravityScale = 0;

        transform.localRotation = Quaternion.identity; // Устанавливаем начальное положение меча
    }

    public void EquipSword(Transform player)
    {
        if (isSwordTaken) return; // Если меч уже взят, ничего не делаем

        playerTransform = player;
        transform.SetParent(playerTransform); // Меч следует за игроком

        // Устанавливаем меч на фиксированную позицию относительно персонажа
        Transform attackPos = playerTransform.Find("AttackPos");
        if (attackPos != null)
        {
            transform.position = attackPos.position;
            transform.position += new Vector3(0, 0, -0.1f); // "Поверх" игрока
            float direction = playerTransform.localScale.x > 0 ? 1f : -1f;
            transform.rotation = Quaternion.Euler(0, direction > 0 ? 0 : 180, 45);
            transform.localScale = new Vector3(direction, 1, 1);
        }
        else
        {
            UnityEngine.Debug.LogWarning("AttackPos not found. Sword position might be incorrect.");
        }

        isEquipped = true;
        swordCollider.enabled = true;
        swordRb.isKinematic = true;
        swordRb.gravityScale = 0;

        SpriteRenderer swordRenderer = GetComponent<SpriteRenderer>();
        if (swordRenderer != null)
        {
            swordRenderer.sortingLayerName = "Foreground";
            swordRenderer.sortingOrder = 1;
        }

        UnityEngine.Debug.Log("Sword equipped and positioned correctly.");
    }

    public void DropSword()
    {
        transform.SetParent(null); // Убираем меч из родительского объекта
        isEquipped = false;
        swordCollider.enabled = false;

        swordRb.isKinematic = false;
        swordRb.gravityScale = 1;

        UnityEngine.Debug.Log("Sword dropped.");
    }

    public void Attack()
    {
        if (!isEquipped)
        {
            UnityEngine.Debug.Log("Sword is not equipped. Attack cannot be performed.");
            return;
        }

        if (!isAttacking && !isRotating)
        {
            isAttacking = true;
            swordCollider.enabled = true;

            StartCoroutine(DisableColliderAfterDelay(0.1f));

            // Определяем, движется ли игрок
            isPlayerMoving = Mathf.Abs(playerTransform.GetComponent<Rigidbody2D>().velocity.x) > 0;

            // Угол атаки зависит от направления игрока
            float attackAngle = playerTransform.localScale.x > 0 ? -90f : 90f;

            // Длительность атаки зависит от движения игрока
            float duration = isPlayerMoving ? 0.1f : attackDuration;

            StartCoroutine(RotateSword(attackAngle, duration));
            StartCoroutine(EndAttack()); // Запускаем корутину для завершения атаки
        }
    }

    private IEnumerator RotateSword(float targetAngle, float duration)
    {
        isRotating = true; // Устанавливаем флаг, что вращение в процессе

        float currentAngle = transform.eulerAngles.z;
        float targetRotation = currentAngle + targetAngle;

        float time = 0;
        while (time < duration)
        {
            float angle = Mathf.Lerp(currentAngle, targetRotation, time / duration);
            transform.rotation = Quaternion.Euler(0, 0, angle);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, targetRotation);
        isRotating = false;
    }

    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(attackDuration); // Подождем, пока атака закончится

        // Скрыть меч после атаки
        HideSword();

        yield return new WaitForSeconds(1.0f); // Задержка, прежде чем вернуть меч

        // Показать меч снова
        ShowSword();
    }

    private IEnumerator DisableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Используем damage здесь
            UnityEngine.Debug.Log("Enemy hit for " + damage + " damage.");
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isEquipped)
            {
                DropSword();
                HideSword();
            }
            else
            {
                ShowSword();
            }
        }
    }

    public void HideSword()
    {
        gameObject.SetActive(false);
        UnityEngine.Debug.Log("Sword hidden.");
    }

    public void ShowSword()
    {
        if (isSwordTaken) return;

        gameObject.SetActive(true);
        transform.SetParent(playerTransform);

        transform.localPosition = swordOffset;
        transform.localRotation = Quaternion.identity;

        swordRb.isKinematic = true;
        swordRb.gravityScale = 0;

        UnityEngine.Debug.Log("Sword shown.");
    }

    public void TakeSword()
    {
        isSwordTaken = true;
        DropSword();
    }
}
