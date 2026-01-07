
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
    [SerializeField] float chaseRadius;
    [SerializeField] float _Distance;
    [SerializeField] float attackRadius;
    [SerializeField] int points;
    [SerializeField] int maxSFXRpeat = 0;
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

    public State currentState;
    void Start()
    {
        StateUpdate(State.Patrol);
        playerPosition = GameObject.Find("Player-Platform-Sariant (1)").transform;
    }


    void Update()
    {
        healtBar.value = _healt.enemyHealth;
        AnimatorController();
        if (_healt.enemyHealth <= 0)
        {
            StateUpdate(State.Death);
        }
        else if (_Distance <= attackRadius)
        {

            StateUpdate(State.Attack);
        }
        else { _anim.SetBool("Attack", false); }



    }

    void FixedUpdate()
    {
        _Distance = Vector2.Distance(transform.position, playerPosition.position);

        if (_Distance < chaseRadius)
        {
            StateUpdate(State.Chase);
        }
        else if (_Distance > chaseRadius)
        {
            StateUpdate(State.Patrol);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

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
                healtBar.enabled = false;
                Death();
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
        gameObject.Chase(_Rb, _Distance, attackRadius, playerPosition, chaseVelocity);

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
        gameObject.SetActive(false);
    }

    public void AttackSound()
    {
        MusicManager.Instance.SFXPlayer(attack);
    }
    #endregion

}
