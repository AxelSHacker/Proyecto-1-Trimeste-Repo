using System;
using System.Collections;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControllerPlatform : MonoBehaviour
{



    [Header("REFERENCES"), SerializeField]
    Rigidbody2D _rB;

    public Animator _anim;
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
    [SerializeField]
    Healt _healt;
    [SerializeField]
    TextMeshProUGUI lifePoint;



    [Header("MOVEMENT")]

    [SerializeField]
    float speed;
    [SerializeField]
    float speedMultiplier;
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

    [Header("SCHIELD"), SerializeField]
    bool schieldOn;
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
        if (_healt.health < 0)
        {
            Death();
        }

        maxTimer -= Time.deltaTime;
        AnimatorController();
        GroundCheck();
        DeathFalling();
        SliderController();
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
    #region Trigger/Collider
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


        if (collision.gameObject.CompareTag("Proyectil") || collision.gameObject.CompareTag("Sword"))
        {
            if (collision.gameObject.CompareTag("Proyectil"))
            {
                Destroy(collision.gameObject);
            }
            _anim.SetTrigger("Impact");
        }
    }
    //Collision para manejar la colisiones

    #endregion






    #region New Impu System
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
    public void OnSchield(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SchieldOn();
        }
        if (context.canceled)
        {
            _healt.noDamage = false;
        }
    }
    #endregion






    #region Funciones 
    private void PlatformMovement()
    {
        if (_healt.noDamage) return;
        //Aqui le indicamos que se mueva al rigidbody
        _rB.linearVelocityX = xMotion * speed * speedMultiplier * Time.deltaTime;
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
            dashRepeat = 2;
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

        _anim.SetBool("SchieldOn", _healt.noDamage);
    }
    //Funcion que aumenta la gravedad cuiando pulsamos una tecla
    //Funcion para controlar la muerte de el player
    private void Death()
    {
        //Activamos la animacion de muerte
        _anim.SetBool("Death", true);
        //se instancia el panel de derrota
        Invoke("EndGame", 1.5f);
    }
    private void Dash()
    {
        if (dashRepeat >= 0 && !isGrounded)
        {
            dashRepeat--;
            if (transform.rotation.y == 0)
            {
                Debug.Log("si entra");
                _rB.AddForceX(dashForce, ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("si entra");
                _rB.AddForceX(-dashForce, ForceMode2D.Impulse);
            }


        }
    }

    private void Falling()
    {
        if (!isGrounded)
        {
            _rB.gravityScale = 5f;
        }
    }

    private void DeathFalling()
    {
        if (transform.position.y < -5.8f)
        {
            EndGame();
            _healt.health = 0;
        }
    }

    private void EndGame()
    {
        GameManager.Instance.EndGame();
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

    private void SchieldOn()
    {
        _healt.noDamage = true;
        _rB.linearVelocity = Vector2.zero;


    }

    private void SliderController()
    {

        lifePoint.text = _healt.health.ToString();
    }
    #endregion






    #region Courrotine
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







    #region Animation Event
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
    #endregion



}































































