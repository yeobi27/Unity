using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldDetector : MonoBehaviour
{
    public static FieldDetector Instance { get; private set; }
    public bool is_OnField;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        is_OnField = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BlackGoStone"))
        {
            is_OnField = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BlackGoStone"))
        {
            is_OnField = false;
        }
    }
}
