using UnityEngine;

public class EnemyCrawler : MonoBehaviour
{
    [Header("REFERENCES"), SerializeField]
    Rigidbody2D _rb;
    [SerializeField]
    Renderer enemyRenderer;
    [SerializeField]
    Transform[] patrol;
    [SerializeField]
    ParticleSystem sparks;

    [SerializeField, Header("VARIABLES")]
    int currentPatrolIndex;
    [SerializeField]
    float patrolSpeed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.contactCount > 0)
        {
            sparks.transform.position = collision.GetContact(0).point;

            sparks.Play();
        }


    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            sparks.Stop();
        }
    }



    //El  Enemigo va patrullando una zona delimtada por 2 transforms
    private void Movement()
    {
        if (patrol.Length < 2) return;

        _rb.MovePosition(Vector2.MoveTowards(transform.position, patrol[currentPatrolIndex].position, patrolSpeed * Time.deltaTime));

        if (currentPatrolIndex == 1)
            transform.rotation = Quaternion.Euler(0, 180, 0);

        else if (currentPatrolIndex == 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);


        if (Vector2.Distance(transform.position, patrol[currentPatrolIndex].position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrol.Length;

            //enemySprite.flipX = patrol[currentPatrolIndex].position.x < transform.position.x;
        }
        _rb.constraints = RigidbodyConstraints2D.None;
    }
}
