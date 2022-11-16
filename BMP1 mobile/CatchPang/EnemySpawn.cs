using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Transform northPoint;
    public Transform southPoint;
    public Transform eastPoint;
    public Transform westPoint;

    public GameObject[] cuteAnimals;
    private List<GameObject> animalPool;


    void Start()
    {
        animalPool = new List<GameObject>();

        StartSpawner();
    }

    public void StartSpawner()
    {
        StopAllCoroutines();
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        GameObject go;
        Vector3 randPos;

        while (CatchPang_DataManager.Instance.levelTimer.timeLeft > 0)
        {
            go = Instantiate(cuteAnimals[Random.Range(0, cuteAnimals.Length - 1)]);
            randPos.x = Random.Range(westPoint.position.x, eastPoint.position.x);
            randPos.z = Random.Range(southPoint.position.z, northPoint.position.z);
            randPos.y = 0f;

            animalPool.Add(go);
            
            go.transform.SetParent(this.transform);
            go.transform.position = randPos;
                        
            yield return new WaitForSecondsRealtime(1f);
        }

        yield return null;
    }

    public void OnRoundEnd()
    {
        foreach (var v in animalPool)
        {
            Destroy(v);
        }
    }
}
