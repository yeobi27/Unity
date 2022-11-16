using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CatchPang_AppManager : MonoBehaviour
{
    private EnemySpawn enemySpawner;
    private int startCountSetting = 4;
    private int roundEndCountSetting = 5;
    public Image[] startCount;
    public GameObject tutorial;
    //public Text nextCount;

    public static CatchPang_AppManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else Instance = this;
    }

    void Start()
    {
        CatchPang_SoundManager.Instance.PlayRandomBGM();

        enemySpawner = FindObjectOfType<EnemySpawn>();

        OnRoundStart();  // 바로시작
    }
    
    void OnEnable()
    {
        CatchPang_Timer.RoundEnd += OnRoundEnd;
        //Debug.Log("Timer.RoundEnd OnEnable : " + CatchPang_Timer.RoundEnd);
    }

    void OnDisable()
    {
        CatchPang_Timer.RoundEnd -= OnRoundEnd;
        //Debug.Log("Timer.RoundEnd OnDisable : " + CatchPang_Timer.RoundEnd);
    }

    void OnRoundEnd()
    {
       StopAllCoroutines();
       StartCoroutine(_OnRoundEnd());
    }

    // 시작버튼
    public void OnRoundStart()
    {
        StopAllCoroutines();
        // 실행
        StartCoroutine(_OnRoundStart());
    }
    private IEnumerator _OnRoundStart()
    {
        yield return CatchPang_UIManager.Instance.OnRoundStart();

        yield return GameStartCount(startCountSetting);

        yield return CatchPang_DataManager.Instance.OnRoundStart();

        enemySpawner.StartSpawner();
    }

    private IEnumerator _OnRoundEnd()
    {
        yield return CatchPang_DataManager.Instance.OnRoundEnd();

        yield return CatchPang_UIManager.Instance.OnRoundEnd();

        enemySpawner.OnRoundEnd();
    }

    private IEnumerator GameStartCount(int countLeft)
    {
        WaitForSecondsRealtime ws = new WaitForSecondsRealtime(0.8f);

        CatchPang_SoundManager.Instance.PlaySE("CountDown");

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
        CatchPang_DataManager.Instance.clearParticles.SetActive(false);

        CatchPang_SoundManager.Instance.StopBGM();

        SceneManager.LoadScene("Main");
    }
}