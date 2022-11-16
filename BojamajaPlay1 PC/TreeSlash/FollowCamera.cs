using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;        
    private Transform tr;                
    private Vector3 targetPosition;
    private int index;

    void Start()
    {
        index = 0;
        tr = GetComponent<Transform>();
    }

    void LateUpdate()
    {
        tr.position = new Vector3(target.position.x, tr.position.y, target.position.z - 1.7f);

        if (!GameObject.Find("Interactable").transform.Find("Target" + index).transform.Find("CameraTarget" + index).gameObject.activeSelf)
        {
            index++;
        }
        tr.LookAt(GameObject.Find("Interactable").transform.Find("Target" + index).transform.Find("CameraTarget" + index).gameObject.transform);
    }
}