using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoGameManager : MonoBehaviour
{
    public static GoGameManager instance { get; private set; }

    public GameObject countdownPanel;
    public bool gamePlay = false;


    void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    void Start()
    {
        //System.GC.Collect();
        Resources.UnloadUnusedAssets();
        GamePlayStart();
        countdownPanel.SetActive(true);
    }

    void OnEnable()
    {
        GoTimer.RoundEnd += GamePlayEnd;
    }

    void OnDisable()
    {
        GoTimer.RoundEnd -= GamePlayEnd;
    }

    void GamePlayEnd()
    {
        StopAllCoroutines();
        Resources.UnloadUnusedAssets();
        StartCoroutine(_GameEnd());
    }


    public void GamePlayStart()
    {
        StopAllCoroutines();
        Resources.UnloadUnusedAssets();
        StartCoroutine(_GameStart());
    }

    private IEnumerator _GameStart()
    {
        
        yield return GoUIManager.instance.GameStart();
        GoSoundManager.Instance.PlaySE("CountDown");
        yield return new WaitForSeconds(4f);

        gamePlay = true;
        yield return GoDataManager.instance.GameStart();
        
    }

    private IEnumerator _GameEnd()
    {
        gamePlay = false;

        yield return GoDataManager.instance.GameEnd();
        yield return GoUIManager.instance.GameEnd();

        yield return null;
    }

    public void HomeBtnOnClick()
    {
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene("Main");
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }
}
