using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoUIManager : MonoBehaviour
{
    public static GoUIManager instance { get; private set; }

    [Header("[카운트다운]")]
    public Image startCountImg;    //3.2.1
    public Image gameTitle;
    //public Text endCountImg;   //5.4.3.2.1

    [Header("[실패성공화면]")]
    public Image successImage;
    public GameObject Failure;
    public GameObject Success;
    public GameObject fantasticPan;
    public GameObject[] starLevel;
    public GameObject[] finishLevel;
    public GoTimer playTimer;
    public GameObject topTextGroup;
    public GameObject goPan;

    [Header("[스코어]")]
    public Text score;
    public Text success_scroe;

    public int startCountTime;  //3sec
    public int endCountTime;    //5sec
    int currStartCountTime;
    int currEndCountTime;

    int scoreNum;
    int levelMax1 = 2000, levelMax2 = 4000, levelMax3 = 6000, levelMax4 = 8000;

    private float f_totalScore;

    void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;

        currStartCountTime = startCountTime;
        currEndCountTime = endCountTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        f_totalScore = 0f;
    }

    
    private void Update()
    {
        if (GoGameManager.instance.gamePlay)
        {
            scoreNum = int.Parse(score.text);
            if (scoreNum > 0 && scoreNum <= levelMax1)
            {
                starLevel[0].SetActive(true);
            }
            else if (scoreNum > levelMax1 && scoreNum <= levelMax2)
            {
                starLevel[0].SetActive(true);
                starLevel[1].SetActive(true);
            }
            else if (scoreNum > levelMax2 && scoreNum <= levelMax3)
            {
                starLevel[0].SetActive(true);
                starLevel[1].SetActive(true);
                starLevel[2].SetActive(true);
            }
            else if (scoreNum > levelMax3 && scoreNum <= levelMax4)
            {
                starLevel[0].SetActive(true);
                starLevel[1].SetActive(true);
                starLevel[2].SetActive(true);
                starLevel[3].SetActive(true);
            }
            else if (scoreNum > levelMax4)
            {
                starLevel[0].SetActive(true);
                starLevel[1].SetActive(true);
                starLevel[2].SetActive(true);
                starLevel[3].SetActive(true);
                starLevel[4].SetActive(true);
            }

        }
    }

    public IEnumerator StartCount()
    {
        currStartCountTime = startCountTime;

        while (currStartCountTime > 0)
        {
            if (currStartCountTime > 3f)
                gameTitle.gameObject.SetActive(true);
            else
            {
                gameTitle.gameObject.SetActive(false);
                startCountImg.gameObject.SetActive(true);
                startCountImg.sprite = Resources.Load<Sprite>("Count" + currStartCountTime);
            }
            currStartCountTime--;
            yield return new WaitForSecondsRealtime(1f);
        }

        startCountImg.gameObject.SetActive(false);
    }

    //5.4.3.2.1
    public IEnumerator EndCount()
    {
        currEndCountTime = endCountTime;
        yield return new WaitForSecondsRealtime(1f);
    }

    public IEnumerator GameStart()
    {
        topTextGroup.SetActive(true);
        Success.SetActive(false);
        Failure.SetActive(false);

        yield return null;
    }

    public void SetScore(float points)
    {
        score.text = points.ToString();
        f_totalScore = points;
    }

    public IEnumerator GameEnd()
    {
        topTextGroup.SetActive(false);
        goPan.SetActive(false);

        if (GoDataManager.instance.GameEndScoreState())
        {
            FinishLevelShow();
            Success.SetActive(true);

            //시간 들고온다
            float timeNum = GoTimer.copyTime;

            success_scroe.text = score.text;

            PlayerPrefs.SetString("GoState", "Success");
            PlayerPrefs.SetFloat("GoTime", timeNum);
            PlayerPrefs.SetString("GoScore", score.text);


            string playerLevel = "";

            if (scoreNum > 0 && scoreNum <= levelMax1)
            {
                successImage.sprite = Resources.Load<Sprite>("Textures/Level/Good");
                playerLevel = "Good";
            }
            else if (scoreNum > levelMax1 && scoreNum <= levelMax2)
            {
                successImage.sprite = Resources.Load<Sprite>("Textures/Level/Great");
                playerLevel = "Great";
            }
            else if (scoreNum > levelMax2 && scoreNum <= levelMax3)
            {
                successImage.sprite = Resources.Load<Sprite>("Textures/Level/Amazing");
                playerLevel = "Amazing";
            }
            else if (scoreNum > levelMax3 && scoreNum <= levelMax4)
            {
                successImage.sprite = Resources.Load<Sprite>("Textures/Level/Excellent");
                playerLevel = "Excellent";
            }
            else if (scoreNum > levelMax4)
            {
                fantasticPan.SetActive(true);
                successImage.sprite = Resources.Load<Sprite>("Textures/Level/Fantastic");
                playerLevel = "Fantastic";
            }

            PlayerPrefs.SetString("GoLevel", playerLevel);
            GoSoundManager.Instance.bgmAfterGameEnd("Success Screen");
            StartCoroutine(EndCount());
            StartCoroutine(NextSceneChange());
        }
        else
        {
            PlayerPrefs.SetString("GoState", "Failure");

            Failure.SetActive(true);

            GoSoundManager.Instance.bgmAfterGameEnd("Fail Screen");
            StartCoroutine(EndCount());
            StartCoroutine(NextSceneChange());
        }

        yield return null;
    }

    void FinishLevelShow()
    {
        if (scoreNum > 0 && scoreNum <= levelMax1)
        {
            finishLevel[0].SetActive(true);
        }
        else if (scoreNum > levelMax1 && scoreNum <= levelMax2)
        {
            finishLevel[0].SetActive(true);
            finishLevel[1].SetActive(true);
        }
        else if (scoreNum > levelMax2 && scoreNum <= levelMax3)
        {
            finishLevel[0].SetActive(true);
            finishLevel[1].SetActive(true);
            finishLevel[2].SetActive(true);
        }
        else if (scoreNum > levelMax3 && scoreNum <= levelMax4)
        {
            finishLevel[0].SetActive(true);
            finishLevel[1].SetActive(true);
            finishLevel[2].SetActive(true);
            finishLevel[3].SetActive(true);
        }
        else if (scoreNum > levelMax4)
        {
            finishLevel[0].SetActive(true);
            finishLevel[1].SetActive(true);
            finishLevel[2].SetActive(true);
            finishLevel[3].SetActive(true);
            finishLevel[4].SetActive(true);
        }
    }

    IEnumerator NextSceneChange()
    {
        yield return new WaitForSeconds(5f);
        GameManager.instance.gamePlayNum += 1;

        if (GameManager.instance.gamePlayNum < GameManager.instance.gameTotalSu)
            GameManager.instance.SceneMove(GameManager.instance.gamePlayNum);
        else if (GameManager.instance.gamePlayNum == GameManager.instance.gameTotalSu)
            SceneManager.LoadScene("EndScene");
    }
}
