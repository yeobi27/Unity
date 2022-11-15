using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SaturationInterpolator : MonoBehaviour
{
    PostProcessVolume volume;
    ColorGrading colorGrading;
    public float speed = 5f;

    void Awake()
    {
        volume = GetComponent<PostProcessVolume>();

        colorGrading = volume.profile.GetSetting<ColorGrading>();
    }

    void OnEnable()
    {
        colorGrading.saturation.value = 0f;

        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        while (colorGrading.saturation.value > -99f)
        {
            colorGrading.saturation.value = Mathf.Lerp(colorGrading.saturation.value, -100f, speed * Time.deltaTime);
            yield return null;
        }
        colorGrading.saturation.value = -100f;
    }
}
