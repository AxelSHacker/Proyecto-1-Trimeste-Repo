using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField]
    int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent<Healt>(out Healt _healt)) 
            {
                _healt.Damage(damage);
                
            }

        }
    }
}
