using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public float attackDuration = 0.5f;  
    public float attackCooldown = 1f;    
    private float nextAttackTime = 0f;   

    private BoxCollider2D swordCollider;  
    private bool isAttacking = false;

    // ����� ���������� ��� ������������ ������ � ���� �����
    private List<Enemy> enemiesInRange = new List<Enemy>();

    private void Start()
    {
        swordCollider = GetComponent<BoxCollider2D>();
        swordCollider.enabled = false;  
    }

    private void Update()
    {
        if (Time.time >= nextAttackTime && Input.GetMouseButtonDown(0))  
        {
            Attack();
        }
    }

    void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            swordCollider.enabled = true;  

            Invoke("EndAttack", attackDuration);

            nextAttackTime = Time.time + attackCooldown;

            // ������� ���� ������ � �������� ���� �����
            foreach (var enemy in enemiesInRange)
            {
                enemy.TakeDamage(10);  // �������������� ������������� �������� �����
                UnityEngine.Debug.Log("���� ������� 10 �����.");
            }
        }
    }

    void EndAttack()
    {
        swordCollider.enabled = false;  
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemiesInRange.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }
}
