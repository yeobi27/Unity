using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class Navi : MonoBehaviour
{
    public GameObject target;
    private Camera cam;
    private NavMeshAgent agent;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        SetInitialReferences();
        index = 0;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.transform.position);
    }

    private void Update()
    {
        if (TreeSlashGameManager.instance.gamePlay)
        {
            if (!GameObject.Find("Interactable").transform.Find("Target" + index).gameObject.activeSelf)
            {
                StopAllCoroutines();
                Resources.UnloadUnusedAssets();

                agent.isStopped = false;

                index++;
                agent.SetDestination(GameObject.Find("Interactable").transform.Find("Target" + index).gameObject.transform.position);  // 초기 첫나무 : Target0

                if (index > 0)
                {
                    TreeSlashSoundManager.Instance.PlaySE("Approaching");
                }

            }

            Vector3 dirToTarget = GameObject.Find("Interactable").transform.Find("Target" + index).transform.Find("CameraTarget" + index).gameObject.transform.position - this.transform.position;
            Vector3 look = Vector3.Slerp(this.transform.forward, dirToTarget.normalized, Time.deltaTime);

            this.transform.rotation = Quaternion.LookRotation(look, Vector3.up);

            cam.transform.LookAt(GameObject.Find("Interactable").transform.Find("Target" + index).transform.Find("CameraTarget" + index).gameObject.transform);
        }
    }
    private void SetInitialReferences()
    {
        cam = GetComponentInChildren<Camera>();

        if (!cam)
            cam = Camera.main;
    }
}
