using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GrillingMeat_UIManager : MonoBehaviour
{
    int gameSu = 4;

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
    [Header("Lv Score Img")]
    public Image Level;
    private readonly FullScreenMode fullscreen;

    public static GrillingMeat_UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        // 16 : 9
        Screen.SetResolution(Screen.width, (Screen.width * 9) / 16, fullscreen);
    }

    public void SetScore(int points)
    {
        score.text = points.ToString();
    }

    // UI 시작
    public IEnumerator _UI_Start()
    {
        SetScore(0);

        timerObj.SetActive(true);
        scoreObj.SetActive(true);
        highscoreObj.SetActive(true);

        // Victory UI / Play Agin False
        endScreen.SetActive(false);
        fantasticPan.SetActive(false);
        failScreen.SetActive(false);
        // use leapmotion
        // HomeBtnCanvas.SetActive(false);
        // use mobile
        HomeBtn.SetActive(false);

        yield return null;
    }
    // UI end : restart ? menu 
    public IEnumerator _UI_End()
    {
        timeChage = time.text;
        timeChage = timeChage.Replace(":", ".");

        //PlayerPrefs.SetString("GrillingMeatTime", time.text);
        PlayerPrefs.SetFloat("GrillingMeatTime", float.Parse(timeChage));
        PlayerPrefs.SetString("GrillingMeatScore", score.text);
        
        timerObj.SetActive(false);
        scoreObj.SetActive(false);
        highscoreObj.SetActive(false);

        if (GrillingMeat_DataManager.Instance.WonRound())
        {
            PlayerPrefs.SetString("GrillingMeatState", "Success");

            // clear
            endScreen.SetActive(true);
            //HomeBtnCanvas.SetActive(true);

            // change score's sprite
            if (float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) < 30 && float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) >= 25)
            {
                GrillingMeat_DataManager.Instance.SuccessTime.text = (30f- float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text)).ToString().Replace(".", ":");// + "\"";
                //Medal.sprite = Resources.Load("Textures/Level/Gold", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Fantastic", typeof(Sprite)) as Sprite;
                fantasticPan.SetActive(true);
                level = "Fantastic";
            }
            else if (float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) < 25 && float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) >= 20)
            {
                GrillingMeat_DataManager.Instance.SuccessTime.text = (30f - float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text)).ToString().Replace(".", ":");// + "\"";
                //Medal.sprite = Resources.Load("Textures/Level/Gold", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Fantastic", typeof(Sprite)) as Sprite;
                fantasticPan.SetActive(true);
                level = "Fantastic";
            }
            else if (float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) < 20 && float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) >= 15)
            {
                GrillingMeat_DataManager.Instance.SuccessTime.text = (30f - float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text)).ToString().Replace(".", ":");// + "\"";
                //Medal.sprite = Resources.Load("Textures/Level/Sliver", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Excellent", typeof(Sprite)) as Sprite;
                level = "Excellent";
            }
            else if (float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) < 15 && float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) >= 10)
            {
                GrillingMeat_DataManager.Instance.SuccessTime.text = (30f - float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text)).ToString().Replace(".", ":");// + "\"";
                //Medal.sprite = Resources.Load("Textures/Level/Sliver", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Amazing", typeof(Sprite)) as Sprite;
                level = "Awesome";
            }
            else if (float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) < 10 && float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) >= 5)
            {
                GrillingMeat_DataManager.Instance.SuccessTime.text = (30f - float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text)).ToString().Replace(".", ":");// + "\"";
                //Medal.sprite = Resources.Load("Textures/Level/Dong", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Great", typeof(Sprite)) as Sprite;
                level = "Great";
            }
            else if (float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) < 5 && float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) > 0)
            {
                GrillingMeat_DataManager.Instance.SuccessTime.text = (30f - float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text)).ToString().Replace(".", ":");// + "\"";
                //Medal.sprite = Resources.Load("Textures/Level/Dong", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Good", typeof(Sprite)) as Sprite;
                level = "Good";
            }

            PlayerPrefs.SetString("GrillingMeatLevel", level);

            HomeBtn.SetActive(true);

            StartCoroutine(NextSceneChange());
        }
        else
        {
            PlayerPrefs.SetString("GrillingMeatState", "Failure");  // Whether the game is successful or not
            // restart
            failScreen.SetActive(true);
            //HomeBtnCanvas.SetActive(true);
            HomeBtn.SetActive(true);

            StartCoroutine(NextSceneChange());  // next game
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
