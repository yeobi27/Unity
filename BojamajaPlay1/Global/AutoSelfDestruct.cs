using UnityEngine;

public class AutoSelfDestruct : MonoBehaviour
{
    public float selfDestructInSeconds;
    void Start()
    {
        Destroy(gameObject, selfDestructInSeconds);
    }
}
