using UnityEngine;
using UnityEngine.UIElements;

public class RandomEnviromentSpawner : MonoBehaviour
{
    public GameObject[] prefabEnviroment;
    [Range(0f, 1f)]
    public float spawnRatio = 1f;
        void Start()
    {
        if (prefabEnviroment == null)
        {
            Debug.LogWarning("No se ha especificado el objettop que debe instanciarse");
            return;
        }

        if (Random.value <= spawnRatio)
        {
            Instantiate(prefabEnviroment[Random.Range(0, prefabEnviroment.Length)], transform.position, Quaternion.identity, transform);
        }
    }
}
