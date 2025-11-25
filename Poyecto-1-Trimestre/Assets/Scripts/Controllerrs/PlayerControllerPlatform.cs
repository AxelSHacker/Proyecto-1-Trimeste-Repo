using System;
using System.Collections;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControllerPlatform : MonoBehaviour
{



    [Header("REFERENCES"), SerializeField]
    Rigidbody2D _rB;
    [SerializeField]
    Animator _anim;
    [SerializeField]
    Vector2 offSet;
    [SerializeField]
    LayerMask Detectable;
    [SerializeField]
    Transform detetablePoint;
    [SerializeField]
    SpriteRenderer _spriteRenderer;
    [SerializeField]
    ParticleSystem dust;



    [Header("MOVEMENT")]

    [SerializeField]
    float speed;
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float xMotion;

    [Header("JUMP"), SerializeField]
    float jumpForce;
    [SerializeField]
    int maxJump;
    [SerializeField]
    bool isGrounded;

    [Header("DASCH"), SerializeField]
    float dashForce;
    [SerializeField]
    int dashRepeat;

    [Header("FALLING"), SerializeField]
    float normalGravity;

    [Header("ATTACK"), SerializeField]
    int attackCount;
    [SerializeField]
    float timer;
    [SerializeField]
    bool attacking = false;
    [Header("POWER UP"), SerializeField]
    float powerUpTimer;
    [SerializeField]
    float maxTimer = 0;
    GameObject collisionObject;
    [SerializeField]
    ParticleSystem fairy;
    [SerializeField]
    ParticleSystem speedUp;
    [SerializeField]
    float speedUpVelocity;
    [SerializeField]
    bool canDie = true;
    [Header("CORRUTINA"), SerializeField]
    private Coroutine colorFlaschCoroutine;
    void Start()
    {
        normalGravity = _rB.gravityScale;


    }
    void Update()
    {

        maxTimer -= Time.deltaTime;
        AnimatorController();
        GroundCheck();
        DeathFalling();
        if (maxTimer > 0)
        {
            if (!canDie) { GameManager.Instance.Invincibility(collisionObject); }
            else { _rB.linearVelocityX = _rB.linearVelocityX + speedUpVelocity; }
        }
    }
    void FixedUpdate()
    {
        PlatformMovement();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(detetablePoint.position, offSet);
    }
    //Trigger para controllar las colisiones
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Invencibility"))
        {
            canDie = false;
            Destroy(collision.gameObject);
            maxTimer = powerUpTimer;
            fairy.Play();

        }

        if (collision.gameObject.CompareTag("Speed Up"))
        {
            Destroy(collision.gameObject);
            maxTimer = powerUpTimer;
            speedUp.Play();
            canDie = true;

        }
    }
    //Collision para manejar la colisiones
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

            collisionObject = collision.gameObject;
            if (canDie) Death();
        }
    }
    /// <summary>
    /// Funciones para asignar mediante el New Impu System
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            _anim.SetTrigger("Jumping");
            Jump();
        }
    }
    public void OnFalling(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Falling();
        }
        else if (context.canceled)
        {
            _rB.gravityScale = normalGravity;
        }
    }
    public void OnDasching(InputAction.CallbackContext context)
    {
        if (context.started)
        {

            Dash();
        }
    }
    public void OnAttacking(InputAction.CallbackContext context)
    {
        if (context.started)
        {

            Attack();

        }

    }
    public void OnMovement(InputAction.CallbackContext context)

    {
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            xMotion = input.x;
        }
        if (context.canceled)
        {
            xMotion = 0f;
        }
    }
    //Funcion que controla el movimiento y la rotacion del personaje
    private void PlatformMovement()
    {
        //Aqui le indicamos que se mueva al rigidbody
        _rB.AddForceX(xMotion * maxSpeed, ForceMode2D.Force);
        //Velocidad maxima del desplazamiento del player
        if (_rB.linearVelocityX >= maxSpeed)
        {
            _rB.linearVelocityX = maxSpeed;
        }

        //Codigo para que el personaje mire hacia un lugar u otro segundo a donde se dirija
        if (xMotion < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (xMotion > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    //Funcion que controla la fuerza de salto , el numero de saltos
    private void Jump()
    {
        if (maxJump <= 0)
        {
            _rB.linearVelocityY = 0f;
            maxJump++;
            _rB.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    //Funcion que comprueba si estamos tocando el suelo
    private void GroundCheck()
    {

        if (Physics2D.OverlapBox(detetablePoint.position, offSet, 0, Detectable))
        {
            isGrounded = true;
            maxJump = 0;

        }
        else
        {
            isGrounded = false;

        }
    }
    //Controlador de las animaciones
    private void AnimatorController()
    {
        float velocity = Math.Abs(_rB.linearVelocityX);
        _anim.SetFloat("Velocity", velocity);

        if (!isGrounded)
        {

            _anim.SetBool("OnAir", true);
        }
        else
        {
            _anim.SetBool("OnAir", false);
        }


    }
    //Funcion que aumenta la gravedad cuiando pulsamos una tecla
    private void Falling()
    {
        if (!isGrounded)
        {
            _rB.gravityScale = 5f;
        }
    }
    //Funcion para controlar la muerte de el player
    private void Death()
    {
        //Activamos la animacion de muerte
        _anim.SetBool("Death", true);
        //se instancia el panel de derrota
        Invoke("EndGame", 1.5f);
    }
    private void DeathFalling()
    {
        if (transform.position.y < -5.8f)
        {
            Death();
        }
    }
    private void EndGame()
    {
        GameManager.Instance.EndGame();
    }
    /// <summary>
    /// Asigna el colo del flash e inicia la corutina de recuperacion del color
    /// en el tiempo indicado
    /// </summary>
    /// <param name="color"></param>
    /// <param name="time"></param>
    public void StartColorFlash(Color color, float time)
    {
        //Si ya hay una corutina funcionando , la paro
        if (colorFlaschCoroutine != null)
        {
            StopCoroutine(colorFlaschCoroutine);
        }
        //Primero asignamos el color entrante al sprite renderer
        _spriteRenderer.color = color;
        //Iniciamos de nuevo la corrutina
        colorFlaschCoroutine = StartCoroutine(ColorRecover(time));
    }
    private IEnumerator ColorRecover(float time)
    {
        //Inicialiamos el contado de tiempo
        float timeCounter = 0f;
        //Almacenamos el color inicial
        Color initialColor = _spriteRenderer.color;
        //Mediante un bbucle while hacemos el cambio de colo a la vey que contamos el tiempo que pasa
        while (timeCounter < time)
        {
            _spriteRenderer.color = Color.Lerp(initialColor, Color.green, timeCounter / time);
            timeCounter += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _spriteRenderer.color = Color.white;

    }
    //Funcion de control de el Dash
    private void Dash()
    {
        if (dashRepeat <= 1)
        {

            if (transform.rotation.y == 0)
            {
                _rB.AddForceX(dashForce, ForceMode2D.Impulse);
            }
            else
            {
                _rB.AddForceX(-dashForce, ForceMode2D.Impulse);
            }


        }
    }
    //Funcion de control de Ataques
    private void Attack()
    {
        if (attackCount == 0)
        {
            attackCount++;
            _anim.SetTrigger("Attacking");
            _anim.SetInteger("Combo", attackCount);
        }

        if (attacking)
        {
            attackCount++;
            _anim.SetInteger("Combo", attackCount);
            _anim.SetTrigger("Attacking");
            attacking = false;
        }



    }
    /// <summary> Animation Events
    /// Animation Events
    /// </summary>
    public void EnableCombo()
    {
        attacking = true;
    }
    public void ResetCombo()
    {
        attackCount = 0;
        _anim.SetInteger("Combo", attackCount);
        attacking = false;
    }
    public void DustEffect()
    {
        dust.Play();

    }
        

        

}































































