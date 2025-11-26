using System;
using System.Data;
using JetBrains.Annotations;
using UnityEngine;

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
    public State currentState;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _Distance = Vector2.Distance(transform.position, playerPosition.position);
        if (_healt.health <= 0)
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
        //si la lista esta vacia, sale directamente
        if (patrol.Length < 2) return;
        //Aplicamos el movimiento 
        _Rb.MovePosition(Vector2.MoveTowards(transform.position, patrol[currentPatrolIndex].position, patrolVelocity * Time.deltaTime));
        //Apliacamos la velocidad al animator para la velocidad de anda
        _anim.SetFloat("Velocity", patrolVelocity);
        ///
        //Condiciones para que el el game object se gire hacia la dirreccion a la que se dirige
        if (currentPatrolIndex == 1)
            transform.rotation = Quaternion.Euler(0, 180, 0);

        else if (currentPatrolIndex == 0)
        { transform.rotation = Quaternion.Euler(0, 0, 0); }

        //Condiciones para que cuando se llegue al punto de transfor se dirija al siguiente puunto
        if (Vector2.Distance(transform.position, patrol[currentPatrolIndex].position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrol.Length;

            //enemySprite.flipX = patrol[currentPatrolIndex].position.x < transform.position.x;
        }

    }
    private void Chase()
    {
        if (_Distance > attackRadius)
        {
            EnemyTourn();
            _Rb.MovePosition(Vector2.MoveTowards(transform.position, playerPosition.position, chaseVelocity * Time.deltaTime));

            _anim.SetFloat("Velocity", chaseVelocity);
        }
        else { _Rb.MovePosition(transform.position); }


    }

    private void Attack()
    {
        _Rb.MovePosition(Vector2.zero);
        EnemyTourn();
        _anim.SetBool("Attack", true);
    }

    private void EnemyTourn()
    {
        if (playerPosition.position.x < transform.position.x)
            transform.rotation = Quaternion.Euler(0, 180, 0); // Mira a la izquierda
        else
            transform.rotation = Quaternion.Euler(0, 0, 0); // Mira a la derecha
    }

    private void Death()
    {

        _anim.SetBool("Death", true);
        _Rb.linearVelocity = Vector2.zero;


    }
    #region Animation Event
    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
    #endregion

}
