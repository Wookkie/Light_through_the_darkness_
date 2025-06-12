using UnityEngine;
using System.Linq;
public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [Header("Комбинация")]
    public int[] correctOrder = { 0, 2, 4 }; // 3 из 6 факелов

    private int currentStep = 0;
    public bool puzzleSolved { get; private set; } = false; // ← теперь публичный getter


    [Header("Факелы и дверь")]
    public Torch[] allTorches;
    public GameObject door;

    private void Awake()
    {
        Instance = this;
    }

   public void TryActivateTorch(int id, Torch torch)
{
    if (puzzleSolved) return;

    // Если этот факел вообще не в комбинации → сброс
    if (!correctOrder.Contains(id))
    {
        Debug.Log($"Факел {id} — лишний. Сброс.");
        ResetPuzzle();
        return;
    }

    // Проверка правильного шага по порядку
    if (id == correctOrder[currentStep])
    {
        if (!torch.isLit)
        {
            torch.LightUp();
            currentStep++;

            if (currentStep >= correctOrder.Length)
            {
                SolvePuzzle();
            }
        }
    }
    else
    {
        Debug.Log($"Факел {id} не в нужной последовательности. Сброс.");
        ResetPuzzle();
    }
}



    private void SolvePuzzle()
    {
        puzzleSolved = true;
        door.SetActive(false); // убираем дверь
        Debug.Log("Дверь открыта!");
    }

    private void ResetPuzzle()
    {
        currentStep = 0;

        foreach (Torch t in allTorches)
        {
            t.ResetTorch();
        }

        Debug.Log("Неверная последовательность. Сброс.");
    }
}
