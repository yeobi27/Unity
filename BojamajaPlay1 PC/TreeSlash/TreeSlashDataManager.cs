using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSlashDataManager : MonoBehaviour
{
    public static TreeSlashDataManager instance { get; private set; }

    //public GameObject successParticle; 
    public TreeSlashTimer playTime; 

    public int score;   
    public int totalScore;

    [Header("Particle")]
    public GameObject hitParticle;

    void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void AddScore(float points)
    {
        score += (int)points;

        TreeSlashUIManager.instance.SetScore(score);
    }

    public bool GameEndScoreState()
    {
        //Debug.Log("GameEndScoreState");
        return score > 0;//>= totalScore;
    }

    public IEnumerator GameStart()
    {
        ResetScore();
        TreeSlashUIManager.instance.SetScore(score);
        playTime.StartTimer(); 

        yield return null;
    }

    public IEnumerator GameEnd()
    {
        if (GameEndScoreState())
            Resources.UnloadUnusedAssets();

        yield return null;
    }
}
