using UnityEngine;
using System.Collections;

public class SheepClue : MonoBehaviour
{
    public GameObject sheepLight;
    public float blinkDuration = 0.3f;
    public float pauseBetweenBlinks = 0.2f;
    public float pauseBetweenNumbers = 1.0f;

    public float initialDelay = 4f;
    public float repeatDelay = 30f;

    void Start()
    {
        sheepLight.SetActive(false); // 👈 Сначала темно
        StartCoroutine(ClueLoop());
    }

    IEnumerator ClueLoop()
    {
        yield return new WaitForSeconds(initialDelay);

        // Включаем свет перед началом подсказки
        sheepLight.SetActive(true);
        yield return new WaitForSeconds(0.2f); // небольшой эффект "включения"

        while (!PuzzleManager.Instance.puzzleSolved)
        {
            yield return StartCoroutine(PlayClueSequence());

            if (PuzzleManager.Instance.puzzleSolved)
                break;

            yield return new WaitForSeconds(repeatDelay);
        }

        // После решения можно выключить свет (если нужно)
        sheepLight.SetActive(false);
    }

    IEnumerator PlayClueSequence()
    {
        int[] clue = PuzzleManager.Instance.correctOrder;

        foreach (int number in clue)
        {
            for (int i = 0; i < number + 1; i++)
            {
                sheepLight.SetActive(true);
                yield return new WaitForSeconds(blinkDuration);
                sheepLight.SetActive(false);
                yield return new WaitForSeconds(pauseBetweenBlinks);
            }

            yield return new WaitForSeconds(pauseBetweenNumbers);
        }
    }
}
