using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterWoodenPillar : MonoBehaviour
{
    public GameObject leftWood;
    public GameObject rightWood;

    private RightWoodenPillar rightWoodenPillar;
    private LeftWoodenPillar leftWoodenPillar;

    private bool randBool;

    void Start()
    {
        rightWoodenPillar = rightWood.GetComponent<RightWoodenPillar>();
        leftWoodenPillar = leftWood.GetComponent<LeftWoodenPillar>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TreeSlashGameManager.instance.gamePlay)
        {
            if (other.CompareTag("L_Hand"))
            {
                randBool = (Random.value > 0.5f);
                if (randBool)
                {
                    rightWoodenPillar.RightMeshfilter();
                }
                else
                {
                    leftWoodenPillar.LeftMeshfilter();
                }
            }

            if (other.CompareTag("R_Hand"))
            {
                randBool = (Random.value > 0.5f);
                if (randBool)
                {
                    rightWoodenPillar.RightMeshfilter();
                }
                else
                {
                    leftWoodenPillar.LeftMeshfilter();
                }
            }
        }
    }
}
