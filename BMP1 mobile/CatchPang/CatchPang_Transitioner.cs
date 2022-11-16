using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CatchPang_Transitioner : MonoBehaviour
{
    private Image image;
    private float b;
    private float perc = 0f;
    private string parentName;
    private bool b_isEndProc;
    private bool b_isRequestAd;

    public float time = 5.2f;

    public UnityEvent onTransition = null;

    void OnEnable()
    {
        b = 5.2f;
        b_isEndProc = true;
        b_isRequestAd = true;
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
            GameManager.instance.countForAdvertising += 1;

            TimeManager.instance.diamondSu = TimeManager.instance.diamondSu == -1 ? TimeManager.instance.diamondSu = 0 : TimeManager.instance.diamondSu -= 1;

            b_isEndProc = false;

            CatchPang_SoundManager.Instance.bgmPlayerVolumeControll(0f);

            CatchPang_SoundManager.Instance.bgmAfterGameEnd(parentName);
        }

        b -= Time.deltaTime;

        perc = b / time;
        image.fillAmount = Mathf.LerpAngle(0f, 1f, perc);

        if (image.fillAmount < 0.5f && b_isRequestAd)
        {
            if (TimeManager.instance.diamondSu == 0)
            {                
                Time.timeScale = 0f;

                AdManager.Instance.ShowRewardAd();

                Time.timeScale = 1f;
            }
            else if (GameManager.instance.countForAdvertising > 2)
            {
                Time.timeScale = 0f;

                GameManager.instance.countForAdvertising = 0;
                // 전면광고
                AdManager.Instance.ShowScreenAd();

                Time.timeScale = 1f;
            }

            b_isRequestAd = false;
        }

        if (image.fillAmount == 0f)
        {
            CatchPang_SoundManager.Instance.bgmPlayerVolumeControll(1f);
            CatchPang_SoundManager.Instance.StopSfx();            
            onTransition.Invoke();
        }
    }
}
