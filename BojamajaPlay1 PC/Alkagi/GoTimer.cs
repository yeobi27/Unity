using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class GoTimer : MonoBehaviour
{
    public static UnityAction RoundEnd = null;

    public float timeLeft;
    public int roundLength;
    public static float copyTime;

    public Slider timerSlider;
    public Image sliderHandle;

    int levelCount = 0;
    int levelMax1 = 2000, levelMax2 = 4000, levelMax3 = 6000, levelMax4 = 8000;

    private void Awake()
    {
        timeLeft = roundLength;
        timerSlider.value = timeLeft / roundLength;
    }

    public void StartTimer()
    {
        timeLeft = roundLength;
        copyTime = timeLeft;
        StartCoroutine(Clock());
    }

    IEnumerator Clock()
    {
        RectTransform rectTran = sliderHandle.gameObject.GetComponent<RectTransform>();
        

        while (timeLeft > 0 )//&& !WindowDataManager.instance.GameEndScoreState())
        {
            timeLeft -= Time.deltaTime;
            copyTime = timeLeft;

            if (timeLeft <= 0)
                timeLeft = 0;

            if (timeLeft <= 30f && timeLeft >= 15f)
            {
                rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 75f);
                sliderHandle.sprite = Resources.Load<Sprite>("TimerIcon_1");
            }
            else if (timeLeft < 15f && timeLeft >= 5f)
            {
                sliderHandle.sprite = Resources.Load<Sprite>("TimerIcon_2");

                if (timeLeft < 15f && timeLeft > 14.98f)
                    GoSoundManager.Instance.IconImageChange();
            }
            else if (timeLeft < 5f && timeLeft >= 0)
            {
                rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100f);
                sliderHandle.sprite = Resources.Load<Sprite>("TimerIcon_3");
                if (timeLeft < 5f && timeLeft > 4.98f)
                {
                    GoSoundManager.Instance.IconImageChange();
                    GoSoundManager.Instance.sfxLimitFiveSec();
                }

            }

            if (GoDataManager.instance.score > 0 && GoDataManager.instance.score <= levelMax1 && levelCount == 0)
            {
                GoSoundManager.Instance.LevelUpSound();
                levelCount++;
            }
            else if (GoDataManager.instance.score > levelMax1 && GoDataManager.instance.score <= levelMax2 && levelCount == 1)
            {
                GoSoundManager.Instance.LevelUpSound();
                levelCount++;
            }
            else if (GoDataManager.instance.score > levelMax2 && GoDataManager.instance.score <= levelMax3 && levelCount == 2)
            {
                GoSoundManager.Instance.LevelUpSound();
                levelCount++;
            }
            else if (GoDataManager.instance.score > levelMax3 && GoDataManager.instance.score <= levelMax4 && levelCount == 3)
            {
                GoSoundManager.Instance.LevelUpSound();
                levelCount++;
            }
            else if (GoDataManager.instance.score > levelMax4 && levelCount == 4)
            {
                GoSoundManager.Instance.LevelUpSound();
                levelCount++;
            }

            timerSlider.value = timeLeft / roundLength;

            yield return new WaitForEndOfFrame();

        }

        GoSoundManager.Instance.StopSelectedSfx("타이머");

        if (RoundEnd != null)
            RoundEnd.Invoke();
    }

}
