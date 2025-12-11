using System;
using System.Data;
using JetBrains.Annotations;
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
    int points;
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

    public State currentState;


    void Update()
    {
        healtBar.value = _healt.enemyHealth;
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
        gameObject.Patrol(patrol, _Rb, ref currentPatrolIndex, patrolVelocity);

    }


    private void Chase()
    {
        gameObject.Chase(_Rb, _Distance, attackRadius, playerPosition, chaseVelocity);
        MusicManager.Instance.SFXPlayer(detected);

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
        GameManager.Instance.PicupCollectable(points); 
        MusicManager.Instance.SFXPlayer(death);
        _Rb.linearVelocity = Vector2.zero;
        _anim.SetBool("Death", true);
    }




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
