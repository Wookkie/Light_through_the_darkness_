using System.Diagnostics;
using UnityEngine;

public class TestAttackAnimation : MonoBehaviour
{
    private Animator m_anim;

    void Start()
    {
        m_anim = GetComponentInChildren<Animator>();
        if (m_anim == null)
        {
            UnityEngine.Debug.LogError("Animator not found!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 Ч это код левой кнопки мыши.
        {
            UnityEngine.Debug.Log("Setting Attack trigger");
            m_anim.SetTrigger("Attack");
        }
    }
}
