using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light lightSource;

    public float secondsInbetween;
    public float secondsLightsOut;
    
    
    void Start()
    {
        lightSource = GetComponent<Light>();
        lightSource.enabled = true;

    }

    public void StartFlicker()
    {
        StartCoroutine("Flicker");
    }
    public void StopFlicker()
    {
        StopCoroutine("Flicker");

        lightSource.enabled = true;
    }

    IEnumerator Flicker()
    {
        while (DataManager.Instance.timerManager.timeLeft > 0f)
        {
            yield return new WaitForSeconds(secondsInbetween);
            lightSource.enabled = false;
            SoundManager.Instance.PlaySFX("lightSwitch-sound");
            yield return new WaitForSeconds(secondsLightsOut);
            lightSource.enabled = true;
            SoundManager.Instance.PlaySFX("lightSwitch-sound");
        }
        yield return null;
    }
}
