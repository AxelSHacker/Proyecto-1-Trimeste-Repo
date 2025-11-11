
using UnityEngine;

public class RandomObjectSpawnPoint : MonoBehaviour
{

    public GameObject prefab;
    [Range(0f, 1f)]
    public float spawnRatio = 1f;
        void Start()
    {
        if (prefab == null)
        {
            Debug.LogWarning("No se ha especificado el objettop que debe instanciarse");
            return;
        }

        if (Random.value <= spawnRatio)
        {
            Instantiate(prefab, transform.position, Quaternion.identity, transform);
        }
    }

}
