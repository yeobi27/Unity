using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;

public class GrillingMeat_Timer : MonoBehaviour
{
    public static UnityAction RoundEnd = null;
    public static bool isPlaying;
    public int roundLength; // 30sec
    public float timeLeft;    // 0

    private Text timer;
    private string secToString;
    private bool istimeLimit;

    public static GrillingMeat_Timer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else Instance = this;
    }

    public void StartTimer()
    {
        istimeLimit = true;

        timeLeft = roundLength;

        timer = GetComponent<Text>();

        StartCoroutine(_Clock());
    }

    public IEnumerator _Clock()
    {
        isPlaying = true;
        bool gameOver = true;
        timeLeft = roundLength;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            timer.text = SecToString(timeLeft);

            if (timeLeft < 5f && istimeLimit)
            {
                istimeLimit = false;
                GrillingMeat_SoundManager.Instance.sfxLimitFiveSec();
            }

            if (RoundEnd != null && GrillingMeat_DataManager.Instance.WonRound())
            {
                gameOver = false;

                // RoundEnd CallBack
                RoundEnd.Invoke();

                timeLeft = 0;
            }

            yield return new WaitForEndOfFrame();
        }

        GrillingMeat_SoundManager.Instance.StopSelectedSfx("Limit5sec");

        timer.text = SecToString(30);

        isPlaying = false;

        if (gameOver)
        {
            RoundEnd.Invoke();
        }
    }

    private string SecToString(float sec)
    {
        string changeText;

        secToString = sec.ToString("N2");
        changeText = secToString.Replace(".", ":");

        return changeText;
    }


}