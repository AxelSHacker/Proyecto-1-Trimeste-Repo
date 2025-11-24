using UnityEngine;
using UnityEngine.Pool;

public class MoveTowars : MonoBehaviour
{
    [Range(-2, 2)]
    public float velocityX;
    [Range(-2, 2)]
    public float velocityY;
     private Vector3 tempVector = Vector3.zero;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tempVector = new Vector3 (velocityX, velocityY) * Time.deltaTime;
        transform.position += tempVector;
    }
}
