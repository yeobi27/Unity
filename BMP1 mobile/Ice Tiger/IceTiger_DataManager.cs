using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceTiger_DataManager : MonoBehaviour
{
    [Header("Data Information")]
    public float hammerDamage; 
    //public int scoreCostOnGettingHit;  
    public IceTiger_Timer timer;    
    public int score;   
    public int highscore;

    [Header("Lv Score Text")]
    public Text SuccessTime;
    public Text SuccessScore;
    public Text FailedTime;
    public Text FailedScore;

    [Header("Particles")]
    public GameObject fireworks;
    public GameObject[] hitParticle;

    public static IceTiger_DataManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else Instance = this;
    }
    
    public void ResetScore()
    {
        score = 0;
    }

    public void AddScore(float points)
    {
        score += (int)points;

        IceTiger_UIManager.Instance.SetScore(score);
    }

    public bool WonRound()
    {
        return score >= highscore;
    }

    public IEnumerator _Data_Start()
    {
        
        fireworks.SetActive(false);
        timer.StartTimer();
        ResetScore();

        yield return null;
    }

    public IEnumerator _Data_End()
    {
        if (WonRound())
        {
            SuccessTime.text = IceTiger_Timer.Instance.timeLeft.ToString("N2");

            SuccessScore.text = score.ToString();
            fireworks.SetActive(true);
        }
        else
        {
            //FailedTime.text = "00'00\"";
            FailedScore.text = score.ToString();
        }

        yield return null;
    }
}
