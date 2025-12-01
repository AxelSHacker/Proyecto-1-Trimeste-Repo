using UnityEngine;

public class SchieldScript : MonoBehaviour
{
    #region References
    [Header("References"), SerializeField]

    Animator _anim;
    #endregion






    #region OnTrigger/OnCollider
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Proyectil") || collision.gameObject.CompareTag("Sword"))
        {
            _anim.SetTrigger("Impact");
        }
        
    }





    #endregion
}
