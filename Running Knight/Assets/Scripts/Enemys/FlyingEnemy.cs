using UnityEngine;
using UnityEngine.UI;

public class FlyingEnemy : MonoBehaviour, IDamageabe<int>
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
    [SerializeField] int _vidaActual;
    [SerializeField] int _vidaMaxima;
    [Header("REFERENCES"), SerializeField]
    Rigidbody2D _Rb;
    [SerializeField] Animator _anim;
    [SerializeField] Transform[] patrol;
    [SerializeField] Transform playerPosition;
    [SerializeField] GameObject lance;
    [SerializeField] GameObject fireProyectil;
    [SerializeField] Transform positionFireProyectil;
    [SerializeField] Transform positionLance;
    [SerializeField] Slider healtBar;
    [SerializeField] AudioClip detected;
    [SerializeField] AudioClip attack;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip[] swordImpactSound;

    [SerializeField] State currentState;

    int IDamageabe<int>.Maxhealt => _vidaMaxima;
    int IDamageabe<int>.Currentealt => _vidaActual;
    bool IDamageabe<int>.IsDead => _vidaActual <= 0;

    void Awake()
    {
        healtBar.value = _vidaMaxima;
        _vidaActual = _vidaMaxima;
    }
    void Start()
    {
        attackTimer = maxAttackTimer;
        if (playerPosition == null) playerPosition = GameObject.FindWithTag("Player")?.GetComponent<Transform>();
        _anim.SetBool("Death", false);
        StateUpdate(State.Patrol);
    }

    void Update()
    {
        _distance = Vector2.Distance(transform.position, playerPosition.position);

        AnimatorController();
        if (_vidaActual <= 0)
        {
            StateUpdate(State.Death);
        }
        else if (_distance <= attackRadius)
        {
            StateUpdate(State.Attack);
        }
        else if (_distance <= _radioBusqueda)
        {
            StateUpdate(State.Chase);
        }
        else if (_distance > _radioBusqueda)
        {
            StateUpdate(State.Patrol);
        }
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
        if (collision.gameObject.TryGetComponent(out IDamageabe<int> component))
        {
            MusicManager.Instance.SFXPlayer(swordImpactSound[randomIndex]);
        }
    }
    #endregion

    #region Funtions
    private void Patrol()
    {
        _Rb.constraints = RigidbodyConstraints2D.None;
        gameObject.Patrol(patrol, _Rb, ref currentPatrolIndex, patrolVelocity);
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
        _anim.SetFloat("Velocity", patrolVelocity);
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
        _Rb.linearVelocity = Vector2.zero;
        if (pointCount)
        {
            GameManager.Instance.PicupCollectable(points);
            MusicManager.Instance.SFXPlayer(death);
            _anim.SetBool("Death", true);
            pointCount = false;
        }
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

    public void TakeDamag(int damage, Vector3 impactPoint = default)
    {
        _vidaActual -= damage;
        healtBar.value = _vidaActual;
        _vidaActual = Mathf.Clamp(_vidaActual, 0, _vidaMaxima);
    }


    #endregion
}
