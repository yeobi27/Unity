using UnityEngine;
using System.Collections;

public class CatchPang_PlayerController : MonoBehaviour
{
    private Camera cam;
    private Ball currentBall;
    private float dist;
    private Vector3 rayPointPos;

    [Header("Player Info")]
    public GameObject hand;
    public LayerMask projectileLayer;
    public float strength;
    public CanvasGroup playerHit;
    public string[] flyingSfx;

    [Header("Balls")]
    public GameObject[] balls;

    public static CatchPang_PlayerController Instance { get; private set; }
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
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(cam.transform.position, Input.mousePosition, Color.magenta, 0.2f);
        // projectileLayer > Ball
        if (Physics.Raycast(ray, out hit, 5f, projectileLayer))
        {
            //Debug.Log("Raycast hit: " + hit.transform.name);
            if (hit.collider.CompareTag("Ball"))
            {                
                if (Input.GetMouseButtonDown(0))
                {
                    foreach (var ball in balls)
                    {   
                        //If the material of the ball and the material of the colliding object are the same
                        if (ball.GetComponent<MeshRenderer>().sharedMaterial == hit.transform.GetComponent<MeshRenderer>().sharedMaterial)
                            currentBall = Instantiate(ball).GetComponent<Ball>();
                    }

                    // Move the position 0.5 towards the camera.
                    currentBall.transform.localScale = hit.transform.lossyScale;
                    dist = Vector3.Distance(cam.transform.position, hit.transform.position);
                    dist -= 0.5f;
                }

            }
        }
        // If clicked
        if (currentBall != null)
        {
            // Move the ball you are holding to the position where the ray is touching.
            rayPointPos = ray.GetPoint(dist);
            currentBall.transform.position = rayPointPos;

            if (Input.GetMouseButtonUp(0))
            {
                CatchPang_SoundManager.Instance.PlaySE(flyingSfx[Random.Range(0,2)]);

                //Debug.Log("rayPointPos - cam.transform.position : " + (rayPointPos - cam.transform.position));
                currentBall.Throw(rayPointPos - cam.transform.position, strength);
                currentBall = null;
            }
        }
    }
    void OnGUI()
    {
        Debug.DrawRay(cam.transform.position, rayPointPos - cam.transform.position, Color.magenta, 0.2f);
    }

    public void GetHit()
    {
        StopCoroutine("_GetHit");
        StartCoroutine("_GetHit");
    }
    IEnumerator _GetHit()
    {
        CatchPang_DataManager.Instance.SubtractScore();
        
        float value = 1;
        playerHit.alpha = value;

        while (value > 0.1f)
        {
            value -= Time.deltaTime * 7f;
            playerHit.alpha = value;
            yield return null;
        }

        playerHit.alpha = 0f;
    }

    void SetInitialReferences()
    {
        cam = GetComponentInChildren<Camera>();
        if (!cam)
            cam = Camera.main;
    }
}