using UnityEngine;

public class GrillingMeat_AutoSelfDestruct : MonoBehaviour
{
    public float selfDestructInSeconds;
    void Start()
    {
        Destroy(gameObject, selfDestructInSeconds);
    }
}
