using UnityEngine;

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
    [SerializeField]
    float chaseVelocity;
    [SerializeField]
    int currentPatrolIndex;
    [SerializeField]
    float chaseRadius;
    [SerializeField]
    float _Distance;
    [SerializeField]
    float attackRadius;
    [SerializeField]
    float attackTimer;
    [SerializeField]
    float maxAttackTimer;
    [SerializeField]
    float attackVelocity;
    [Header("REFERENCES"), SerializeField]
    Rigidbody2D _Rb;
    [SerializeField]
    Animator _anim;
    [SerializeField]
    Transform[] patrol;
    [SerializeField]
    Transform playerPosition;
    [SerializeField]
    Healt _healt;
    [SerializeField]
    GameObject proyectil;

    [SerializeField]
    Transform shootingPoint;


    public State currentState;

    void Start()
    {
        attackTimer = maxAttackTimer;
    }

    void Update()
    {
        AnimatorController();
        _Distance = Vector2.Distance(transform.position, playerPosition.position);
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
        _Rb.constraints = RigidbodyConstraints2D.None;
        gameObject.Patrol(patrol, _Rb, ref currentPatrolIndex, patrolVelocity);

    }


    private void Chase()
    {
        _Rb.constraints = RigidbodyConstraints2D.None;
        gameObject.Chase(_Rb, _Distance, attackRadius, playerPosition, chaseVelocity);
        transform.EnemyRotattion(playerPosition);

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
        _Rb.linearVelocity = Vector2.zero;
        _anim.SetBool("Death", true);
    }




    #region Animation Event
    public void DisableObject()
    {
        gameObject.SetActive(false);
    }

    public void LaunchProyectil()
    {
        Vector2 direction = (new Vector2(playerPosition.position.x, playerPosition.position.y + 0.5f) - (Vector2)shootingPoint.position).normalized;
        transform.EnemyRotattion(playerPosition);
        GameObject instantiateProyectil = Instantiate(proyectil, shootingPoint.position, proyectil.transform.rotation);
        instantiateProyectil.transform.EnemyRotattion(playerPosition);
        instantiateProyectil.GetComponent<Rigidbody2D>().linearVelocity = direction * attackVelocity;
    }
    #endregion
}
