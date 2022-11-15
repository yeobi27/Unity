using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private readonly FullScreenMode fullscreen;


    // Main panels
    public GameObject panel_Intro;
    public GameObject panel_Win;
    public GameObject panel_Lose;
    public GameObject panel_Stats;

    public GameObject fantasticPan;

    // Stats
    public GameObject scoreManager;
    public GameObject timerManager;
    public GameObject highscore;

    // Misc
    public AssetCollection titles;


    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else Instance = this;
    }

    void Start()
    {
        Screen.SetResolution(Screen.width, (Screen.width * 9) / 16, fullscreen);
    }

    public void Reset()
    {
        panel_Stats.SetActive(false);
        panel_Lose.SetActive(false);
        panel_Intro.SetActive(true);
        fantasticPan.SetActive(false);
    }

    private void WinSetup(float timeLeft)
    {                                           
        titles.GetComponent<Image>().sprite = titles.assets[Mathf.FloorToInt((30 - timeLeft) * 0.2f)];

        string playerLevel = "";

        if (timeLeft <= 30f && timeLeft > 20f) //fantastic
        {
            playerLevel = "Fantastic";
            fantasticPan.SetActive(true);
        }
        else if (timeLeft <= 20f && timeLeft > 15f)   //excellent
        {
            playerLevel = "Excellent";
        }
        else if (timeLeft <= 15f && timeLeft > 10f)   //awesome
        {
            playerLevel = "Amazing";
        }
        else if (timeLeft <= 10f && timeLeft > 5f)    //great
        {
            playerLevel = "Great";
        }
        else if (timeLeft <= 5f && timeLeft > 0f)  //good
        {
            playerLevel = "Good";
        }
        PlayerPrefs.SetString(AppManager.Instance.gameName + "Level", playerLevel);
    }

    public IEnumerator OnRoundStart()
    {
        scoreManager.SetActive(true);
        timerManager.SetActive(true);
        highscore.SetActive(true);

        panel_Intro.SetActive(false);
        panel_Win.SetActive(false);
        panel_Lose.SetActive(false);
        panel_Stats.SetActive(false);
        fantasticPan.SetActive(false);

        yield return null;
    }

    public IEnumerator OnRoundEnd()
    {
        if (DataManager.Instance.scoreManager.ReachedHighscore())
        {
            panel_Win.SetActive(true);
            PlayerPrefs.SetString(AppManager.Instance.gameName + "State", "Success");
            WinSetup(DataManager.Instance.timerManager.timeLeft);
        }
        else
        {
            panel_Lose.SetActive(true);
            PlayerPrefs.SetString(AppManager.Instance.gameName + "State", "Failure");
        }

        panel_Stats.SetActive(true);
        scoreManager.SetActive(false);
        timerManager.SetActive(false);
        highscore.SetActive(false);

        yield return null;
    }
}

