using UnityEngine;
using System.Collections.Generic;
public class DamageDealer : MonoBehaviour
{
    [SerializeField]
    int damage;


    private void OnTriggerEnter2D(Collider2D other)
    {

        // 2. Buscar si el objeto (o su padre) tiene la interfaz de daño
        if (other.TryGetComponent(out IDamageabe<int> iDamageable))
        {
            iDamageable.TakeDamag(damage);
        }
    }
}
