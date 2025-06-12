using UnityEngine;

public class TestAttackAnimation : MonoBehaviour
{
    private Animator m_anim;
    private Sword m_sword;
    public PlayerController m_controller;


    void Start()
    {
        m_anim = GetComponentInChildren<Animator>();

        if (m_anim == null)
        {
            Debug.LogError("Animator not found!");
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            m_controller = playerObj.GetComponent<PlayerController>();
            if (m_controller == null)
                Debug.LogError("PlayerController not found on Player tagged object!");
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found!");
        }
    }

    void Update()
    {
        //if (m_controller != null)
        //{
        //    UnityEngine.Debug.Log("Linked controller canAttack: " + m_controller.canAttack);
        //}
        //else
        //{
        //    UnityEngine.Debug.LogError("Controller is not linked!");
        //}

        if (Input.GetMouseButtonDown(0) && m_controller != null && m_controller.canAttack)
        {
            UnityEngine.Debug.Log("Setting Attack trigger");
            m_anim.SetTrigger("Attack");
        }
    }




    public void SetSword(Sword sword)
    {
        m_sword = sword;
    }

    public void ShowSwordAfterAttack()
    {
        if (m_sword != null)
        {
            m_sword.ShowSword();
        }
    }
}
