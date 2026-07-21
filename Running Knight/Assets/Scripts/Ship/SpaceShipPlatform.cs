
using NUnit.Framework.Interfaces;
using UnityEngine;

public class SpaceShipPlatform : MonoBehaviour
{
    #region Referencias 


    [Header("References"), SerializeField]
    Rigidbody2D shipRB;
    [SerializeField] Transform linePosition;
    [SerializeField] Transform from;
    [SerializeField] Transform to;
    [SerializeField] LayerMask contactLayer;
    [SerializeField] Transform playerTransform;
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] CanvasGroup shipAlert;

    #endregion





    #region Variables
    [SerializeField]
    float velocity;
    [SerializeField]
    float maxVelocity;
    [SerializeField]
    float lineLenght;
    [SerializeField]
    float timer;
    [SerializeField]
    float movementTimer;
    [SerializeField]
    float chaseTimer;
    [SerializeField]
    float chaseVelocity;


    #endregion





    #region Funciones

    void Start()
    {
        timer = movementTimer;
        shipAlert.SetEnable(false);
        velocity = maxVelocity;
    }


    void Update()
    {
        timer -= Time.deltaTime;  
    }

    private void FixedUpdate()
    {
        Movement();
        PlayerFinder();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.EndGame();
            playerRB.constraints = RigidbodyConstraints2D.FreezePositionY;
            playerRB.gravityScale = 1f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(linePosition.position, linePosition.position + (-linePosition.up) * lineLenght);
    }
    private void Movement()
    {
        if (timer <= 2f) shipAlert.SetEnable(true);
        if (timer <= 0f)
        {
            shipAlert.SetEnable(false);
            shipRB.AddForceX(velocity, ForceMode2D.Force);

            if (transform.position.x >= to.position.x)
            {
                shipRB.linearVelocity = Vector2.zero;
                transform.position = from.position;
                timer = movementTimer;
            }
        }
        else
        {
            transform.position = from.position;
        }

    }
    private void PlayerFinder()
    {
        RaycastHit2D contact = Physics2D.Linecast(linePosition.position, linePosition.position + (-linePosition.up) * lineLenght,
                                                                 contactLayer);

        if (contact.collider == null) return;
        if (contact.collider.gameObject.CompareTag("Player"))
        {
            chaseTimer = 5f;
            playerRB.gravityScale = -0.5f;
            velocity = 0;
        }
        else
        {
            chaseTimer -= Time.deltaTime;
            playerRB.gravityScale = 1f;
            velocity = maxVelocity;

        }
        if (chaseTimer >= 0f)
        {
            shipRB.MovePosition(Vector2.MoveTowards(transform.position, new Vector2(playerTransform.position.x, transform.position.y),
                                chaseVelocity * Time.deltaTime));
        }
    }
    private void EndGameMovement()
    {
        Vector2 separation = new Vector2(transform.position.x, transform.position.y);

        if (transform.position.x >= playerTransform.position.x) transform.position = from.position;

        separation.x = playerTransform.position.x - 6.8f;

        transform.position = Vector2.Lerp(transform.position, separation, 4f * Time.deltaTime);
    }


    #endregion




























}
