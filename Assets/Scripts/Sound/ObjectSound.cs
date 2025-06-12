using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Animator))]
public class AnimatedObjectSound : MonoBehaviour
{
    [Header("Соответствие анимаций и звуков")]
    public List<AnimationSound> animationSounds = new List<AnimationSound>();

    private AudioSource audioSource;
    private Animator animator;

    private AudioClip currentClip;

    [System.Serializable]
    public class AnimationSound
    {
        public string animationName;    // Имя состояния анимации
        public AudioClip soundClip;     // Звук для этой анимации
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        audioSource.playOnAwake = false;
        audioSource.loop = true; // звук будет играть, пока активна анимация
    }

    private void Update()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        foreach (var animSound in animationSounds)
        {
            if (state.IsName(animSound.animationName))
            {
                // если анимация совпала, но звук уже играет — ничего не делаем
                if (audioSource.clip == animSound.soundClip && audioSource.isPlaying)
                    return;

                // в противном случае — меняем звук
                audioSource.clip = animSound.soundClip;
                audioSource.Play();
                currentClip = animSound.soundClip;
                return;
            }
        }

        // Если текущая анимация не в списке — останавливаем звук
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = null;
            currentClip = null;
        }
    }
}
