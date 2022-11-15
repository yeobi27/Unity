using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BlackGoStone : MonoBehaviour
{
    Vector3 currentGoPos;
    Vector3 lastGoPos;

    public float distance;
    
    // Start is called before the first frame update
    void Start()
    {
        StopAllCoroutines();
        Resources.UnloadUnusedAssets();
        StartCoroutine(GoStone());
    }

    IEnumerator GoStone()
    {
        while (GoDataManager.instance.playTime.timeLeft > 0)
        {
            currentGoPos = new Vector3(this.transform.position.x, transform.position.y, transform.position.z);

            yield return new WaitForSeconds(0.1f);

            distance = Vector3.Distance(lastGoPos, currentGoPos);
            
            if (currentGoPos != lastGoPos)
            {
                lastGoPos = currentGoPos;
            }

            // set on the table, the value stays
            if (CheckOnGoStone.Instance.is_OntheTable && !CheckOnGoStone.Instance.is_InCollider && distance < 0.001f)
            {
                CheckOnGoStone.Instance.is_OntheTable = false;
                FieldDetector.Instance.is_OnField = false;

                DestroyStone();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Hands"))
        {
            GoSoundManager.Instance.PlaySE("BlackStoneHitbyHand");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Floor"))
        {
            GoSoundManager.Instance.PlaySE("BumpIntoStone");
            DestroyStone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FieldDetector"))
        {
            GoSoundManager.Instance.PlaySE("GoStoneIsFalling");
        }
    }

    public void DestroyStone()
    {
        Destroy(gameObject);
        Resources.UnloadUnusedAssets();
    }
}
