using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTiger_PlayerContoller : MonoBehaviour
{
    private Camera cam;
    private Vector3 rayPointPos;
    public LayerMask layerMask;
    //public float hitDist;

    public static IceTiger_PlayerContoller Instance { get; private set; }
    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else Instance = this;
    }
    void Start()
    {
        SetInitialReferences();
    }

    void Update()
    {
        //GameObject hitParticle;
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 15f, layerMask))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("IceTigers"))
            {
                if (hit.collider.CompareTag("IceTiger"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        hit.transform.gameObject.GetComponent<IceTiger>().OnDown();
                    }
                }
            }
        }

    }

    //void OnGUI()
    //{
    //    //Debug.DrawRay(cam.transform.position, rayPointPos - cam.transform.position, Color.magenta, 0.2f);
    //}

    void SetInitialReferences()
    {
        cam = GetComponentInChildren<Camera>();
        if (!cam)
            cam = Camera.main;
    }
}
