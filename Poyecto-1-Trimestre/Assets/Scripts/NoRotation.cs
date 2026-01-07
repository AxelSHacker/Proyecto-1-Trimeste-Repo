using UnityEngine;

public class KeepTextUpright : MonoBehaviour
{
    private Quaternion initialRotation;

    void Start()
    {
        // Guardamos la rotación inicial (que suele ser la correcta)
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // Forzamos al texto a mantener siempre la misma rotación global
        transform.rotation = initialRotation;
    }
}
