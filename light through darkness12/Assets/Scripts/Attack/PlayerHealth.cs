using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;           // ������������ �������� ������
    private int currentHealth;            // ������� �������� ������

    public bool isDead = false;           // ����, ��� ����� �����

    private void Start()
    {
        currentHealth = maxHealth;       // ������������� ������������ ��������
    }

    // ����� ��� ��������� �����
    public void TakeDamage(int damage)
    {
        if (isDead) return;               // ���� ����� �����, �� �������� ����

        currentHealth -= damage;          // ��������� �������� �� �������� �����

        if (currentHealth <= 0)
        {
            Die();                        // ���� �������� ����� 0 ��� ������, ����� �������
        }
    }

    // ��������� ����� ��� ������ ������
    public void Die()
    {
        isDead = true;                    // ������������� ���� ������
        UnityEngine.Debug.Log("Player has died!");    // ������� ��������� � �������
        // ����� �������� �������� ������ ��� ������ �������� (��������, ������������ ������)

        // ������ ���������� ������ ������
        GetComponent<Rigidbody2D>().simulated = false;  // ���� � ��� ���� Rigidbody2D, ��� ����� ���������
        // ��� ����� ���������� ������ ������
        // Destroy(gameObject);
    }
}
