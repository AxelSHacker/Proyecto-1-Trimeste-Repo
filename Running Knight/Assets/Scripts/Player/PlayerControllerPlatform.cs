using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControllerPlatform : MonoBehaviour, IDamageabe<int>
{
    [Header("REFERENCES"), SerializeField]
    Rigidbody2D _rB;

    public Animator _anim;
    [SerializeField] Vector2 offSet;
    [SerializeField] LayerMask Detectable;
    [SerializeField] Transform detetablePoint;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] ParticleSystem dust;
    [SerializeField] TextMeshProUGUI lifePoint;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip[] attacks;
    [SerializeField] AudioClip[] death;
    [SerializeField] AudioClip[] shieldImpact;



    [Header("MOVEMENT")]

    [SerializeField] float speed;
    [SerializeField] float speedMultiplier;
    [SerializeField] float maxSpeed;
    [SerializeField] float xMotion;
    bool isActive = false;

    [Header("JUMP"), SerializeField]
    float jumpForce;
    [SerializeField] int maxJump;
    [SerializeField] bool isGrounded;

    [Header("DASCH"), SerializeField]
    float dashForce;
    [SerializeField] int dashRepeat;
    [SerializeField] bool onDasching;

    [Header("FALLING"), SerializeField]
    float normalGravity;

    [Header("ATTACK"), SerializeField]
    int attackCount;
    [SerializeField] float timer;
    [SerializeField] bool attacking = false;

    [Header("SCHIELD"), SerializeField]
    bool schieldOn;

    [Header("POWER UP"), SerializeField]
    float invencibilityMaxTimer;
    [SerializeField] float speedUpMaxTimer;
    [SerializeField] float invencibilityTimer = 0;
    [SerializeField] float speedUpTimer = 0;
    GameObject collisionObject;
    [SerializeField] ParticleSystem fairy;
    [SerializeField] ParticleSystem speedUp;
    [SerializeField] float speedUpVelocity;
    [Header("CORRUTINA"), SerializeField]
    private Coroutine colorFlaschCoroutine;
    [SerializeField] int _vidaActual;
    [SerializeField] int _vidaMaxima;
    public int Maxhealt => _vidaMaxima;
    public int Currentealt => _vidaActual;
    public bool IsDead => _vidaActual <= 0;

    void Start()
    {
        GameManager.Instance.AlphaCanvas(GameManager.Instance.canvasGroup, 1, false);
        GameManager.Instance.AlphaCanvas(GameManager.Instance.endGameCanvasGroup, 0, false);
        GameManager.Instance.AlphaCanvas(GameManager.Instance.continueCanvasGroup, 0, false);
        normalGravity = _rB.gravityScale;
        lifePoint.text = DataManager.Instance.actualGameScore.ToString();
        _vidaActual = DataManager.Instance.actualGameScore;
        MusicManager.Instance.PlayRandomSong();
    }
    void Update()
    {


        // if (invencibilityTimer > 0)
        // {
        //     invencibilityTimer -= Time.deltaTime;
        //     Invincibility(collisionObject);
        // }
        if (speedUpTimer > 0)
        {
            _rB.linearVelocityX = _rB.linearVelocityX + speedUpVelocity;
        }


        AnimatorController();
        GroundCheck();
        DeathFalling();

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
        if (collision.gameObject.CompareTag("Potion"))
        {
            _vidaActual += 5;
        }
        else if (collision.gameObject.CompareTag("CheckPoint"))
        {
            GameManager.Instance.continueCanvasGroup.SetEnable(true);
        }
        else if (collision.gameObject.CompareTag("TerminarTutorial"))
        {
            GameManager.Instance.EndGame();
        }
    }
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
            onDasching = true;
            Dash();
        }
        else if (context.canceled)
        {
            onDasching = false;
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
            schieldOn = false;
        }
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isActive = !isActive;
            GameManager.Instance.Pause(isActive);
        }
    }
    #endregion






    #region Funciones 
    private void PlatformMovement()
    {
        if (onDasching || schieldOn) return;
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

        _anim.SetBool("SchieldOn", schieldOn);
    }
    //Funcion que aumenta la gravedad cuiando pulsamos una tecla
    //Funcion para controlar la muerte de el player
    private void Death()
    {
        //Activamos la animacion de muerte
        _anim.SetBool("Death", true);
        //se instancia el panel de derrota
        GameManager.Instance.EndGame();
    }
    private void Dash()
    {
        if (dashRepeat >= 0 && !isGrounded)
        {
            dashRepeat--;
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
    private void Falling()
    {
        if (!isGrounded)
        {
            _rB.gravityScale = 5f;
        }
    }
    private void DeathFalling()
    {
        if (transform.position.y < -5.5f)
        {
            GameManager.Instance.EndGame();
            _vidaActual = 0;
        }
    }
    //Funcion de control de Ataques
    private void Attack()
    {
        int randomIndex = UnityEngine.Random.Range(0, attacks.Length);
        if (attackCount == 0)
        {
            attackCount++;
            _anim.SetTrigger("Attacking");
            _anim.SetInteger("Combo", attackCount);
            MusicManager.Instance.SFXPlayer(attacks[randomIndex]);


        }
        else if (attacking)
        {

            attackCount++;
            _anim.SetInteger("Combo", attackCount);
            _anim.SetTrigger("Attacking");
            attacking = false;
            MusicManager.Instance.SFXPlayer(attacks[randomIndex]);
        }
    }
    private void SchieldOn()
    {
        schieldOn = true;
        _rB.linearVelocity = Vector2.zero;
    }
    public void InvincibleButton()
    {
        _vidaActual = 100;
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
    // public void Invincibility(GameObject gameObject)
    // {
    //     Destroy(gameObject);
    // }
    public void TakeDamag(int damage, Vector3 impactPoint = default)
    {
        if (schieldOn) return;
        _vidaActual -= damage;
        lifePoint.text = _vidaActual.ToString();
        _vidaActual = Mathf.Clamp(_vidaActual, 0, _vidaMaxima);

        if (_vidaActual <= 0)
        {
            Death();
        }
    }
    #endregion


}































































