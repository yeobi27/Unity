using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeatFromPoint
{
    public string pointName;
    public Transform point;
    public GameObject meat;

    public MeatFromPoint(string pointName, Transform point, GameObject meat)
    {
        this.pointName = pointName;
        this.point = point;
        this.meat = meat;
    }
}

public class MeatSpawn : MonoBehaviour
{
    [Header("CheckPoint Information")]
    [SerializeField] MeatFromPoint[] meatFromPoints;

    [Header("Meat Information")]
    [SerializeField] GameObject[] meats;

    // Pooling for delete
    private List<GameObject> meatsPool;

    private void OnEnable()
    {
        GrillingMeat_AppManager.RoundStart += StartSpawner;
        GrillingMeat_Timer.RoundEnd += OnRoundEnd;
    }

    private void OnDisable()
    {
        GrillingMeat_AppManager.RoundStart -= StartSpawner;
        GrillingMeat_Timer.RoundEnd -= OnRoundEnd;
    }

    void Start()
    {
        meatsPool = new List<GameObject>();
    }

    public void StartSpawner()
    {
        StopAllCoroutines();
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        GameObject go;
        
        while (GrillingMeat_DataManager.Instance.timer.timeLeft > 0)
        {
            for (int i = 0; i < meatFromPoints.Length; i++)
            {
                if (meatFromPoints[i].meat == null)
                {
                    //points[i].hasExist = true;
                    go = Instantiate(meats[Random.Range(0, meats.Length - 1)], this.transform.GetChild(i));

                    // Remove (Clone) to be used in conditional statements in Meat.cs.
                    go.name = go.name.Replace("(Clone)", "");

                    meatFromPoints[i].meat = go;

                    meatsPool.Add(go);

                    //go.transform.SetParent(this.transform);
                    go.transform.position = meatFromPoints[i].point.position;
                }
            }

            yield return new WaitForSecondsRealtime(1f);
        }

        yield return null;
    }

    public void OnRoundEnd()
    {
        foreach (var v in meatsPool)
        {
            Destroy(v);
        }
    }
}
