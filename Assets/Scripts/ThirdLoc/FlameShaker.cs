using UnityEngine;

public class FlameShaker : MonoBehaviour
{
    public float shakeAmount = 0.03f;     // Максимальное смещение по X/Y
    public float shakeSpeed = 20f;        // Скорость колебаний

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        float offsetX = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * 2f * shakeAmount;
        float offsetY = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * 2f * shakeAmount;

        transform.localPosition = initialPosition + new Vector3(offsetX, offsetY, 0f);
    }
}
