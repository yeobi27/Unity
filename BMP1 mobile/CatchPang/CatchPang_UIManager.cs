using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class CatchPang_UIManager : MonoBehaviour
{
    [Header("Top Score Img")]
    public GameObject timerObj;
    public GameObject scoreObj;
    public GameObject highscoreObj;

    [Header("End Screen Obj")]
    public GameObject endScreen;
    public GameObject failScreen;
    public GameObject HomeBtn;
    public GameObject fantasticPan;
    public Text time;
    public Text score;
    private string level;
    //public GameObject HomeBtnCanvas;

    [Header("Lv Score Img")]
    public Image Medal;
    public Image Level;
    private readonly FullScreenMode fullscreen;
    string timeChage;

    public static CatchPang_UIManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        Screen.SetResolution(Screen.width, (Screen.width * 9) / 16, fullscreen);
    }

    public void SetScore(float points)
    {
        score.text = points.ToString();
    }
    
    public IEnumerator OnRoundStart()
    {
        SetScore(0);

        timerObj.SetActive(true);
        scoreObj.SetActive(true);
        highscoreObj.SetActive(true);

        endScreen.SetActive(false);
        fantasticPan.SetActive(false);

        failScreen.SetActive(false);

        // HomeBtnCanvas.SetActive(false);

        HomeBtn.SetActive(false);

        yield return null;
    }

    public IEnumerator OnRoundEnd()
    {
        timeChage = time.text;
        timeChage = timeChage.Replace(":", ".");
        PlayerPrefs.SetFloat("CatchPangTime", float.Parse(timeChage));
        PlayerPrefs.SetString("CatchPangScore", score.text);


        timerObj.SetActive(false);
        scoreObj.SetActive(false);
        highscoreObj.SetActive(false);

        if (CatchPang_DataManager.Instance.WonRound())
        {
            PlayerPrefs.SetString("CatchPangState", "Success");
            // 클리어
            endScreen.SetActive(true);
            //HomeBtnCanvas.SetActive(true);

            if (float.Parse(CatchPang_DataManager.Instance.SuccessTime.text) < 30 && float.Parse(CatchPang_DataManager.Instance.SuccessTime.text) >= 25)
            {
                CatchPang_DataManager.Instance.SuccessTime.text = (30f - float.Parse(CatchPang_DataManager.Instance.SuccessTime.text)).ToString("N2").Replace(".", ":");// + "\"";
                Medal.sprite = Resources.Load("Textures/Level/Gold", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Fantastic", typeof(Sprite)) as Sprite;
                fantasticPan.SetActive(true);
                level = "Fantastic";
            }
            else if (float.Parse(CatchPang_DataManager.Instance.SuccessTime.text) < 25 && float.Parse(CatchPang_DataManager.Instance.SuccessTime.text) >= 20)
            {
                CatchPang_DataManager.Instance.SuccessTime.text = (30f - float.Parse(CatchPang_DataManager.Instance.SuccessTime.text)).ToString("N2").Replace(".", ":");// + "\"";
                Medal.sprite = Resources.Load("Textures/Level/Gold", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Fantastic", typeof(Sprite)) as Sprite;
                fantasticPan.SetActive(true);
                level = "Fantastic";
            }
            else if (float.Parse(CatchPang_DataManager.Instance.SuccessTime.text) < 20 && float.Parse(CatchPang_DataManager.Instance.SuccessTime.text) >= 15)
            {
                CatchPang_DataManager.Instance.SuccessTime.text = (30f - float.Parse(CatchPang_DataManager.Instance.SuccessTime.text)).ToString("N2").Replace(".", ":");// + "\"";
                Medal.sprite = Resources.Load("Textures/Level/Sliver", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Excellent", typeof(Sprite)) as Sprite;
                level = "Excellent";
            }
            else if (float.Parse(CatchPang_DataManager.Instance.SuccessTime.text) < 15 && float.Parse(CatchPang_DataManager.Instance.SuccessTime.text) >= 10)
            {
                CatchPang_DataManager.Instance.SuccessTime.text = (30f - float.Parse(CatchPang_DataManager.Instance.SuccessTime.text)).ToString("N2").Replace(".", ":");// + "\"";
                Medal.sprite = Resources.Load("Textures/Level/Sliver", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Amazing", typeof(Sprite)) as Sprite;
                level = "Awesome";
            }
            else if (float.Parse(CatchPang_DataManager.Instance.SuccessTime.text) < 10 && float.Parse(CatchPang_DataManager.Instance.SuccessTime.text) >= 5)
            {
                CatchPang_DataManager.Instance.SuccessTime.text = (30f - float.Parse(CatchPang_DataManager.Instance.SuccessTime.text)).ToString("N2").Replace(".", ":");// + "\"";
                Medal.sprite = Resources.Load("Textures/Level/Dong", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Great", typeof(Sprite)) as Sprite;
                level = "Great";
            }
            else if (float.Parse(CatchPang_DataManager.Instance.SuccessTime.text) < 5 && float.Parse(CatchPang_DataManager.Instance.SuccessTime.text) > 0)
            {
                CatchPang_DataManager.Instance.SuccessTime.text = (30f - float.Parse(CatchPang_DataManager.Instance.SuccessTime.text)).ToString("N2").Replace(".", ":");// + "\"";
                Medal.sprite = Resources.Load("Textures/Level/Dong", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Good", typeof(Sprite)) as Sprite;
                level = "Good";
            }

            PlayerPrefs.SetString("CatchPangLevel", level);

            HomeBtn.SetActive(true);

            StartCoroutine(NextSceneChange());
        }
        else
        {
            PlayerPrefs.SetString("CatchPangState", "Failure");

            failScreen.SetActive(true);
            //HomeBtnCanvas.SetActive(true);
            HomeBtn.SetActive(true);

            StartCoroutine(NextSceneChange());
        }

        yield return null;
    }

    IEnumerator NextSceneChange()
    {
        yield return new WaitForSeconds(5f);
        GameManager.instance.gamePlayNum += 1;

        if (TimeManager.instance.diamondSu == 0 && GameManager.instance.gamePlayNum != 10)
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            if (GameManager.instance.gamePlayNum < GameManager.instance.gameTotalSu)
                GameManager.instance.SceneMove(GameManager.instance.gamePlayNum);
            else if (GameManager.instance.gamePlayNum == GameManager.instance.gameTotalSu)
                SceneManager.LoadScene("EndScene");
        }
    }
}
