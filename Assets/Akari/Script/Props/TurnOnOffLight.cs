using UnityEngine;

public class TurnOnOffLight : MonoBehaviour
{
    public DayNightCycle dayNightCycle;
    public Light selfLight;

    // Update is called once per frame
    void Update()
    {
        if (dayNightCycle.isDay)
        {
            selfLight.enabled = false;
        }
        if (dayNightCycle.isNight)
        {
            selfLight.enabled = true;
        }
    }
}
