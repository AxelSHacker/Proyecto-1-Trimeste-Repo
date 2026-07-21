using UnityEngine;
using UnityEngine.UI;

public class FlyingEnemy : MonoBehaviour
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
    [SerializeField] float attackVelocity;
    [SerializeField] int points;
    [SerializeField] float _radioBusqueda = 7f;
    [SerializeField] bool pointCount = true;
    [SerializeField] bool soundPlayed = false;
    [Header("REFERENCES"), SerializeField]
    Rigidbody2D _Rb;
    [SerializeField] Animator _anim;
    [SerializeField] Transform[] patrol;
    [SerializeField] Transform playerPosition;
    [SerializeField] Healt _healt;
    [SerializeField] GameObject lance;
    [SerializeField] GameObject fireProyectil;
    [SerializeField] Transform positionFireProyectil;
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
        attackTimer = maxAttackTimer;
        playerPosition = GameObject.Find("Player-Platform-Sariant (1)").transform;
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
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

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
        gameObject.Chase(_Rb,ref _distance, attackRadius, playerPosition, chaseVelocity);
        if (!soundPlayed)
        {
            soundPlayed = true;
            MusicManager.Instance.SFXPlayer(detected);
        }
    }
    private void AnimatorController()
    {
        _anim.SetFloat("Velocity", _Rb.linearVelocityX);
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
        if (pointCount)
        {
            GameManager.Instance.PicupCollectable(points);
            MusicManager.Instance.SFXPlayer(death);
            _anim.SetBool("Death", true);
            pointCount = false;
        }
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
        transform.EnemyRotattion(playerPosition);

        GameObject instantiateProyectil = Instantiate(proyectil, shootingPoint.position,
                                                      proyectil.transform.rotation);

        instantiateProyectil.transform.EnemyRotattion(playerPosition);

        instantiateProyectil.GetComponent<Rigidbody2D>().linearVelocity = direction * attackVelocity;

        Destroy(instantiateProyectil, 3f);
    }

    public void OneProyectil()
    {
        LaunchProyectil(fireProyectil, positionFireProyectil);
        MusicManager.Instance.SFXPlayer(attack);
    }

    public void TwoProyectil()
    {
        LaunchProyectil(fireProyectil, positionFireProyectil);
        LaunchProyectil(lance, positionLance);
    }

    #endregion
}
