using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionPulse : MonoBehaviour
{
    private Color[] colors;

    public Material[] sharedMaterials;
    public float frequency = 1f;

    void Start()
    {
        StartCoroutine(Pulse());
    }

    IEnumerator Pulse()
    {
        float value = 1f;
        colors = new Color[sharedMaterials.Length];
        int i = 0;

        foreach (Material sharedMaterial in sharedMaterials)
        {
            colors[i++] = sharedMaterial.GetColor("_EmissionColor");
        }

        while (true)
        {
            i = 0;
            value = (Mathf.Sin(Time.time * frequency) + 2f) / 3f;

            foreach (var mat in sharedMaterials)
            {
                colors[i].r = value;

                mat.SetColor("_EmissionColor", colors[i++]);
            }

            yield return null;
        }
    }
}
