using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlamePulse : MonoBehaviour
{
    public Light2D light2D;
    public float pulseSpeed = 2f;
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.2f;

    private float baseIntensity;

    void Start()
    {
        if (light2D == null)
            light2D = GetComponent<Light2D>();

        baseIntensity = light2D.intensity;
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f; // 0 → 1
        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, t) * baseIntensity;
    }
}
