using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System;





public class PlayerControllerEndLess : MonoBehaviour
{

    [Header("REFERENCES"), SerializeField]
    Rigidbody2D _rB;
    [SerializeField] Animator _anim;
    [SerializeField] Vector2 offSet;
    [SerializeField] LayerMask Detectable;
    [SerializeField] Transform detetablePoint;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] ParticleSystem dust;

    [Header("MOVEMENT")]
    public bool autoMovement = true;
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;

    [Header("JUMP"), SerializeField]
    float jumpForce;
    [SerializeField] int maxJump;
    [SerializeField] bool isGrounded;

    [Header("FALLING"), SerializeField]
    float normalGravity;

    [Header("POWER UP"), SerializeField]
    float invencibilityMaxTimer;
    [SerializeField] float speedUpMaxTimer;
    [SerializeField] float invencibilityTimer = 0;
    [SerializeField] float speedUpTimer = 0;
    GameObject collisionObject;
    [SerializeField] ParticleSystem fairy;
    [SerializeField] ParticleSystem speedUp;
    [SerializeField] float speedUpVelocity;
    public bool canDie = true;
    [Header("SOUNDS"), SerializeField]
    AudioClip jump;
    [SerializeField] AudioClip[] death;
    [SerializeField] AudioClip landing;
    [SerializeField] AudioClip powerUp;


    [Header("CORRUTINA"), SerializeField]
    private Coroutine colorFlaschCoroutine;
    void Start()
    {
        normalGravity = _rB.gravityScale;
        GameManager.Instance.AlphaCanvas(GameManager.Instance.canvasGroup, 1, false);
    }
    void Update()
    {
        AnimatorController();
        GroundCheck();
        DeathFalling();
        if (invencibilityTimer > 0)
        {
            invencibilityTimer -= Time.deltaTime;
            Invincibility(collisionObject);
        }
        else
        {
            canDie = true;
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
    #region OnTrigger/OnCollision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Invencibility"))
        {
            canDie = false;
            Destroy(collision.gameObject);
            invencibilityTimer = invencibilityMaxTimer;
            fairy.Play();
        }
        if (collision.gameObject.CompareTag("Speed Up"))
        {
            Destroy(collision.gameObject);
            speedUpTimer = speedUpMaxTimer;
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
    #endregion





    #region New Input System
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
    #endregion




    #region Funciones 

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
            MusicManager.Instance.SFXPlayer(jump);
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
        float velocity = MathF.Abs(_rB.linearVelocityX);
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
        int randomIndex = UnityEngine.Random.Range(0, death.Length);
        //Paramos el movimiento automaico del player
        autoMovement = false;
        //Activamos la animacion de muerte
        _anim.SetBool("Death", true);
        if (!MusicManager.Instance.audioSource.isPlaying) MusicManager.Instance.SFXPlayer(death[randomIndex]);
        GameManager.Instance.ContinueGame();
    }
    private void DeathFalling()
    {
        if (transform.position.y < -5.8f)
        {
            int randomIndex = UnityEngine.Random.Range(0, death.Length);

            if (!MusicManager.Instance.audioSource.isPlaying) MusicManager.Instance.SFXPlayer(death[randomIndex]);

            GameManager.Instance.EndGame();
            autoMovement = false;
        }
    }
    public void InvincibleButton()
    {
        canDie = !canDie;
    }
    public void SpeedUpButton()
    {
        speedUpTimer = 5;
    }
    #endregion




    #region Couroutine
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
    #endregion





    #region Animation Events
    public void DustEffect()
    {
        dust.Play();

    }
    #endregion





    #region Power Up
    public void Invincibility(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    #endregion
}
































