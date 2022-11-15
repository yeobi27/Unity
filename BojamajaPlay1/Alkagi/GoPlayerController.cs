using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoPlayerController : MonoBehaviour
{
    private Camera cam;

    [SerializeField]
    private float originView;
    private float findView;
    public static GoPlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        SetInitialReferences();
        originView = 76f;
        findView = 51f;
    }
    public void FieldofViewActive()
    {
        StopAllCoroutines();
        Resources.UnloadUnusedAssets();
        StartCoroutine(_FieldofViewActive());
    }

    IEnumerator _FieldofViewActive()
    {
        while (cam.fieldOfView >= findView + 0.02f)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, findView, Time.deltaTime * 7f);

            yield return null;
        }
    }

    public void FieldofViewDeActive()
    {
        StopAllCoroutines();
        Resources.UnloadUnusedAssets();
        StartCoroutine(_FieldofViewDeActive());
    }

    IEnumerator _FieldofViewDeActive()
    {
        while (cam.fieldOfView <= originView - 0.02f)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, originView, Time.deltaTime * 7f);

            yield return null;
        }
    }

    private void SetInitialReferences()
    {
        cam = GetComponentInChildren<Camera>();

        if (!cam)
            cam = Camera.main;
    }
}
