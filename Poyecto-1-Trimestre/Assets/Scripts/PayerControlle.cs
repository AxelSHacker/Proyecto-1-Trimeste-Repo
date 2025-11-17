using System;
using System.Collections;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public class PayerControlle : MonoBehaviour
{

    public enum State
    {
        EndLessRunner,

        Platform,
    }

    [Header("REFERENCES"), SerializeField]
    Rigidbody2D _rB;
    [SerializeField]
    Animator _anim;
    [SerializeField]
    Vector2 offSet;
    [SerializeField]
    LayerMask Detectable;
    [SerializeField]
    LayerMask noDetectables;
    [SerializeField]
    Transform detetablePoint;

    [SerializeField]
    Transform panza;
    [SerializeField]
    SpriteRenderer _spriteRenderer;

    [Header("MOVEMENT")]
    public bool autoMovement = true;
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
    [SerializeField]
    float wallContact;

    [Header("DASCH"), SerializeField]
    float dashForce;
    [SerializeField]
    int dashRepeat;

    [Header("FALLING"), SerializeField]
    float normalGravity;

    [Header("ATTACK"), SerializeField]
    int attackCount = 1;
    [SerializeField]
    float timer;
    [SerializeField]
    bool atttacking = false;

    [Header("CORRUTINA"), SerializeField]
    private Coroutine colorFlaschCoroutine;


    void Start()
    {
        normalGravity = _rB.gravityScale;

    }


    void Update()
    {
        AnimatorController();
        GroundCheck();

        WallSlide();




    }
    void FixedUpdate()
    {
        if (attackCount >= 0)
        {
            timer += Time.deltaTime;
            if (timer >= 2)
            {
                attackCount = 0;
                _anim.SetInteger("Combo", 0);
            }

        }


        if (autoMovement)
        {
            UpdateState(State.EndLessRunner);
        }
        else
        {

            UpdateState(State.Platform);
        }


    }
    private void UpdateState(State actualState)
    {
        switch (actualState)
        {
            case State.EndLessRunner:
                Movement();
                Dash();
                break;
            case State.Platform:

                PlatformMovement();

                break;
        }
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(detetablePoint.position, offSet);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(panza.transform.position, Vector2.up * wallContact);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Death();
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
    //Para que cuando choque contra una pared la fuerza no lo presione contra el colliser , si no que,
    // se deslice 
    private void WallSlide()
    {
        RaycastHit2D hit = Physics2D.Raycast(panza.transform.position, Vector2.up, wallContact, noDetectables);


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
    //Funcion para controlar la muerte de el player
    private void Death()
    {
        autoMovement = false;
        //Desactivamos el movimiento
        _rB.linearVelocity = Vector2.zero;
        //Activamos la animacion de muerte
        _anim.SetBool("Death", true);
        //se instancia el panel de derrota
        Invoke("EndGame", 1.5f);
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

    /// FUNCIONES PARA LA FASE PLATFORM

    private void PlatformMovement()
    {
        if (!autoMovement)
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
    }

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

    private void Attack()
    {
        if (attackCount == 0)
        {
            attackCount = 1;
            _anim.SetTrigger("Attacking");
            _anim.SetInteger("Combo", attackCount);
        }
        

        


    }
}






