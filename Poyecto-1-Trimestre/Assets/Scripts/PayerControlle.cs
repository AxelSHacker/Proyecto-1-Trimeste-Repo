using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PayerControlle : MonoBehaviour
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
    Transform panza;
    [SerializeField]
    SpriteRenderer _spriteRenderer;

    [Header("MOVEMENT"), SerializeField]
    float speed;
    [SerializeField]
    float maxSpeed;

    [Header("JUMP"), SerializeField]
    float jumpForce;
    [SerializeField]
    int maxJump;
    [SerializeField]
    bool isGrounded;
    [SerializeField]
    float wallContact;

    [Header("Dash"), SerializeField]
    float dashForce;
    [SerializeField]
    int dashRepeat;

    [Header("FALLING"), SerializeField]
    float normalGravity;

    [Header("PLAYERSPAWN")]
    Transform playerSpawn;

    [Header("CORRUTINA"), SerializeField]
    private Coroutine colorFlaschCoroutine;

    
    void Start()
    {
        normalGravity = _rB.gravityScale;
        playerSpawn = transform;
    }

    
    void Update()
    {
        AnimatorController();
        Movement();
        WallSlide();
        GroundCheck();
        PlayerSpawn();
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(detetablePoint.position, offSet);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(panza.transform.position, Vector2.up * wallContact);
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
//Funcion de movimiento
    private void Movement()
    {
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
    //Para que cuando choque contra una pared la fuerza no lo presione contra el colliser , si no que,
    // se deslice 
    private void WallSlide()
    {


        RaycastHit2D hit = Physics2D.Raycast(panza.transform.position, Vector2.up, wallContact);


        if (hit)
        {

            _rB.linearVelocityX = Vector2.zero.x;

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
    
    private void Dash()
    {
        if (dashRepeat <= 1)
        {
            float actualVelocity = _rB.linearVelocityX;;
            _rB.AddForce(transform.right * dashForce, ForceMode2D.Impulse);
            Debug.Log("CAraculo");
        }
    }

    private void PlayerSpawn()
    {
        if (transform.position.y <= -8f)
        {
            transform.position = playerSpawn.position;
        }
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




}
