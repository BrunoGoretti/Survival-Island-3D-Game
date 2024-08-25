using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Cubemap skyboxNight;
    [SerializeField] private Cubemap skyboxSunrise;
    [SerializeField] private Cubemap skyboxDay;
    [SerializeField] private Cubemap skyboxSunset;

    [SerializeField] private Gradient gradientNightToSunrise;
    [SerializeField] private Gradient gradientSunriseToDay;
    [SerializeField] private Gradient gradientDayToSunset;
    [SerializeField] private Gradient gradientSunsetToNight;

    [SerializeField] private Light globalLight;

    public int minutes;
    public int Minutes
    {
        get { return minutes; }
        set { minutes = value; OnMinutesChange(value); }
    }

    public int hours;
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
    private float rotationSpeed = 1f;

    private void Start()
    {
        Hours = 8;
    }

    public void Update()
    {
        tempSecond += Time.deltaTime;

        if (tempSecond >= 1)
        {
            Minutes += 1;
            tempSecond = 0;
        }

        globalLight.transform.rotation = Quaternion.Slerp(globalLight.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnMinutesChange(int value)
    {


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
        float lightIntensity = 0.1f; // Default low intensity for night hours
        Vector3 lightRotation = Vector3.zero;

        if (value >= 5 && value < 17) // Adjusted time for earlier light intensity change
        {
            lightIntensity = Mathf.Lerp(0.1f, 1f, (value - 5) / 12f); // Increase intensity from 0.1 to 1 from 5 AM to 5 PM
            lightRotation = new Vector3((value - 5) * 15f, 0f, 0f); // Rotate light based on time
        }
        else if (value >= 17 || value < 5) // Adjusted time for earlier light intensity decrease
        {
            lightIntensity = Mathf.Lerp(1f, 0.1f, (value >= 17 ? value - 17 : value + 7) / 12f); // Decrease intensity from 1 to 0.1 from 5 PM to 5 AM
            lightRotation = new Vector3((value >= 17 ? value - 17 : value + 7) * 15f + 180f, 0f, 0f); // Rotate light for night
        }

        globalLight.intensity = lightIntensity;
        targetRotation = Quaternion.Euler(lightRotation);

        if (value == 5)
        {
            StartCoroutine(LerpSkybox(skyboxNight, skyboxSunrise, 10f));
            StartCoroutine(LerpLight(gradientNightToSunrise, 10f));
        }
        else if (value == 7)
        {
            StartCoroutine(LerpSkybox(skyboxSunrise, skyboxDay, 10f));
            StartCoroutine(LerpLight(gradientSunriseToDay, 10f));
        }
        else if (value == 17)
        {
            StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 10f));
            StartCoroutine(LerpLight(gradientDayToSunset, 10f));
        }
        else if (value == 19)
        {
            StartCoroutine(LerpSkybox(skyboxSunset, skyboxNight, 10f));
            StartCoroutine(LerpLight(gradientSunsetToNight, 10f));
        }
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