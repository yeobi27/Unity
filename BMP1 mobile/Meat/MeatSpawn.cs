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

    //[Header("Point Information")]
    //[SerializeField] Transform[] fixedPoint;

    // 삭제하기위한 Pooling
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
            // 위치에 있는지 없는지 체크

            for (int i = 0; i < meatFromPoints.Length; i++)
            {
                if (meatFromPoints[i].meat == null)
                {
                    //points[i].hasExist = true;
                    // 고기를 랜덤으로 생성해서 올림
                    go = Instantiate(meats[Random.Range(0, meats.Length - 1)], this.transform.GetChild(i));

                    // Meat.cs 에 조건문에 쓰이기 위해서 (Clone) 떼줌.
                    go.name = go.name.Replace("(Clone)", "");

                    // 클래스 내 오브젝트 넣기
                    meatFromPoints[i].meat = go;

                    // 한번에 지우기위함
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
            Debug.Log("없앤다.");
            Destroy(v);
        }
    }
}
