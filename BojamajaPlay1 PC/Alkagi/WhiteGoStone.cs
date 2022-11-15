using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteGoStone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Floor"))
        {
            GoDataManager.instance.AddScore(500);
            GoSoundManager.Instance.PlaySE("GetScore");

            WhiteGoStoneSpawn.Instance.goStonePool.Remove(gameObject);

            Destroy(gameObject);
            Resources.UnloadUnusedAssets();
        }
        else if (collision.collider.gameObject.CompareTag("BlackGoStone"))
        {
            GoSoundManager.Instance.PlaySE("BumpIntoStone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FieldDetector"))
        {
            GoSoundManager.Instance.PlaySE("GoStoneIsFalling");
        }
    }
}
