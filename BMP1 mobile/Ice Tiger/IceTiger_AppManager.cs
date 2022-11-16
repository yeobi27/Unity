using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class IceTiger_AppManager : MonoBehaviour
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
    public static IceTiger_AppManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        IceTiger_SoundManager.Instance.PlayRandomBGM();

        GameStart();
    }
    
    private void OnEnable()
    {
        IceTiger_Timer.RoundEnd += GameEnd;
    }

    private void OnDisable()
    {
        IceTiger_Timer.RoundEnd -= GameEnd;
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
        yield return IceTiger_UIManager.Instance._UI_Start();

        yield return GameStartCount(startCountSetting);
        
        yield return IceTiger_DataManager.Instance._Data_Start();

        Debug.Log("RoundStart : " + RoundStart);
        if (RoundStart != null)
        {
            Debug.Log("RoundStart Invoke");
            RoundStart.Invoke();
        }
    }

    private IEnumerator _GameEnd()
    {
        yield return IceTiger_DataManager.Instance._Data_End();

        yield return IceTiger_UIManager.Instance._UI_End();

        //yield return _NextCount(roundEndCountSetting);
    }

    private IEnumerator GameStartCount(int countLeft)
    {
        WaitForSecondsRealtime ws = new WaitForSecondsRealtime(0.8f);

        IceTiger_SoundManager.Instance.PlaySE("CountDown");

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
    }

    public void SceneLoad()
    {
        IceTiger_DataManager.Instance.fireworks.SetActive(false);

        IceTiger_SoundManager.Instance.StopBGM();

        SceneManager.LoadScene("Main");
    }
}
