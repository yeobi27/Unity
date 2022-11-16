using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillingMeat_PlayerContoller : MonoBehaviour
{
    private Camera cam;
    private Animator camAnimator;
    private Vector3 rayPointPos;
    public LayerMask layerMask;
    
    public static GrillingMeat_PlayerContoller Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else Instance = this;
    }
    void Start()
    {
        SetInitialReferences();
        camAnimator.SetBool("ZoomInOut", true);
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        rayPointPos = ray.GetPoint(10f);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 15f, layerMask))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Meats"))
            {
                if (hit.collider.CompareTag("Meat"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        hit.transform.gameObject.GetComponent<Meat>().TurnUp();
                    }
                }
            }
        }

    }

    private void OnGUI()
    {
        Debug.DrawRay(cam.transform.position, rayPointPos - cam.transform.position, Color.magenta, 0.2f);
    }

    void SetInitialReferences()
    {
        cam = GetComponentInChildren<Camera>();

        if (!cam)
        {
            cam = Camera.main;
        }
        camAnimator = GetComponentInChildren<Animator>();
    }
}
