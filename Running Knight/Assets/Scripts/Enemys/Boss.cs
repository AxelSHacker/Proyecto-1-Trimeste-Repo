using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

    public enum State
    {

        RangeAttack,

        Attack,

        Death,

    }
    [Header("VARIABLES"), SerializeField]


    float rangeAttackRadius;
    [SerializeField] float _Distance;
    [SerializeField] float rollingAttackRadius;
    [SerializeField] float attackTimer;
    [SerializeField] float maxAttackTimer;
    [SerializeField] float attackImpulseForce;
    [SerializeField] int points;
    [SerializeField] float heightAttack;
    [SerializeField] float AttackProyectilForce;

    [Header("REFERENCES"), SerializeField]
    Rigidbody2D _Rb;
    [SerializeField] Animator _anim;
    [SerializeField] Transform playerPosition;
    [SerializeField] Healt _healt;
    [SerializeField] GameObject bossProyectil;
    [SerializeField] Transform bossProyectilPosition;
    [SerializeField] Transform bossProyectilPosition1;

    [SerializeField] Transform bossProyectilPosition2;
    [SerializeField] Slider healtBar;



    public State currentState;

    void Start()
    {

        attackTimer = maxAttackTimer;
    }

    void Update()
    {
        healtBar.value = _healt.enemyHealth;

        AnimatorController();

        _Distance = Vector2.Distance(transform.position, playerPosition.position);

        if (_healt.enemyHealth <= 0)
        {
            StateUpdate(State.Death);
        }


    }

    void FixedUpdate()
    {


        if (_Distance <= rollingAttackRadius)
        {
            StateUpdate(State.Attack);
            _anim.SetBool("Roar", false);
        }
        else if (_Distance <= rangeAttackRadius && _Distance >= rollingAttackRadius)
        {
            StateUpdate(State.RangeAttack);
        }
        






    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeAttackRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rollingAttackRadius);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _anim.SetBool("isAttacking", false);

        }
    }
    //Funcion para ir cambiando los estados del enemigo
    private void StateUpdate(State currentState)
    {
        switch (currentState)
        {

            case State.RangeAttack:

                RangeAttack();
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



    private void RangeAttack()
    {

        _anim.SetBool("Roar", true);

        if (playerPosition.position.y >= heightAttack)
        {

            _anim.SetTrigger("RangeAttack");
        }

    }

    private void AnimatorController()
    {
        _anim.SetFloat("Velocity", _Rb.linearVelocityX);

    }

    public void Attack()
    {


        transform.EnemyTourn(playerPosition);

        _anim.SetTrigger("Attack");

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            Vector2 direction = (new Vector2(playerPosition.position.x, playerPosition.position.y + 0.5f) -
                                         (Vector2)transform.position).normalized;

            _Rb.AddForceX(direction.x * attackImpulseForce, ForceMode2D.Impulse);

            //attackTimer = maxAttackTimer;

        }


    }

    private void Death()
    {
        GameManager.Instance.PicupCollectable(points);
        _Rb.linearVelocity = Vector2.zero;
        _anim.SetBool("Death", true);
    }

    #region Animation Event
    public void DisableObject()
    {
        gameObject.SetActive(false);
    }

    public void LaunchProyectil(GameObject proyectil, Transform shootingPoint)
    {
        GameObject instantiateProyectil = Instantiate(proyectil, shootingPoint.position,
                                                      shootingPoint.transform.rotation);



        instantiateProyectil.GetComponent<Rigidbody2D>().AddForce(proyectil.transform.forward * AttackProyectilForce, ForceMode2D.Impulse);
    }
        



    public void StartRolling()
    {
        attackTimer = maxAttackTimer;

        _anim.SetBool("isAttacking", true);


    }

    public void ThreeProyectil()
    {
        Debug.Log("Hola");

        LaunchProyectil(bossProyectil, bossProyectilPosition);
        LaunchProyectil(bossProyectil, bossProyectilPosition1);
        LaunchProyectil(bossProyectil, bossProyectilPosition2);

    }



    #endregion
}
