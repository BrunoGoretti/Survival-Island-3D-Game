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

    private int hours;
    public int Hours
    {
        get { return hours; }
        set { hours = value; OnHoursChange(value); }
    }

    private int days;
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
        Hours = 13;

        // Reset skybox and lighting settings
        RenderSettings.skybox.SetTexture("_Texture1", skyboxDay);
        RenderSettings.skybox.SetTexture("_Texture2", skyboxDay);
        RenderSettings.skybox.SetFloat("_Blend", 0);

        globalLight.color = Color.white;
        RenderSettings.fogColor = Color.white;
    }

    public void Update()
    {
        tempSecond += Time.deltaTime;

        if (tempSecond >= 1)
        {
            Minutes += 1;
            tempSecond = 0;
        }

        // Calculate the time of day as a fraction (0.0 to 1.0) where 0.0 is 00:00 and 1.0 is 24:00
        float timeOfDay = (hours * 60f + minutes) / (24f * 60f);

        // Calculate the target rotation based on the time of day
        float targetAngle = timeOfDay * 360f; // 360 degrees for a full day
        targetRotation = Quaternion.Euler(targetAngle - 90f, 0f, 0f); // -90 to start from horizon

        // Smoothly rotate the light
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

        // Adjust these times to make the light appear earlier and disappear later
        if (value >= 3 && value < 19) // Light starts increasing at 3 AM and fades after 7 PM
        {
            if (value < 7) // Before sunrise
            {
                lightIntensity = Mathf.Lerp(0.1f, 1f, (value - 3) / 4f); // Increase intensity from 0.1 to 1 from 3 AM to 7 AM
            }
            else if (value >= 7 && value < 17) // Daytime
            {
                lightIntensity = 1f; // Full intensity during the day
            }
            else // After sunset
            {
                lightIntensity = Mathf.Lerp(1f, 0.1f, (value - 17) / 2f); // Decrease intensity from 1 to 0.1 from 5 PM to 7 PM
            }
            lightRotation = new Vector3((value - 3) * 15f, 0f, 0f); // Rotate light based on adjusted time
        }
        else // Nighttime
        {
            lightIntensity = 0.1f;
            lightRotation = new Vector3((value >= 19 ? value - 19 : value + 5) * 15f + 270f, 0f, 0f); // Adjusted rotation for nighttime
        }

        globalLight.intensity = lightIntensity;
        targetRotation = Quaternion.Euler(lightRotation);

        // Skybox and lighting transitions
        if (value == 3)
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

    private void OnApplicationQuit()
    {
        // Reset skybox to default
        RenderSettings.skybox.SetTexture("_Texture1", skyboxDay); 
        RenderSettings.skybox.SetTexture("_Texture2", skyboxDay);
        RenderSettings.skybox.SetFloat("_Blend", 0);

        // Reset light settings to default
        globalLight.color = Color.white;
        globalLight.intensity = 1f;
        RenderSettings.fogColor = Color.white;
    }
}