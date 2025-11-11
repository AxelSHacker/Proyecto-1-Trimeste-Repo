
using System.Linq;
using UnityEngine;

public class RandomObjectSpawnPoint : MonoBehaviour
{

    [System.Serializable]
    public struct ObjectRatio
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float ratio;
    }
    public ObjectRatio[] prefas;
    public float spawnRatio;


    void Start()
    {
        if (prefas == null || prefas.Length == 0)
        {
            Debug.LogWarning("No se ha especificado el objettop que debe instanciarse");
            return;
        }
        bool spawn = Random.value <= spawnRatio;

        if (spawn)
        {
            float random = Random.value;
            ObjectRatio prefab = GetObjectToSpawn(random);
            Instantiate(prefab.prefab, transform.position, Quaternion.identity, transform.parent);
            //Instantiate(prefab, transform.position, Quaternion.identity, transform);
        }
    }

    private ObjectRatio GetObjectToSpawn(float ratio)
    {
        ObjectRatio[] ordererPrefabs = prefas.OrderBy(p => p.ratio).ToArray();

        for (int i = 0; i < ordererPrefabs.Length; i++)
        {
            if (ordererPrefabs[i].ratio >= ratio)
            {
                return ordererPrefabs[i];
            }
        }
        return ordererPrefabs[ordererPrefabs.Length - 1];
    }

}
