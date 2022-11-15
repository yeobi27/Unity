using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteGoStoneSpawn : MonoBehaviour
{
    public GameObject goStone;
    public int StoneAmount;
    public List<GameObject> goStonePool;
    private new BoxCollider collider;
    private bool is_OnCheckWhiteGoStone;

    public static WhiteGoStoneSpawn Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        goStonePool = new List<GameObject>();
        collider = GetComponent<BoxCollider>();
        is_OnCheckWhiteGoStone = false;
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
	    // If there is no white stone
            if (goStonePool.Count == 0)
            {
                CreateGoStone();
            }

            yield return new WaitForSecondsRealtime(0.4f);
        }

        yield return null;
    }

    public void CreateGoStone()
    {
        // if black stone exists
        if (!is_OnCheckWhiteGoStone)
        {
            GameObject go;
            Vector3 randPos;

            // 15
            for (int i = 0; i < StoneAmount; i++)
            {
                go = Instantiate(goStone);

                goStonePool.Add(go);

                go.transform.SetParent(this.transform);
                randPos = GetRandomPointInCollider();
                go.transform.position = randPos;
            }
        }
    }

    // get random Spawn
    public Vector3 GetRandomPointInCollider()
    {
        Bounds bounds = collider.bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    public void OnRoundEnd()
    {
        foreach (var v in goStonePool)
        {
            Destroy(v);
            Resources.UnloadUnusedAssets();
        }
        goStonePool.Clear();
    }

}
