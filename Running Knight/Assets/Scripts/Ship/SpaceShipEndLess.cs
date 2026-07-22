using System;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceShipEndLess : MonoBehaviour
{
    //Distancia a la que se tiene que mantener la nave de manera normal
    [Header("REFERENCES"), SerializeField]
    //Rigidbody de la nabe
    Rigidbody2D _rb;
    [SerializeField] Rigidbody2D playerRB;
    //Transform del Player
    [SerializeField] Transform playerPosition;
    [SerializeField] Transform transforObject;
    [SerializeField] Vector2 minScale;
    [SerializeField] Vector2 maxScale; [SerializeField]
    PlayerControllerEndLess _payerController;


    [Header("PARAMETER")]
    [SerializeField] float offSetX;
    [SerializeField] float waitTime;
    [SerializeField] float currentTime;
    [SerializeField] float velocity;
    [SerializeField] float maxDistance;
    [SerializeField] float maxShipVelocity;
    [SerializeField] float abductionSpeed = 3f; // Velocidad a la que el OVNI "absorbe" al jugador
    bool _hasReachedPlayer = false;
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
            GameManager.Instance.ContinueGame();
            other.gameObject.SetActive(false);
        }
    }
    //Con esta funcion seguimos al player
    private void FollowPayer()
    {
        currentTime -= Time.fixedDeltaTime;

        // 1. Si ya alcanzó al jugador, la mantenemos fija en la X del player y no aplicamos más fuerzas
        if (_hasReachedPlayer)
        {
            _rb.linearVelocity = Vector2.zero;

            // Mantenemos la nave fija en la X del player
            transform.position = new Vector3(playerPosition.position.x, transform.position.y, transform.position.z);

            // 🛸 EFECTO ABDUCCIÓN: Subimos al player hacia la posición de la nave (o al abductionPoint)
            Vector3 targetPoint = (transform.position != null) ? transform.position : transform.position;

            // Desactivamos la gravedad del player para que no luche contra el movimiento
            playerRB.gravityScale = 0f;
            playerRB.linearVelocity = Vector2.zero; // Anulamos cualquier inercia previa

            // Movemos progresivamente al jugador hacia arriba/centro de la nave
            playerPosition.position = Vector3.MoveTowards(
                playerPosition.position,
                targetPoint,
                abductionSpeed * Time.fixedDeltaTime
            );

            return;
        }

        // 2. Calculamos la separación real
        float separacion = Mathf.Abs(playerPosition.position.x - transform.position.x);

        // Usamos un margen (ej. 0.3f) en lugar de Math.Round para que no falle por alta velocidad
        if (separacion <= 0.3f)
        {
            _hasReachedPlayer = true; // Marcamos que ya llegó

            _rb.linearVelocity = Vector2.zero;
            _payerController.autoMovement = false;
            if (playerPosition.TryGetComponent(out Collider2D collider2D))
            {
                collider2D.isTrigger = true;
            }
            // Fijamos su posición X exacta en este frame
            transform.position = new Vector3(playerPosition.position.x, transform.position.y, transform.position.z);
            return;
        }

        // 3. Si aún no ha llegado, sigue aplicando fuerza hacia adelante
        _rb.AddForce(transform.right * velocity, ForceMode2D.Force);

        if (_rb.linearVelocityX >= maxShipVelocity)
        {
            _rb.linearVelocityX = maxShipVelocity;
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


















































