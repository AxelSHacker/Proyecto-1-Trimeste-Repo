using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;




public class PlayerControllerEndLess : MonoBehaviour
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
    public bool autoMovement = true;
    [SerializeField]
    float speed;
    [SerializeField]
    float maxSpeed;

    [Header("JUMP"), SerializeField]
    float jumpForce;
    [SerializeField]
    int maxJump;
    [SerializeField]
    bool isGrounded;

    [Header("FALLING"), SerializeField]
    float normalGravity;

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
        Movement();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(detetablePoint.position, offSet);
    }
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
            canDie = true;
            Destroy(collision.gameObject);
            maxTimer = powerUpTimer;
            speedUp.Play();

        }
    }
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
    //Funcion de movimiento
    private void Movement()
    {
        if (!autoMovement) return;
        _rB.AddForce(transform.right * speed, ForceMode2D.Force);

        if (_rB.linearVelocityX >= maxSpeed)
        {
            _rB.linearVelocityX = maxSpeed;
        }

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
        //Paramos el movimiento automaico del player

        autoMovement = false;
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

    public void DustEffect()
    {
        dust.Play();

    }
}




   
   













    












