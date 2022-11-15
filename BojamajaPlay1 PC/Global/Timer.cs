using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Text;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    private bool warningOn;
    [HideInInspector]
    public float timeLeft { get; private set; }

    public int roundLength;
    public Text text_Timer;
    public Text text_RoundEndTime;

    public static UnityAction RoundEnd = null;


    private void OnDisable()
    {
        if (DataManager.Instance.timerManager.timeLeft > 0)
        {
            text_RoundEndTime.text = (roundLength - timeLeft).ToString("F2").Replace(".", ":");

            PlayerPrefs.SetFloat(AppManager.Instance.gameName + "Time", (roundLength - timeLeft));
        }
        else
        {
            text_RoundEndTime.text = "<color=#ED6F6A>OVER</color>";
        }

    }

    public void StartTimer()
    {
        timeLeft = roundLength;
        warningOn = false;
        StartCoroutine(Clock());
    }
    private IEnumerator Clock()
    {
        bool gameOver = true;

        while (timeLeft > 0 && gameOver)
        {
            timeLeft -= Time.deltaTime;

            text_Timer.text = timeLeft.ToString("N2").Replace(".", ":");

            yield return new WaitForEndOfFrame();

            if (timeLeft < 10f && warningOn == false)
            {
                warningOn = true;
                SoundManager.Instance.timer.Play();
            }

            if (RoundEnd != null && DataManager.Instance.scoreManager.ReachedHighscore())
            {
                SoundManager.Instance.success.Play();
                RoundEnd.Invoke();

                gameOver = false;
            }
        }

        if (gameOver)
        {
            SoundManager.Instance.fail.Play();
            RoundEnd.Invoke();
        }
    }
}
