using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Wood : MonoBehaviour
{
    public GameObject woodObj;
    public GameObject brokenWood;
    //private bool gamePlayShoot = true;
    private static int Index = 1;
    // Start is called before the first frame update
    void Start()
    {
        Index = 0;
    }

    private void Update()
    {
        if (TreeSlashGameManager.instance.gamePlay)
        {
            if (RightWoodenPillar.rightWoodmesheIndex == 3 && LeftWoodenPillar.leftWoodmesheIndex == 3 && this.gameObject.name == "Wood"+ Index)
            {
                RightWoodenPillar.rightWoodmesheIndex = 0;
                LeftWoodenPillar.leftWoodmesheIndex = 0;
                Index++;
                woodObj.SetActive(false);
                brokenWood.SetActive(true);

                TreeSlashDataManager.instance.AddScore(500);
                TreeSlashSoundManager.Instance.PlaySE("GetScore");
                TreeSlashSoundManager.Instance.PlaySE("FelledTree");

                StartCoroutine(_Destroy());
            }
        }
    }

    IEnumerator _Destroy()
    {
        WaitForSeconds ws = new WaitForSeconds(0.8f);

        yield return ws;

        Resources.UnloadUnusedAssets();
        //Destroy(gameObject);
        GameObject parentObj = transform.parent.gameObject;
        parentObj.transform.GetChild(1).gameObject.SetActive(false);
        parentObj.SetActive(false);

        yield return null;
    }

    public void WoodStart()
    {
        StopAllCoroutines();
        Resources.UnloadUnusedAssets();
        StartCoroutine(WoodState());
    }

    IEnumerator WoodState()
    {
        WaitForSeconds ws = new WaitForSeconds(0.5f);

        while (TreeSlashDataManager.instance.playTime.timeLeft > 0f)
        {
            if (RightWoodenPillar.rightWoodmesheIndex == 3 && LeftWoodenPillar.leftWoodmesheIndex == 3)
            {
                yield return ws;

                woodObj.SetActive(false);
                brokenWood.SetActive(true);
            }
        }
    }

    public void WoodEnd()
    {
        StopAllCoroutines();
        Resources.UnloadUnusedAssets();
    }
}
