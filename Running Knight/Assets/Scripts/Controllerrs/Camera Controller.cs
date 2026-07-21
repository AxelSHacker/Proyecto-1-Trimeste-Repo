using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("REFERENCES")]
    
    //Transfor de e objetivo a seguir
    [SerializeField]
    Transform target;
    //Indica si la camara seguira al objetivo horizontalmente y o verticalmente
    [SerializeField]
    bool followX = true;
    [SerializeField]
    bool followY = false;
    //Desviacion de la posicion de la camra en x e y
    [Range(-2, 2)]
    public float offsetX = 1;
    [Range(-2, 2)]
    public float offsetY = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;

        if (followX)
        {
            newPosition.x = target.position.x + offsetX;
        }

        if (followY)
        {
            newPosition.y = target.position.y + offsetY;
        }

        transform.position = newPosition;
    }
}
