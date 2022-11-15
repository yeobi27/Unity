using UnityEngine;

public class IceTiger_AutoSelfDestruct : MonoBehaviour
{
    public float selfDestructInSeconds;
    void Start()
    {
        Destroy(gameObject, selfDestructInSeconds);
    }
}
