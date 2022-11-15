using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrillingMeat_DataManager : MonoBehaviour
{
    [Header("Data Information")]
    public GrillingMeat_Timer timer;
    public int score;
    public int highscore;

    [Header("Lv Score Text")]
    public Text SuccessTime;
    public Text SuccessScore;
    public Text FailedTime;
    public Text FailedScore;

    public static GrillingMeat_DataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }
    
    public void ResetScore()
    {
        score = 0;
    }

    public void AddScore(float points)
    {
        score += (int)points;

        GrillingMeat_UIManager.Instance.SetScore(score);
    }

    public bool WonRound()
    {
        return score >= highscore;
    }

    public IEnumerator _Data_Start()
    {
        timer.StartTimer();
        ResetScore();

        yield return null;
    }

    public IEnumerator _Data_End()
    {
        if (WonRound())
        {
            SuccessTime.text = GrillingMeat_Timer.Instance.timeLeft.ToString("N2");

            SuccessScore.text = score.ToString();
        }
        else
        {
            FailedScore.text = score.ToString();
        }

        yield return null;
    }
}
