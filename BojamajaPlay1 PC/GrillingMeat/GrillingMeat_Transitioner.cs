using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GrillingMeat_Transitioner : MonoBehaviour
{
    private Image image;
    private float b;
    private float perc = 0f;
    private string parentName;
    private bool b_isEndProc;

    public float time = 5.1f;
    public UnityEvent onTransition = null;

    void OnEnable()
    {
        b = 5.1f;
        b_isEndProc = true;
    }

    void Start()
    {
        image = GetComponent<Image>();
        parentName = transform.parent.name;
    }

    void Update()
    {
        if (b_isEndProc)
        {
            b_isEndProc = false;
            GrillingMeat_SoundManager.Instance.bgmPlayerVolumeControll(0f);
            GrillingMeat_SoundManager.Instance.bgmAfterGameEnd(parentName);
        }

        b -= Time.deltaTime;

        perc = b / time;

        image.fillAmount = Mathf.LerpAngle(0f, 1f, perc);

        if (image.fillAmount == 0f)
        {
            GrillingMeat_SoundManager.Instance.bgmPlayerVolumeControll(1f);
            GrillingMeat_SoundManager.Instance.StopSfx();
            onTransition.Invoke();
        }
    }
}
