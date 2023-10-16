using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float dayDuration = 60.0f; // The duration of a full day in seconds.
    public Transform sun;
    public Light sunLight;
    public Light moonLight;

    private bool isDay = true;
    private float currentTime = 0.0f;

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > dayDuration)
        {
            currentTime = 0;
            isDay = !isDay;
        }

        // Interpolate sun/moon intensity based on time of day.
        float lerpFactor = currentTime / dayDuration;
        if (isDay)
        {
            sunLight.intensity = Mathf.Lerp(0.2f, 1.0f, lerpFactor);
            moonLight.intensity = Mathf.Lerp(1.0f, 0.2f, lerpFactor);
        }
        else
        {
            sunLight.intensity = Mathf.Lerp(1.0f, 0.2f, lerpFactor);
            moonLight.intensity = Mathf.Lerp(0.2f, 1.0f, lerpFactor);
        }
    }
}
