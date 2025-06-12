using UnityEngine;

namespace SupanthaPaul
{
    public class PlayerAnimator : MonoBehaviour
    {
        private Rigidbody2D m_rb;
        private PlayerController m_controller;
        private Animator m_anim;
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int JumpState = Animator.StringToHash("JumpState");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");

        private void Start()
        {
            m_anim = GetComponentInChildren<Animator>();
            m_controller = GetComponent<PlayerController>();
            m_rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            // Idle & Running animation
            m_anim.SetFloat(Move, Mathf.Abs(m_rb.velocity.x));

            // Jump/Fall animation
            m_anim.SetFloat(JumpState, m_rb.velocity.y);

            // Set jumping state
            m_anim.SetBool(IsJumping, !m_controller.IsGrounded && m_rb.velocity.y != 0);
        }
    }
}
