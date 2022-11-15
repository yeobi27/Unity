using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mosquito
{
    public class InsectSpawner : MonoBehaviour
    {
        private new BoxCollider collider;
        private List<GameObject> pool;

        public int minPerBatch = 1;
        public int maxPerBatch = 5;
        public float minTimeBetweenSpawns = 0.5f;
        public float maxTimeBetweenSpawns = 2f;
        public GameObject[] toSpawn;


        void Start()
        {
            collider = GetComponent<BoxCollider>();
            pool = new List<GameObject>();
            maxPerBatch += 1;
        }

        public void StartSpawner()
        {
            StopCoroutine("Spawner");
            Resources.UnloadUnusedAssets();
            StartCoroutine("Spawner");
        }

        IEnumerator Spawner()
        {
            GameObject obj;
            Vector3 randPos;
            int batchCount;

            while (DataManager.Instance.timerManager.timeLeft > 0)
            {
                batchCount = Random.Range(minPerBatch, maxPerBatch);

                yield return new WaitForSeconds(minTimeBetweenSpawns + (batchCount * 0.2f));

                if (pool.Count > 15f) yield return null;

                for (int i = 0; i < batchCount; i++)
                {
                    randPos = GetRandomPointInCollider();

                    obj = Instantiate(toSpawn[Random.Range(0, toSpawn.Length)], randPos, Quaternion.Euler(0f, Random.Range(0f, 359f), 0f), this.transform);

                    pool.Add(obj);
                }
            }

            yield return null;
        }

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
            StopCoroutine("Spawner");
            foreach (var obj in pool)
            {
                Destroy(obj);
                Resources.UnloadUnusedAssets();
            }
            pool.Clear();
        }
    }
}
