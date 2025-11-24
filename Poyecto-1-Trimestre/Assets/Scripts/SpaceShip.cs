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
    [SerializeField]
    PayerControlle _payerController;


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
    float maxShipVelocity;
    [SerializeField]
    float rotationVelocity;





    void Start()
    {
        currentTime = waitTime;

        maxDistance = Mathf.Abs(playerPosition.position.x - transform.position.x);
    }


    private void Update()
    {
        ScaleControl();

    }

    void FixedUpdate()
    {
        FollowPayer();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRB.gravityScale = 0f;
            _rb.linearVelocity = Vector2.zero;
        }
    }
        
    


    //Con esta funcion seguimos al player
    private void FollowPayer()
    {

        //Posicion de el game object
        Vector3 shipVector = transform.position;
        //Activamos la cuenta atras
        currentTime -= Time.deltaTime;
        //Pasado determinado tiempo la nabe persegauira al player
        //flotante que almacena la distancia entre 2 posiciones en x
        float separacion = Mathf.Abs(playerPosition.position.x - transform.position.x);
        //Redondeamos la separacion para que pille la entrada si no , no se por que , pero no  
        if (Math.Round(separacion) == 0f)
        {
            //La nabe se queda en la posicion x del player y se ejecuta la absorcion y la pantalla
            //De carga a la siguiene fase

            _payerController.autoMovement = false;

            _rb.linearVelocity = Vector2.zero;
            playerRB.gravityScale = -0.5f;

            return;


        }
        //Cuando termine la cuenta atras la nave avanzara a su propia velocidad
        if (currentTime <= 0f)
        {
            _rb.AddForce(transform.right * velocity, ForceMode2D.Force);
            if (_rb.linearVelocityX >= maxShipVelocity)
            {
                _rb.linearVelocityX = maxShipVelocity;
            }
        }
        //Si aun no ha terminado la cuenta ,la nabe persigue al player en uns distancia fija
        else
        {
            shipVector.x = playerPosition.position.x + offSetX;
            transform.position = shipVector;
        }




    }
    //Con esta funcion lo que quiero es controlar la escala de la nabe con 
    //respecto a la distancia que esta del player
    private void ScaleControl()
    {
        float distance;
        //Distancia entre los 2 gameobjects
        distance = Mathf.Abs(playerPosition.position.x - transform.position.x);
        //Factor entre 00 y 1 para controlar la escala con respecto a la distatncia
        float faktor = Mathf.Clamp01(distance / maxDistance);
        //Ajustatmos la escala con respecto al faktor
        transforObject.localScale = Vector2.Lerp(maxScale, minScale, faktor);
    }

































}




