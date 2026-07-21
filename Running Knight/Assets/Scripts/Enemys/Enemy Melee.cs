
using UnityEngine;
using UnityEngine.UI;

public class EnemyMelee : MonoBehaviour
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

    [Header("REFERENCES"), SerializeField]
    Rigidbody2D _Rb;
    [SerializeField] Animator _anim;
    [SerializeField] Transform[] patrol;
    [SerializeField] Transform playerPosition;
    [SerializeField] Healt _healt;
    [SerializeField] Slider healtBar;
    [SerializeField] AudioClip detected;
    [SerializeField] AudioClip attack;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip[] swordImpactSound;
    [SerializeField] Collider[] colliders;
    [SerializeField] LayerMask layerMask;

    public State currentState;

    void Awake()
    {
        healtBar.value = _healt.enemyHealth;
        playerPosition = null;
    }
    void Start()
    {
        StateUpdate(State.Patrol);

        _anim.SetBool("Death", false);
        colliders = new Collider[1];
        StateUpdate(State.Patrol);
    }


    void Update()
    {
        playerPosition = gameObject.BusquedadeObjetivo(_radioBusqueda, layerMask, colliders);
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

        if (collision.gameObject.CompareTag("Sword"))
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

            case State.Death:
                Death();
                break;
        }
    }
    //Funcion que hace que el Rigidbody vaya patrullando entre 2 puntos
    private void Patrol()
    {
        gameObject.Patrol(patrol, _Rb, ref currentPatrolIndex, patrolVelocity, _radioBusqueda, layerMask, colliders);
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
        _Rb.MovePosition(Vector2.zero);

        _anim.SetBool("Attack", true);
    }
    private void Death()
    {
        if (pointCount)
        {
            Debug.Log("Entro");
            healtBar.enabled = false;
            GameManager.Instance.PicupCollectable(points);
            pointCount = false;
            MusicManager.Instance.SFXPlayer(death);
            _anim.SetBool("Death", true);
        }
        _Rb.linearVelocity = Vector2.zero;
    }
    #endregion





    #region Animation Event
    public void DisableObject()
    {
        //gameObject.SetActive(false);
    }

    public void AttackSound()
    {
        MusicManager.Instance.SFXPlayer(attack);
    }
    #endregion

}
