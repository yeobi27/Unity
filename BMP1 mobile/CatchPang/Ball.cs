using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Collider col;
    private MeshRenderer meshRenderer;
    private Vector3 currPos;
    private Vector3 deltaVector;
    private Vector3 prevPos;

    public string[] missSfx;

    void Start()
    {
        SetInitialReferences();
        prevPos = transform.position;
    }

    void Update()
    {
        currPos = transform.position;

        deltaVector = currPos - prevPos;

        prevPos = transform.position;
    }

    void SetInitialReferences()
    {
        rigidBody = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void OnCollisionEnter(Collision _col)
    {
        GameObject go;

        if (LayerMask.LayerToName(_col.collider.gameObject.layer) == "Animals")
        {            
            go = Instantiate(CatchPang_DataManager.Instance.hitParticles);
        }
        else
        {
            go = Instantiate(CatchPang_DataManager.Instance.missParticles);

            CatchPang_SoundManager.Instance.PlaySE(missSfx[Random.Range(0, 2)]);
        }

        go.transform.SetParent(this.transform);
        go.transform.position = this.transform.position;

        this.col.enabled = false;
        rigidBody.isKinematic = true;
        meshRenderer.enabled = false;

        Destroy(gameObject, 2f);
    }

    private void EnableGravity()
    {
        rigidBody.isKinematic = false;
    }

    public void Throw(Vector3 forceVector, float strength)
    {
        EnableGravity();

        Debug.DrawRay(Camera.main.transform.position, forceVector, Color.magenta, 10f);
        Debug.DrawRay(prevPos, deltaVector, Color.white, 10f);

        Debug.DrawRay(Camera.main.transform.position, forceVector + (deltaVector * 3f), Color.cyan, 10f);


        rigidBody.AddForce((forceVector + (deltaVector * 3f)).normalized * strength, ForceMode.Impulse);
    }
}