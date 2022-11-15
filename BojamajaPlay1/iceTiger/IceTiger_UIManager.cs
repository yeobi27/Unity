using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net.NetworkInformation;

public class IceTiger_UIManager : MonoBehaviour
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
    string timeChage;

    //public GameObject HomeBtnCanvas;

    [Header("Lv Score Img")]
    //public Image Medal;
    public Image Level;
    private readonly FullScreenMode fullscreen;

    public static IceTiger_UIManager Instance { get; private set; }
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

    public void SetScore(int points)
    {
        score.text = points.ToString();
    }

    public IEnumerator _UI_Start()
    {
        SetScore(0);

        timerObj.SetActive(true);
        scoreObj.SetActive(true);
        highscoreObj.SetActive(true);

        endScreen.SetActive(false);
        fantasticPan.SetActive(false);
        failScreen.SetActive(false);
        // use Leapmotion
        // HomeBtnCanvas.SetActive(false);
        // use mobile
        HomeBtn.SetActive(false);

        //Debug.Log("UIManager Start");

        yield return null;
    }

    public IEnumerator _UI_End()
    {
        timeChage = time.text;
        timeChage = timeChage.Replace(":", ".");

        PlayerPrefs.SetFloat("IceTigerTime", float.Parse(timeChage));
        PlayerPrefs.SetString("IceTigerScore", score.text);

        timerObj.SetActive(false);
        scoreObj.SetActive(false);
        highscoreObj.SetActive(false);

        if (IceTiger_DataManager.Instance.WonRound())
        {
            PlayerPrefs.SetString("IceTigerState", "Success");

            endScreen.SetActive(true);

            if (float.Parse(IceTiger_DataManager.Instance.SuccessTime.text) < 30 && float.Parse(IceTiger_DataManager.Instance.SuccessTime.text) >= 25)
            {
                IceTiger_DataManager.Instance.SuccessTime.text = (30f - float.Parse(IceTiger_DataManager.Instance.SuccessTime.text)).ToString().Replace(".", ":");// + "\"";
                //Medal.sprite = Resources.Load("Textures/Level/Gold", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Fantastic", typeof(Sprite)) as Sprite;
                fantasticPan.SetActive(true);
                level = "Fantastic";
            }
            else if (float.Parse(IceTiger_DataManager.Instance.SuccessTime.text) < 25 && float.Parse(IceTiger_DataManager.Instance.SuccessTime.text) >= 20)
            {
                IceTiger_DataManager.Instance.SuccessTime.text = (30f - float.Parse(IceTiger_DataManager.Instance.SuccessTime.text)).ToString().Replace(".", ":");// + "\"";
                //Medal.sprite = Resources.Load("Textures/Level/Gold", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Fantastic", typeof(Sprite)) as Sprite;
                fantasticPan.SetActive(true);
                level = "Fantastic";
            }
            else if (float.Parse(IceTiger_DataManager.Instance.SuccessTime.text) < 20 && float.Parse(IceTiger_DataManager.Instance.SuccessTime.text) >= 15)
            {
                IceTiger_DataManager.Instance.SuccessTime.text = (30f - float.Parse(IceTiger_DataManager.Instance.SuccessTime.text)).ToString().Replace(".", ":");// + "\"";
                //Medal.sprite = Resources.Load("Textures/Level/Sliver", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Excellent", typeof(Sprite)) as Sprite;
                level = "Excellent";
            }
            else if (float.Parse(IceTiger_DataManager.Instance.SuccessTime.text) < 15 && float.Parse(IceTiger_DataManager.Instance.SuccessTime.text) >= 10)
            {
                IceTiger_DataManager.Instance.SuccessTime.text = (30f - float.Parse(IceTiger_DataManager.Instance.SuccessTime.text)).ToString().Replace(".", ":");// + "\"";
                //Medal.sprite = Resources.Load("Textures/Level/Sliver", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Amazing", typeof(Sprite)) as Sprite;
                level = "Awesome";
            }
            else if (float.Parse(IceTiger_DataManager.Instance.SuccessTime.text) < 10 && float.Parse(IceTiger_DataManager.Instance.SuccessTime.text) >= 5)
            {
                IceTiger_DataManager.Instance.SuccessTime.text = (30f - float.Parse(IceTiger_DataManager.Instance.SuccessTime.text)).ToString().Replace(".", ":");// + "\"";
                // Medal.sprite = Resources.Load("Textures/Level/Dong", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Great", typeof(Sprite)) as Sprite;
                level = "Great";
            }
            else if (float.Parse(IceTiger_DataManager.Instance.SuccessTime.text) < 5 && float.Parse(IceTiger_DataManager.Instance.SuccessTime.text) > 0)
            {
                IceTiger_DataManager.Instance.SuccessTime.text = (30f - float.Parse(IceTiger_DataManager.Instance.SuccessTime.text)).ToString().Replace(".", ":");// + "\"";
                //Medal.sprite = Resources.Load("Textures/Level/Dong", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Good", typeof(Sprite)) as Sprite;
                level = "Good";
            }

            PlayerPrefs.SetString("IceTigerLevel", level);

            HomeBtn.SetActive(true);

            StartCoroutine(NextSceneChange());
        }
        else
        {
            PlayerPrefs.SetString("IceTigerState", "Failure");

            // 다시 시작
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

        if (GameManager.instance.gamePlayNum < GameManager.instance.gameTotalSu)
        {
            GameManager.instance.SceneMove(GameManager.instance.gamePlayNum);
        }
        else if (GameManager.instance.gamePlayNum == GameManager.instance.gameTotalSu)
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}
