using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpawn : MonoBehaviour
{
    public static WoodSpawn Instance { get; private set; }

    public GameObject wood;
    public int instantiateCount;
    public Material[] woodMaterials;
    private List<GameObject> woodPool;
    private new BoxCollider collider;

    private int woodIndex;
    private float z_plusBound;
    private int randTreeColor;
    private int randLeaf;
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
        randTreeColor = 0;
        randLeaf = 0;
        woodPool = new List<GameObject>();
        collider = GetComponent<BoxCollider>();
        woodIndex = 1;
        z_plusBound = 0;
    }

    public void StartSpawn()
    {
        StopAllCoroutines();
        Resources.UnloadUnusedAssets();
        StartCoroutine(_Spawn());
    }

    IEnumerator _Spawn()
    {
        GameObject go;
        Vector3 randPos;

        for (int i = 0; i < instantiateCount; i++)
        {
            go = Instantiate(wood);

            go.name = "Target" + woodIndex;

            go.transform.GetChild(0).name = "Wood" + woodIndex;

            randTreeColor = Random.Range(2, 4); // 2,3
            randLeaf = Random.Range(0, 2);  // 0,1

            for (int j = 4; j <= 5; j++)
            {
                go.transform.Find("Wood" + woodIndex).transform.GetChild(0).gameObject.transform.GetChild(j).GetComponent<MeshRenderer>().material = woodMaterials[randTreeColor];
            }

            for (int x = 0; x <= 3; x++)
            {
                go.transform.Find("Wood" + woodIndex).transform.GetChild(0).gameObject.transform.GetChild(x).GetComponent<MeshRenderer>().material = woodMaterials[randLeaf];
            }

            // 애니메이션 들어간 나무에 materials 바꿔서 생성해줘야함
            for (int y = 0; y <= 1; y++)
            {
                go.transform.Find("Wood" + woodIndex).transform.GetChild(1).gameObject.transform.GetChild(y).GetComponent<MeshRenderer>().material = woodMaterials[randTreeColor];
            }

            go.transform.GetChild(1).name = "CameraTarget" + woodIndex;

            woodPool.Add(go);

            go.transform.SetParent(this.transform);
            randPos = GetRandomPointInCollider();   

            z_plusBound += Random.Range(4,6); 

            go.transform.position = randPos;

            woodIndex++;
        }

        //}

        yield return null;
    }

    // 스폰위치 랜덤값 받기
    public Vector3 GetRandomPointInCollider()
    {
        // z 는 위치 일정하게 바뀜
        // bounds.min.z 초기 위치
        Bounds bounds = collider.bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.min.y),
            //Random.Range(bounds.min.z, bounds.max.z)
            bounds.min.z + z_plusBound
        );
    }

    public void OnRoundEnd()
    {
        foreach (var v in woodPool)
        {
            Destroy(v);
            Resources.UnloadUnusedAssets();
        }
        woodPool.Clear();
    }
}
