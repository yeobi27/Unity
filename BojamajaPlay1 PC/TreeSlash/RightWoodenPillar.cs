using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightWoodenPillar : MonoBehaviour
{
    public Mesh[] meshes;

    private MeshFilter meshFilter;
    private int HitCount;   // 0 ~ 3
    private int meshesIndex;    // 0 ~ 11
    private GameObject hitParticle;
    public static int rightWoodmesheIndex;
    //public GameObject woodFragments;
    private void Start()
    {
        HitCount = 0;
        meshesIndex = 0;
        meshFilter = GetComponent<MeshFilter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TreeSlashGameManager.instance.gamePlay)
        {
            if (other.CompareTag("L_Hand"))
            {
                TreeSlashSoundManager.Instance.PlaySE("Hit" + Random.Range(1, 3));
                HitCount++;
                if (HitCount == 3)
                {
                    // 나무 파편 파티클
                    hitParticle = Instantiate(TreeSlashDataManager.instance.hitParticle, gameObject.transform);
                    hitParticle.transform.position = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);

                    HitCount = 0;
                    if (meshesIndex < 3)
                    {
                        meshFilter.sharedMesh = meshes[meshesIndex++];
                        rightWoodmesheIndex = meshesIndex;
                    }

                }
            }

            if (other.CompareTag("R_Hand"))
            {
                TreeSlashSoundManager.Instance.PlaySE("Hit" + Random.Range(1, 3));
                HitCount++;
                if (HitCount == 3)
                {
                    hitParticle = Instantiate(TreeSlashDataManager.instance.hitParticle, gameObject.transform);
                    hitParticle.transform.position = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);

                    HitCount = 0;
                    if (meshesIndex < 3)
                    {
                        meshFilter.sharedMesh = meshes[meshesIndex++];
                        rightWoodmesheIndex = meshesIndex;
                    }
                }
            }
        }

    }

    public void RightMeshfilter()
    {
        TreeSlashSoundManager.Instance.PlaySE("Hit" + Random.Range(1, 3));
        HitCount++;
        if (HitCount == 3)
        {
            //TreeSlashDataManager.instance.hitParticle
            hitParticle = Instantiate(TreeSlashDataManager.instance.hitParticle, gameObject.transform);
            hitParticle.transform.position = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);

            HitCount = 0;
            if (meshesIndex < 3)
            {
                meshFilter.sharedMesh = meshes[meshesIndex++];
                rightWoodmesheIndex = meshesIndex;
            }

        }
    }
}