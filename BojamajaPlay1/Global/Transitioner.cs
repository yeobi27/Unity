using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Transitioner : MonoBehaviour
{
    private Image image;
    float value = 0;
    float _time = 0f;
    public float time = 5f;

    void OnEnable()
    {
        time = 5f;
    }

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        time -= Time.deltaTime;

        if (time < _time)
        {
            time = _time;
        }

        value = time / 5f;
        image.fillAmount = Mathf.LerpAngle(0f, 1f, value);
    }
}
