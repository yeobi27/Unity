using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillingMeat_AutoSelfSetActive : MonoBehaviour
{
    public float selfDestructInSeconds;

    private void OnEnable()
    {
        Invoke("AutoSelfSetActive", selfDestructInSeconds);
    }

    void AutoSelfSetActive()
    {
        gameObject.SetActive(false);
    }
}
