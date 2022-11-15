using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int score;
    public int highscore;

    public Text text_Score;
    public Text text_Highscore;
    public Text text_RoundEndScore;

    void Start()
    {
        text_Highscore.text = highscore.ToString();
    }

    void OnDisable()
    {
        text_RoundEndScore.text = text_Score.text;
        
        PlayerPrefs.SetString(AppManager.Instance.gameName + "Score", text_RoundEndScore.text);
    }

    public void Set(float points)
    {
        score = (int)points;
        text_Score.text = score.ToString();
    }
    public void Add(float points)
    {
        score += (int)points;
        text_Score.text = score.ToString();
    }
    public bool ReachedHighscore()
    {
        return score >= highscore;
    }
}
