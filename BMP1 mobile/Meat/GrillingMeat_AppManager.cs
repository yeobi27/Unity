using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GrillingMeat_AppManager : MonoBehaviour
{
    //private IceTiger_Action iceTiger_Action;
    private int startCountSetting = 4;
    private int roundEndCountSetting = 5;
    public Image[] startCount;
    public GameObject tutorial;
    //public Image[] nextCount;
    //public Text nextCount;

    //public static bool isPlaying;
    public static UnityAction RoundStart = null;
    public static GrillingMeat_AppManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        GrillingMeat_SoundManager.Instance.PlayMainBGM();

        GameStart();
    }

    private void OnEnable()
    {
        GrillingMeat_Timer.RoundEnd += GameEnd;
    }

    private void OnDisable()
    {
        GrillingMeat_Timer.RoundEnd -= GameEnd;
    }

    private void GameEnd()
    {
        StopAllCoroutines();
        StartCoroutine(_GameEnd());
    }

    public void GameStart()
    {
        StopAllCoroutines();
        StartCoroutine(_GameStart());
    }

    private IEnumerator _GameStart()
    {
        yield return GrillingMeat_UIManager.Instance._UI_Start();

        yield return GameStartCount(startCountSetting);

        yield return GrillingMeat_DataManager.Instance._Data_Start();
        
        if (RoundStart != null)
        {
            Debug.Log("RoundStart Invoke");
            RoundStart.Invoke();
        }
    }

    private IEnumerator _GameEnd()
    {
        yield return GrillingMeat_DataManager.Instance._Data_End();

        yield return GrillingMeat_UIManager.Instance._UI_End();
    }

    private IEnumerator GameStartCount(int countLeft)
    {
        WaitForSecondsRealtime ws = new WaitForSecondsRealtime(0.8f);

        // 3,2,1
        GrillingMeat_SoundManager.Instance.PlaySE("CountDown");

        while (countLeft > 0)
        {
            countLeft -= 1;
            startCount[countLeft].gameObject.SetActive(true);
            if (countLeft < 3)
                tutorial.SetActive(true);

            yield return ws;
            startCount[countLeft].gameObject.SetActive(false);
        }
        tutorial.SetActive(false);
        GrillingMeat_SoundManager.Instance.PlayGrillingBGM();
    }

    public void SceneLoad()
    {
        GrillingMeat_SoundManager.Instance.StopMainBGM();
        GrillingMeat_SoundManager.Instance.StopGrillingBGM();
        GrillingMeat_SoundManager.Instance.StopSfx();

        //AdManager.Instance.ToggleAd(true);

        SceneManager.LoadScene("Main");
    }
}
