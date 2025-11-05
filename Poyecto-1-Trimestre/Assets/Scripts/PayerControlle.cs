using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PayerControlle : MonoBehaviour
{

    [Header("REFERENCES"), SerializeField]
    Rigidbody2D _rB;
    [SerializeField]
    Animator _anim;
    [SerializeField]
    SpriteRenderer playerSprite;
    [SerializeField]
    Vector2 offSet;
    [SerializeField]
    LayerMask Detectable;
    [SerializeField]
    Transform detetablePoint;

    [Header("MOVEMENT"), SerializeField]
    float speed;
    [SerializeField]
    float maxSpeed;

    [Header("JUMP"), SerializeField]
    int jumpForce;
    [SerializeField]
    int maxJump;
    [SerializeField]
    bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorController();
        Movement();
        GroundCheck();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(detetablePoint.position, offSet);
    }
    public void OnJump(InputAction.CallbackContext context)

    {
        if (context.started)
        {
            Jump();
        }


    }


    private void Movement()
    {
        _rB.linearVelocityX += speed;

        if (_rB.linearVelocityX >= maxSpeed)
        {
            _rB.linearVelocityX = maxSpeed;
        }
    }

    private void Jump()
    {
        if (maxJump <= 0)
        {
            
            maxJump++;
            _rB.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }


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

    private void AnimatorController()
    {
        float velocity = Math.Abs(_rB.linearVelocityX);
        _anim.SetFloat("Velocity", velocity);

        if (!isGrounded)
        {
            _anim.SetBool("Jump", true);
            _anim.SetBool("OnAir", true);
        }
        else
        {
            _anim.SetBool("Jump", false);
        }

        
    }
}
