using UnityEngine;

public class TreeSlashAutoSelfDestruct : MonoBehaviour
{
    public float selfDestructInSeconds;
    void Start()
    {
        Destroy(gameObject, selfDestructInSeconds);
        Resources.UnloadUnusedAssets();
    }
}
