using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoDataManager : MonoBehaviour
{
    public static GoDataManager instance { get; private set; }

    //public GameObject successParticle;
    public GoTimer playTime;

    public int score;
    public int totalScroe;

    private BlackGoStoneSpawn blackGoStoneSpawn;
    private WhiteGoStoneSpawn whiteGoStoneSpawn;


    void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        blackGoStoneSpawn = FindObjectOfType<BlackGoStoneSpawn>();
        whiteGoStoneSpawn = FindObjectOfType<WhiteGoStoneSpawn>();
    }

    public bool GameEndScoreState()
    {
        return score > 0;
    }


    public IEnumerator GameStart()
    {
        playTime.StartTimer(); // playtime 30sec
        blackGoStoneSpawn.StartSpawner();
        whiteGoStoneSpawn.StartSpawner();

        yield return null;
    }

    public IEnumerator GameEnd()
    {
        // stop spawn
        blackGoStoneSpawn.OnRoundEnd();
        whiteGoStoneSpawn.OnRoundEnd();
        if (GameEndScoreState())
            Resources.UnloadUnusedAssets();

        yield return null;
    }

    public void AddScore(float points)
    {
        score += (int)points;

        GoUIManager.instance.SetScore(score);
    }
}
