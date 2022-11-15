using System.Collections;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public GameObject fireworks;
    public Score scoreManager;
    public Timer timerManager;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else Instance = this;
    }

    public IEnumerator OnRoundStart()
    {
        fireworks.SetActive(false);
        timerManager.StartTimer();
        scoreManager.Set(0);

        yield return null;
    }
    public IEnumerator OnRoundEnd()
    {
        if (scoreManager.ReachedHighscore()) fireworks.SetActive(true);

        yield return null;
    }

    public void TimerStart()
    {
        timerManager.StartTimer();
    }
}
