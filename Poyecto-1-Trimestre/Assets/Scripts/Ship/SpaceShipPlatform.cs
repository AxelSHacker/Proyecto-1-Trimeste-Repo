using UnityEditor.Rendering;
using UnityEngine;

public class SpaceShipPlatform : MonoBehaviour
{
    #region Referencias 


    [Header("References"), SerializeField]
    Rigidbody2D shipRB;
    [SerializeField]
    Transform linePosition;
    [SerializeField]
    Transform from;
    [SerializeField]
    Transform to;
    [SerializeField]
    LayerMask contactLayer;
    [SerializeField]
    Transform playerTransform;
    [SerializeField]
    Rigidbody2D playerRB;

    #endregion





    #region Variables
    [SerializeField]
    float velocity;
    [SerializeField]
    float maxVelocity;
    [SerializeField]
    bool playerCheck = false;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(linePosition.position, linePosition.position + (-linePosition.up) * lineLenght);
    }
    private void Movement()
    {
        
        if (timer <= 0f)
        {
            shipRB.AddForceX(velocity, ForceMode2D.Force);

            if (transform.position.x >= to.position.x)
            {
                shipRB.linearVelocity = Vector2.zero;
                transform.position = from.position;
                timer = movementTimer;
            }

        } 
        //PlayerFinder();
       
    }

    private void PlayerFinder()
    {


        RaycastHit2D contact = Physics2D.Linecast(linePosition.position, linePosition.position + (-linePosition.up) * lineLenght,
                                                                 contactLayer);



        if (contact.collider.gameObject.CompareTag("Player"))
        {

            chaseTimer = 5f;
            playerCheck = true;
            playerRB.gravityScale = -0.5f;
            velocity = 0;
            


        }
        else
        {
            chaseTimer -= Time.deltaTime;
            playerCheck = false;
            playerRB.gravityScale = 1f;
            velocity = 0.5f;

        }
        if (chaseTimer >= 0f)
        {
            shipRB.MovePosition(Vector2.MoveTowards(transform.position, new Vector2(playerTransform.position.x, transform.position.y),
                                chaseVelocity * Time.deltaTime));
        }

            



    }

    #endregion








}
