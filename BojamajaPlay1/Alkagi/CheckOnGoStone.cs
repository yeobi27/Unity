using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOnGoStone : MonoBehaviour
{
    public static CheckOnGoStone Instance { get; private set; }

    public bool is_OntheTable;  // Determine if black stone is present or not
    public bool is_InCollider;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        is_InCollider = false;
        is_OntheTable = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("BlackGoStone"))
        {
            is_OntheTable = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("BlackGoStone"))
        {
            is_OntheTable = false;
        }
    }

    public void DetectingOnDistance()
    {
        is_InCollider = true;
    }

    public void DetectingOffDistance()
    {
        is_InCollider = false;
    }
}
