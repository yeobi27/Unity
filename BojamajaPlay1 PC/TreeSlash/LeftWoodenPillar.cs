using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftWoodenPillar : MonoBehaviour
{
    public Mesh[] meshes;

    private MeshFilter meshFilter;
    private int HitCount;   // 0 ~ 3
    private int meshesIndex;    // 0 ~ 11
    private GameObject hitParticle;
    public static int leftWoodmesheIndex;

    private void Start()
    {
        HitCount = 0;
        meshesIndex = 0;
        leftWoodmesheIndex = 0;
        meshFilter = GetComponent<MeshFilter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TreeSlashGameManager.instance.gamePlay)
        {
            // 3번 마다 Mesh Filter 바뀜
            if (other.CompareTag("L_Hand"))
            {
                TreeSlashSoundManager.Instance.PlaySE("Hit" + Random.Range(1, 3));
                HitCount++;
                if (HitCount == 3)
                {
                    hitParticle = Instantiate(TreeSlashDataManager.instance.hitParticle, gameObject.transform);
                    hitParticle.transform.position = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);

                    HitCount = 0;

                    if (meshesIndex < 3)
                    {
                        meshFilter.sharedMesh = meshes[meshesIndex++];
                        leftWoodmesheIndex = meshesIndex;
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
                    hitParticle.transform.position = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);

                    HitCount = 0;
                    if (meshesIndex < 3)
                    {
                        meshFilter.sharedMesh = meshes[meshesIndex++];
                        leftWoodmesheIndex = meshesIndex;
                    }
                }
            }
        }

    }

    public void LeftMeshfilter()
    {
        TreeSlashSoundManager.Instance.PlaySE("Hit" + Random.Range(1, 3));
        HitCount++;
        if (HitCount == 3)
        {
            hitParticle = Instantiate(TreeSlashDataManager.instance.hitParticle, gameObject.transform);
            hitParticle.transform.position = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);

            HitCount = 0;
            if (meshesIndex < 3)
            {
                meshFilter.sharedMesh = meshes[meshesIndex++];
                leftWoodmesheIndex = meshesIndex;
            }
        }
    }
}