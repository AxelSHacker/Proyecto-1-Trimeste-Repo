using System;
using TreeEditor;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    //Distancia a la que se tiene que mantener la nave de manera normal
    [Header("REFERENCES"), SerializeField]
    //Transform del Player
    Transform playerPosition;
    [SerializeField]
    Transform transforObject;
    [SerializeField]
    Vector2 minScale;
    [SerializeField]
    Vector2 maxScale;

    [Header("PARAMETER")]
    [SerializeField]
    float offSetX;
    [SerializeField]
    float waitTime;
    [SerializeField]
    float currentTime;
    [SerializeField]
    float smoothing;
    [SerializeField]
    


    void Start()
    {
        currentTime = waitTime;
        minScale = transforObject.localScale;
    }


    private void Update()
    {

        FollowPayer();
        ScaleControl();
    }

    private void FollowPayer()
    {
        
        Vector3 objectPosition = transform.position;
        currentTime -= Time.deltaTime;

        if (currentTime <= 0f && offSetX <= 0f)
        {

            offSetX += Time.deltaTime;



        }

        objectPosition.x = playerPosition.transform.position.x + offSetX;

        transform.position = objectPosition;



    }

    private void ScaleControl()
    {

        float distance;

        distance = Vector2.Distance(transform.position, playerPosition.position);

        float percent = Mathf.InverseLerp(8.5f, 7.35f, distance);

        transforObject.localScale = Vector2.Lerp(minScale, maxScale, percent);

        
    }
}
        

        

