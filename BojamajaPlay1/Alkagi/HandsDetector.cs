using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsDetector : MonoBehaviour
{
    public static HandsDetector Instance { get; private set; }
    public bool is_HandsOnField;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }
    public void HandsFieldOnActivate()
    {
        // If there is a hand + go stone, a red outline
        is_HandsOnField = true;
    }

    public void HandsFieldOnDeActivate()
    {
        is_HandsOnField = false;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Hands"), LayerMask.NameToLayer("BlackGoStones"), false);
    }
}
