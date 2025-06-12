using UnityEngine;
using UnityEngine.Rendering.Universal; // важно!

public class SheepGlow : MonoBehaviour
{
    public Light2D light2D;
    public float pulseSpeed = 2f;
    public float minIntensity = 0.2f;
    public float maxIntensity = 1f;

    void Update()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f; // от 0 до 1
        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }
}
