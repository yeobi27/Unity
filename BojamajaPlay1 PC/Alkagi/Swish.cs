using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swish : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("L_Hand"))
        {
            TreeSlashSoundManager.Instance.PlaySE("swish" + Random.Range(1, 3));
        }

        if (other.CompareTag("R_Hand"))
        {
            TreeSlashSoundManager.Instance.PlaySE("swish" + Random.Range(1, 3));
        }
    }
}
