using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TreeSlashUIManager : MonoBehaviour
{
    public static TreeSlashUIManager instance { get; private set; }

    [Header("[카운트다운]")]
    public Image startCountImg;    //3.2.1카운터 이미지
    public Image gameTitle; //시작 시 나오는 게임 타이틀
    //public Text endCountImg;   //5.4.3.2.1 카운터 이미지

    [Header("[실패성공화면]")]
    public Image successImage;  //성공 시 레벨 타이틀
    public GameObject Failure; 
    public GameObject Success; 
    public GameObject fantasticPan; 
    public GameObject[] starLevel; 
    public GameObject[] finishLevel;   
    public TreeSlashTimer playTimer; 
    public GameObject TopTextGroup;

    [Header("[스코어]")]
    public Text score;
    public Text success_scroe;  
    public int startCountTime;  //3초 카운터
    public int endCountTime;    //5초 카운터
    int currStartCountTime; //현재 시작 카운터
    int currEndCountTime; //현재 종료 카운터

    int scoreNum;
    int levelMax1 = 1000, levelMax2 = 2500, levelMax3 = 4000, levelMax4 = 5000;


    private float f_totalScore;

    void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;

        currStartCountTime = startCountTime;   
        currEndCountTime = endCountTime;    
    }

    void Start()
    {
        f_totalScore = 0f;
    }

    private void Update()
    {
        if (TreeSlashGameManager.instance.gamePlay)
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

    public void SetScore(int points)
    {
        score.text = points.ToString();
        f_totalScore = points;
    }

    //3.2.1
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
                startCountImg.gameObject.SetActive(true);   //3.2.1
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
        TopTextGroup.SetActive(true);
        Success.SetActive(false);
        Failure.SetActive(false);

        yield return null;
    }

    public IEnumerator GameEnd()
    {
        TopTextGroup.SetActive(false);

        if (TreeSlashDataManager.instance.GameEndScoreState())
        {
            FinishLevelShow();  
            Success.SetActive(true);

            float timeNum = TreeSlashTimer.copyTime;

            success_scroe.text = score.text;

            PlayerPrefs.SetString("TreeSlashState", "Success"); 
            PlayerPrefs.SetFloat("TreeSlashTime", timeNum);
            PlayerPrefs.SetString("TreeSlashScore", score.text);


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

            PlayerPrefs.SetString("TreeSlashLevel", playerLevel);
            TreeSlashSoundManager.Instance.bgmAfterGameEnd("Success Screen");
            StartCoroutine(EndCount());
            StartCoroutine(NextSceneChange());  //다음 게임으로
        }
        else
        {
            PlayerPrefs.SetString("TreeSlashState", "Failure"); 
 
            Failure.SetActive(true);
            TreeSlashSoundManager.Instance.bgmAfterGameEnd("Fail Screen");
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

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
}
