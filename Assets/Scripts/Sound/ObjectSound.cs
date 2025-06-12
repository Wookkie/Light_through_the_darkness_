using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Animator))]
public class AnimatedObjectSound : MonoBehaviour
{
    [Header("������������ �������� � ������")]
    public List<AnimationSound> animationSounds = new List<AnimationSound>();

    private AudioSource audioSource;
    private Animator animator;

    private AudioClip currentClip;

    [System.Serializable]
    public class AnimationSound
    {
        public string animationName;    // ��� ��������� ��������
        public AudioClip soundClip;     // ���� ��� ���� ��������
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        audioSource.playOnAwake = false;
        audioSource.loop = true; // ���� ����� ������, ���� ������� ��������
    }

    private void Update()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        foreach (var animSound in animationSounds)
        {
            if (state.IsName(animSound.animationName))
            {
                // ���� �������� �������, �� ���� ��� ������ � ������ �� ������
                if (audioSource.clip == animSound.soundClip && audioSource.isPlaying)
                    return;

                // � ��������� ������ � ������ ����
                audioSource.clip = animSound.soundClip;
                audioSource.Play();
                currentClip = animSound.soundClip;
                return;
            }
        }

        // ���� ������� �������� �� � ������ � ������������� ����
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = null;
            currentClip = null;
        }
    }
}
