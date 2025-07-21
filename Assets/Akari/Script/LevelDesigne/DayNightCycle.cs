using UnityEngine;
using System.Collections;


public class DayNightCycle : MonoBehaviour
{
    public float sensitivity;
    public float speedTime;
    public float orbitDamping;
    public Vector3 localRot;

    public int nightCount;
    public int dayCount = 1;
    public int numDay = 1;

    public bool checkCycle;
    public bool isDay = true;
    public bool isNight;
    public bool dayTransitionDone;
    public bool nightTransitionDone;


    [SerializeField] float dayIntensityMultiplier;
    [SerializeField] float nightIntensityMultiplier;
    [SerializeField] bool inTransition;
    [SerializeField] Material daySkyBox;
    [SerializeField] Material TransitionSkyBox;
    [SerializeField] Material nightSkyBox;
    [SerializeField] Light sunLight;
    [SerializeField] Light moonLight;


    private void Start()
    {
        RenderSettings.skybox = daySkyBox;
        RenderSettings.sun = sunLight;
        RenderSettings.ambientIntensity = dayIntensityMultiplier;
        TransitionSkyBox = new Material(RenderSettings.skybox.shader);
        TransitionSkyBox.CopyPropertiesFromMaterial(RenderSettings.skybox);
        RenderSettings.skybox = TransitionSkyBox;
    }

    void Update()
    {
        if (isNight && !nightTransitionDone && !inTransition)
            StartCoroutine(SmoothTransition(nightSkyBox, moonLight, nightIntensityMultiplier));
        if (isDay && !dayTransitionDone && !inTransition)
            StartCoroutine(SmoothTransition(daySkyBox, sunLight, dayIntensityMultiplier));

        localRot.y = Time.timeSinceLevelLoad * sensitivity;

        Quaternion qt = Quaternion.Euler(localRot.y, 0f, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, qt, Time.deltaTime * orbitDamping);

        if (Mathf.Approximately(Mathf.FloorToInt(localRot.y / 180f), nightCount + dayCount))
        {
            if (isNight)
            {
                StartCoroutine(AudioManager.instance.SmoothMusicTransition(AudioManager.instance.dayMusic, AudioManager.instance.dayMusicMixer));
                isNight = false;
                isDay = true;
                nightCount += 1;
                return;
            }
            if (isDay)
            {
                StartCoroutine(AudioManager.instance.SmoothMusicTransition(AudioManager.instance.nightMusic, AudioManager.instance.nightMusicMixer));
                isDay = false;
                isNight = true;
                dayCount += 1;
            }
        }
    }

    public IEnumerator SmoothTransition(Material skybox, Light light, float intensityMultiplier)
    {
        inTransition = true;


        while (speedTime < 1f)
        {
            if (isDay)
                Day();
            else if (isNight)
                Night();

            speedTime += Time.deltaTime * sensitivity;
            speedTime = Mathf.Clamp01(speedTime);
            speedTime = Mathf.SmoothStep(0f, 1f, speedTime);
            Debug.Log($"skybox: {skybox.name}");
            RenderSettings.skybox.Lerp(RenderSettings.skybox, skybox, speedTime);
            RenderSettings.sun = light;
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, intensityMultiplier, speedTime);
            yield return null;
        }
        inTransition = false;
        if (isNight)
        {
            speedTime = 0f;
            nightTransitionDone = true;
            dayTransitionDone = false;
        }
        else if (isDay)
        {
            speedTime = 0f;
            dayTransitionDone = true;
            nightTransitionDone = false;
        }
    }

    public void Day()
    {
        if (sunLight.intensity < 1.5f)
            sunLight.intensity += 0.05f;
    }

    public void Night()
    {
        if (sunLight.intensity > 0)
            sunLight.intensity -= 0.5f;
    }
}
