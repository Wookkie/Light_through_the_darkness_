using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : MonoBehaviour
{
    public bool isLit = false;
    public GameObject flame; // Объект с огнем (спрайт пламени)
    public Light2D light2D;  // Компонент света на огне

    public int torchID; // Индекс в последовательности

    private void Start()
    {
        SetState(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                ToggleTorch();
            }
        }
    }

    void ToggleTorch()
    {
        if (!isLit)
        {
            PuzzleManager.Instance.TryActivateTorch(torchID, this);
        }
        else
        {
            // При повторном нажатии выключаем
            SetState(false);
        }
    }

    public void LightUp()
    {
        SetState(true);
    }

    public void ResetTorch()
    {
        SetState(false);
    }

    private void SetState(bool lit)
    {
        isLit = lit;
        if (flame != null) flame.SetActive(lit);
        if (light2D != null) light2D.enabled = lit;
    }
}
