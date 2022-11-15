using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackGoStoneSpawn : MonoBehaviour
{
    public Transform goStoneSpawnPos;

    public GameObject goStone;
    private List<GameObject> goStonePool;
#pragma warning disable IDE0052
    private bool is_InCollider;
#pragma warning restore IDE0052

    // Start is called before the first frame update
    void Start()
    {
        //is_InCollider = false;
        goStonePool = new List<GameObject>();
    }

    public void StartSpawner()
    {
        StopAllCoroutines();
        Resources.UnloadUnusedAssets();
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while (GoDataManager.instance.playTime.timeLeft > 0)
        {
	     // If the black stone does not exist on the 1. table, it is created
             // 2. If not inside the Detector
             // 3. If not in the fieldbox
            if (!CheckOnGoStone.Instance.is_OntheTable && /*!CheckOnGoStone.Instance.is_InCollider &&*/ !FieldDetector.Instance.is_OnField)
            {
		// Create after checking if there is a hand in the black stone sponge area: If there is a hand,
		// RED will not collide, if there is no hand, GREEN will not collide
                if (HandsDetector.Instance.is_HandsOnField)
                {
                    CreateGoStone("red");
                }
                else
                {
                    CreateGoStone("green");
                }
            }

            yield return new WaitForSecondsRealtime(1.2f);
        }

        yield return null;
    }

    public void CreateGoStone(string outline)
    {
        if (outline.Equals("red"))
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Hands"), LayerMask.NameToLayer("BlackGoStones"), true);

            GameObject go;

            go = Instantiate(goStone);

            goStonePool.Add(go);

            go.transform.SetParent(this.transform);
            go.transform.position = goStoneSpawnPos.position;
        }
        else if (outline.Equals("green"))
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Hands"), LayerMask.NameToLayer("BlackGoStones"), false);

            GameObject go;

            go = Instantiate(goStone);

            goStonePool.Add(go);

            go.transform.SetParent(this.transform);
            go.transform.position = goStoneSpawnPos.position;
        }
    }

    // check if a Go exists : true
    public void DetectingOnDistance()
    {
        is_InCollider = true;
    }

    // check if a Go exists : false
    public void DetectingOffDistance()
    {
        is_InCollider = false;
    }

    public void OnRoundEnd()
    {
        foreach(var stone in goStonePool)
        {
            Destroy(stone);
            Resources.UnloadUnusedAssets();
        }

        goStonePool.Clear();
    }

}
