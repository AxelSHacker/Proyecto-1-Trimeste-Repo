using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFloorRange : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase,
        Attack,
        Death,
    }


    [Header("VARIABLES"), SerializeField]
    float patrolVelocity;
    [SerializeField] float chaseVelocity;
    [SerializeField] int currentPatrolIndex;
    [SerializeField] float chaseRadius;
    [SerializeField] float _distance;
    [SerializeField] float attackRadius;
    [SerializeField] float attackTimer;
    [SerializeField] float maxAttackTimer;
    [SerializeField] float speed;
    [SerializeField] float attackVelocity;
    [SerializeField] int points;
    [SerializeField] bool soundPlayed = false;
    [SerializeField] float _radioBusqueda = 7f;

    [Header("REFERENCES"), SerializeField]
    Rigidbody2D _Rb;
    Vector2 lastPosition;
    [SerializeField] Animator _anim;
    [SerializeField] Transform[] patrol;
    [SerializeField] Transform playerPosition;
    [SerializeField] Healt _healt;
    [SerializeField] GameObject lance;
    [SerializeField] Transform positionLance;
    [SerializeField] Slider healtBar;
    [SerializeField] AudioClip detected;
    [SerializeField] AudioClip attack;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip[] swordImpactSound;
    [SerializeField] Collider[] colliders;
    [SerializeField] LayerMask layerMask;


    public State currentState;

    void Start()
    {
        playerPosition = GameObject.Find("Player-Platform-Sariant (1)").transform;
        attackTimer = maxAttackTimer;
        lastPosition = _Rb.position;
        _anim.SetBool("Death", false);
        colliders = new Collider[1];
        playerPosition = null;
        StateUpdate(State.Patrol);
    }

    void Update()
    {
        healtBar.value = _healt.enemyHealth;

        AnimatorController();
        if (_healt.enemyHealth <= 0)
        {
            StateUpdate(State.Death);
        }
        else if (_distance <= attackRadius)
        {
            StateUpdate(State.Attack);
        }
        else if (colliders.Length > 0)
        {
            StateUpdate(State.Chase);
        }
        else if (colliders.Length <= 0)
        {
            StateUpdate(State.Patrol);
        }
    }

    void FixedUpdate()
    {
       
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radioBusqueda);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
    //Funcion para ir cambiando los estados del enemigo
    private void StateUpdate(State currentState)
    {


        switch (currentState)
        {
            case State.Patrol:

                Patrol();
                break;

            case State.Chase:

                Chase();
                break;

            case State.Attack:

                Attack();

                break;
            case State.Death:
                healtBar.enabled = false;
                Death();
                break;


        }
    }

    #region OnTrigger/OnCollider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int randomIndex = UnityEngine.Random.Range(0, swordImpactSound.Length);


        if (collision.gameObject.CompareTag("Sword"))
        {
            MusicManager.Instance.SFXPlayer(swordImpactSound[randomIndex]);

        }
    }
    #endregion

    #region Funtions
    private void Patrol()
    {
        _Rb.constraints = RigidbodyConstraints2D.None;
        gameObject.Patrol(patrol, _Rb, ref currentPatrolIndex, patrolVelocity, _radioBusqueda, layerMask, colliders);
        soundPlayed = false;
    }


    private void Chase()
    {
        _Rb.constraints = RigidbodyConstraints2D.None;
        gameObject.Chase(_Rb, ref _distance, attackRadius, playerPosition, chaseVelocity);
        if (!soundPlayed)
        {
            soundPlayed = true;
            MusicManager.Instance.SFXPlayer(detected);

        }

    }
    private void AnimatorController()
    {
        _anim.SetFloat("Velocity", speed);
    }
    public void Attack()
    {
        transform.EnemyTourn(playerPosition);

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {

            _anim.SetTrigger("Attack");

            attackTimer = maxAttackTimer;

        }
    }
    private void Death()
    {
        GameManager.Instance.PicupCollectable(points);
        MusicManager.Instance.SFXPlayer(death);
        _anim.SetBool("Death", true);
        _Rb.linearVelocity = Vector2.zero;
    }
    #endregion






















    #region Animation Event
    public void DisableObject()
    {
        gameObject.SetActive(false);
    }

    public void LaunchProyectil(GameObject proyectil, Transform shootingPoint)
    {
        Vector2 direction = (new Vector2(playerPosition.position.x, playerPosition.position.y + 0.5f) -
                             (Vector2)shootingPoint.position).normalized;



        GameObject instantiateProyectil = Instantiate(proyectil, shootingPoint.position,
                                                      proyectil.transform.rotation);

        instantiateProyectil.transform.EnemyRotattion(playerPosition);

        instantiateProyectil.GetComponent<Rigidbody2D>().linearVelocity = direction * attackVelocity;

        Destroy(instantiateProyectil, 3f);
    }

    public void OneProyectil()
    {
        LaunchProyectil(lance, positionLance);
        MusicManager.Instance.SFXPlayer(attack);
    }



    #endregion

}
