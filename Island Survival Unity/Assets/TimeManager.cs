using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Cubemap skyboxDay;
    [SerializeField] private Cubemap skyboxSunset;
    [SerializeField] private Cubemap skyboxNight;
    [SerializeField] private Cubemap skyboxSunrise;

    [SerializeField] private Gradient gradientDayToSunset;
    [SerializeField] private Gradient gradientSunsetToNight;
    [SerializeField] private Gradient gradientNightToSunrise;
    [SerializeField] private Gradient gradientSunriseToDay;

    [SerializeField] private Light globalLight;

    public int minutes;
    public int Minutes
    {
        get { return minutes; }
        set { minutes = value; OnMinutesChange(value); }
    }

    public int hours = 5;
    public int Hours
    {
        get { return hours; }
        set { hours = value; OnHoursChange(value); }
    }

    public int days;
    public int Days
    {
        get { return days; }
        set { days = value; }
    }

    public float tempSecond;

    private Quaternion targetRotation;
    private float rotationSpeed = 1f; // Adjust rotation speed as needed

    public void Update()
    {
        tempSecond += Time.deltaTime;

        if (tempSecond >= 1)
        {
            Minutes += 1;
            tempSecond = 0;
        }

        // Smoothly rotate the light towards the target rotation
        globalLight.transform.rotation = Quaternion.Slerp(globalLight.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnMinutesChange(int value)
    {
        // Calculate total minutes passed in the day
        int totalMinutes = (hours * 60) + minutes;

        // Calculate the rotation based on time (360 degrees over 1440 minutes)
        float rotationAmount = (totalMinutes / 1440f) * 360f;

        // Assuming a light that starts at the horizon at 6:00 AM and returns to the horizon at 6:00 PM
        targetRotation = Quaternion.Euler(new Vector3(rotationAmount - 90f, 0, 0));

        if (value >= 60)
        {
            Hours++;
            minutes = 0;
        }
        if (Hours >= 24)
        {
            Hours = 0;
            Days++;
        }
    }

    private void OnHoursChange(int value)
    {
        if (value == 6)
        {
            StartCoroutine(LerpSkybox(skyboxNight, skyboxSunrise, 10f));
            StartCoroutine(LerpLight(gradientNightToSunrise, 10f));
            StartCoroutine(LerpLightIntensity(0.1f, 0.5f, 10f));
        }
        else if (value == 8)
        {
            StartCoroutine(LerpSkybox(skyboxSunrise, skyboxDay, 10f));
            StartCoroutine(LerpLight(gradientSunriseToDay, 10f));
            StartCoroutine(LerpLightIntensity(0.5f, 1f, 10f));
        }
        else if (value == 18)
        {
            StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 10f));
            StartCoroutine(LerpLight(gradientDayToSunset, 10f));
            StartCoroutine(LerpLightIntensity(1f, 0.5f, 10f));
        }
        else if (value == 22)
        {
            StartCoroutine(LerpSkybox(skyboxSunset, skyboxNight, 10f));
            StartCoroutine(LerpLight(gradientSunsetToNight, 10f));
            StartCoroutine(LerpLightIntensity(0.5f, 0.1f, 10f));
        }
    }

    private IEnumerator LerpLightIntensity(float from, float to, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            globalLight.intensity = Mathf.Lerp(from, to, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        globalLight.intensity = to;
    }

    private IEnumerator LerpSkybox(Cubemap a, Cubemap b, float time)
    {
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", i / time);
            yield return null;
        }
        RenderSettings.skybox.SetTexture("_Texture1", b);
    }

    private IEnumerator LerpLight(Gradient lightGradient, float time)
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            globalLight.color = lightGradient.Evaluate(i / time);
            RenderSettings.fogColor = globalLight.color;
            yield return null;
        }
    }
}