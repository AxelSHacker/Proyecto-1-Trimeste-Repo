
using UnityEngine;
using UnityEngine.UI;
using static EnemyMelee;

public class EnemyMelee : MonoBehaviour, IDamageabe<int>
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
    [SerializeField] float _distance;
    [SerializeField] float attackRadius;
    [SerializeField] float _radioBusqueda = 7f;
    [SerializeField] int points;
    [SerializeField] bool pointCount = true;
    [SerializeField] bool soundPlayed = false;
    [SerializeField] int _vidaActual;
    [SerializeField] int _vidaMaxima;
    bool _vivo = true;

    [Header("REFERENCES"), SerializeField]
    Rigidbody2D _Rb;
    [SerializeField] Animator _anim;
    [SerializeField] Transform[] patrol;
    [SerializeField] Transform playerPosition;
    [SerializeField] Slider healtBar;
    [SerializeField] AudioClip detected;
    [SerializeField] AudioClip attack;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip[] swordImpactSound;
    public State currentState;

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
        StateUpdate(State.Patrol);
        if (playerPosition == null) playerPosition = GameObject.FindWithTag("Player")?.GetComponent<Transform>();
        _anim.SetBool("Death", false);
    }


    void Update()
    {
        AnimatorController();
        if (!_vivo) return;
        _distance = Vector2.Distance(transform.position, playerPosition.position);
        if (_distance <= attackRadius)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int randomIndex = UnityEngine.Random.Range(0, swordImpactSound.Length);

        if (collision.gameObject.TryGetComponent(out IDamageabe<int> component))
        {
            MusicManager.Instance.SFXPlayer(swordImpactSound[randomIndex]);
        }
    }
    #region  Funtions
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
        }
    }
    //Funcion que hace que el Rigidbody vaya patrullando entre 2 puntos
    private void Patrol()
    {
        gameObject.Patrol(patrol, _Rb, ref currentPatrolIndex, patrolVelocity);
        soundPlayed = false;
    }
    private void Chase()
    {
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
    private void Attack()
    {
        _Rb.linearVelocityX = 0f;

        _anim.SetTrigger("Attack");
    }
    private void Death()
    {
        _Rb.linearVelocity = Vector2.zero;
        GameManager.Instance.PicupCollectable(points);
        MusicManager.Instance.SFXPlayer(death);
        _anim.SetBool("Death", true);
        healtBar.enabled = false;
        _vivo = false;
    }
    #endregion





    #region Animation Event
    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
    public void AttackSound()
    {
        MusicManager.Instance.SFXPlayer(attack);
    }
    public void TakeDamag(int damage, Vector3 impactPoint = default)
    {
        _vidaActual -= damage;
        healtBar.value = _vidaActual;
        _vidaActual = Mathf.Clamp(_vidaActual, 0, _vidaMaxima);

        if (_vidaActual <= 0)
        {
            Death();
        }
    }
    #endregion

}

