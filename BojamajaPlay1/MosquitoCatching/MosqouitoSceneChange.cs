using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MosqouitoSceneChange : MonoBehaviour
{
    public static MosqouitoSceneChange Instance { get; private set; }
    

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else Instance = this;
        
    }

    public void SceneChange()
    {
        StartCoroutine(NextSceneChange());
    }

    IEnumerator NextSceneChange()
    {
        GameManager.instance.gamePlayNum += 1;
        //Debug.Log(GameManager.instance.gamePlayNum + ":::" + GameManager.instance.gameTotalSu);

        if (GameManager.instance.gamePlayNum < GameManager.instance.gameTotalSu)
            GameManager.instance.SceneMove(GameManager.instance.gamePlayNum);
        else if (GameManager.instance.gamePlayNum == GameManager.instance.gameTotalSu)
            SceneManager.LoadScene("EndScene");

        yield return null;
    }
}
