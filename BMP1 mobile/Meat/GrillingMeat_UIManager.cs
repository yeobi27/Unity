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
    //public GameObject HomeBtnCanvas;

    [Header("Lv Score Img")]
    //public Image Medal;
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
        Screen.SetResolution(Screen.width, (Screen.width * 9) / 16, fullscreen);

        //Debug.Log("Screen.width : " + Screen.width + ":::: (Screen.width*9) / 16 : " + (Screen.width * 9) / 16);
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
        // 홈버튼 립모션용
        // HomeBtnCanvas.SetActive(false);
        // 홈버튼 모바일
        HomeBtn.SetActive(false);

        Debug.Log("UIManager Start");

        yield return null;
    }
    // UI 끝 : 재시작 ? 메뉴 
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

            endScreen.SetActive(true);
            //HomeBtnCanvas.SetActive(true);

            // 메달, 영어 Image 변경
            if (float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) < 30 && float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) >= 25)
            {
                GrillingMeat_DataManager.Instance.SuccessTime.text = (30f- float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text)).ToString("N2").Replace(".", ":");// + "\"";
                //Medal.sprite = Resources.Load("Textures/Level/Gold", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Fantastic", typeof(Sprite)) as Sprite;
                fantasticPan.SetActive(true);
                level = "Fantastic";
            }
            else if (float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) < 25 && float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) >= 20)
            {
                GrillingMeat_DataManager.Instance.SuccessTime.text = (30f - float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text)).ToString("N2").Replace(".", ":");// + "\"";
                //Medal.sprite = Resources.Load("Textures/Level/Gold", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Fantastic", typeof(Sprite)) as Sprite;
                fantasticPan.SetActive(true);
                level = "Fantastic";
            }
            else if (float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) < 20 && float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) >= 15)
            {
                GrillingMeat_DataManager.Instance.SuccessTime.text = (30f - float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text)).ToString("N2").Replace(".", ":");// + "\"";
                //Medal.sprite = Resources.Load("Textures/Level/Sliver", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Excellent", typeof(Sprite)) as Sprite;
                level = "Excellent";
            }
            else if (float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) < 15 && float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) >= 10)
            {
                GrillingMeat_DataManager.Instance.SuccessTime.text = (30f - float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text)).ToString("N2").Replace(".", ":");// + "\"";
                //Medal.sprite = Resources.Load("Textures/Level/Sliver", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Amazing", typeof(Sprite)) as Sprite;
                level = "Awesome";
            }
            else if (float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) < 10 && float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) >= 5)
            {
                GrillingMeat_DataManager.Instance.SuccessTime.text = (30f - float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text)).ToString("N2").Replace(".", ":");// + "\"";
                //Medal.sprite = Resources.Load("Textures/Level/Dong", typeof(Sprite)) as Sprite;
                Level.sprite = Resources.Load("Textures/Level/Great", typeof(Sprite)) as Sprite;
                level = "Great";
            }
            else if (float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) < 5 && float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text) > 0)
            {
                GrillingMeat_DataManager.Instance.SuccessTime.text = (30f - float.Parse(GrillingMeat_DataManager.Instance.SuccessTime.text)).ToString("N2").Replace(".", ":");// + "\"";
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
            PlayerPrefs.SetString("GrillingMeatState", "Failure");
            failScreen.SetActive(true);
            //HomeBtnCanvas.SetActive(true);
            HomeBtn.SetActive(true);

            StartCoroutine(NextSceneChange());
        }

        yield return null;
    }

    //게임끝나고 다음 게임으로 가는 함수
    IEnumerator NextSceneChange()
    {
        yield return new WaitForSeconds(5f);
        GameManager.instance.gamePlayNum += 1;

        // 모래시계가 없다면 홈으로 씬 전환
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
