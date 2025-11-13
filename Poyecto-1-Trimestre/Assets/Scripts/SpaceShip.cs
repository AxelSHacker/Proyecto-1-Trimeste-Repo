using System;
using TreeEditor;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    //Distancia a la que se tiene que mantener la nave de manera normal
    [Header("REFERENCES"), SerializeField]
    //Rigidbody de la nabe

    Rigidbody2D _rb;
    [SerializeField]
    Rigidbody2D playerRB;
    //Transform del Player
    [SerializeField]
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
    float velocity;
    [SerializeField]
    float maxDistance;
    [SerializeField]
    Vector2 maxPlayerVelocity;





    void Start()
    {
        currentTime = waitTime;
        minScale = transforObject.localScale;
        maxDistance = Mathf.Abs(playerPosition.position.x - transform.position.x);
    }


    private void Update()
    {
        FollowPayer();
        ScaleControl();
    }

    //Con esta funcion seguimos al player
    private void FollowPayer()
    {
        //Posicion de el game object
        Vector3 shipVector = transform.position;
        currentTime -= Time.deltaTime;

        //Pasado determinado tiempo el game oject perseguira llentatmente al pllayer
        if (currentTime <= 0f && offSetX <= 0f)
        {
            if (maxPlayerVelocity.x < playerRB.linearVelocityX)
            {
                maxPlayerVelocity.x = playerRB.linearVelocityX;
            }

            if (playerRB.linearVelocityX == 0)
            {
                _rb.linearVelocityX = maxPlayerVelocity.x / 2;
            }
            else
            {
                Vector2.Lerp(_rb.linearVelocity, maxPlayerVelocity, 2f );
            }
                
            

        }
        else
        {
            shipVector.x = playerPosition.position.x + offSetX;

            transform.position = shipVector;
        }







    }

    private void ScaleControl()
    {
        float distance;

        //Distancia entre los 2 gameobjects
        distance = Mathf.Abs(playerPosition.position.x - transform.position.x);

        //Factor entre 00 y 1 para controlar la escala con rspecto a la distatncia
        float faktor = Mathf.Clamp01(maxDistance / distance);

        //Ajustatmos la escala con respecto al faktor
        transforObject.localScale = Vector2.Lerp(minScale, maxScale, faktor);


    }
}




