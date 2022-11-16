using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TreeSlashGameManager : MonoBehaviour
{
    //public static UnityAction RoundStart = null;
    public static TreeSlashGameManager instance { get; private set; }

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
        TreeSlashTimer.RoundEnd += GamePlayEnd;
        //Debug.Log("Timer.RoundEnd OnEnable : " + Timer.RoundEnd);
    }

    void OnDisable()
    {
        TreeSlashTimer.RoundEnd -= GamePlayEnd;
        //Debug.Log("Timer.RoundEnd OnDisable : " + Timer.RoundEnd);
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
        yield return TreeSlashUIManager.instance.GameStart();
        TreeSlashSoundManager.Instance.PlaySE("Countdown");
        yield return new WaitForSeconds(4f);

        gamePlay = true;
        yield return TreeSlashDataManager.instance.GameStart();
        
        WoodSpawn.Instance.StartSpawn();
    }

    private IEnumerator _GameEnd()
    {
        gamePlay = false;

        yield return TreeSlashDataManager.instance.GameEnd();
        yield return TreeSlashUIManager.instance.GameEnd();
        WoodSpawn.Instance.OnRoundEnd();
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
