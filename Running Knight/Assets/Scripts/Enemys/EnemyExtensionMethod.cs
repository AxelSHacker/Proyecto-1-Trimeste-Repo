using UnityEngine;

public static class EnemyExtensionMethod
{
    public static void Patrol(this GameObject thisEnemy, Transform[] patrol, Rigidbody2D _Rb, ref int currentPatrolIndex,
                             float patrolVelocity, float radioBusqueda, LayerMask layerMaskPlayer, Collider[] colliders)
    {
        //si la lista esta vacia, sale directamente
        if (patrol.Length < 2) return;
        //Aplicamos el movimiento 
        _Rb.MovePosition(Vector2.MoveTowards(thisEnemy.transform.position, patrol[currentPatrolIndex].position, patrolVelocity * Time.deltaTime));
        //Condiciones para que el el game object se gire hacia la dirreccion a la que se dirige
        if (currentPatrolIndex == 1)
            thisEnemy.transform.rotation = Quaternion.Euler(0, 180, 0);

        else if (currentPatrolIndex == 0)
        { thisEnemy.transform.rotation = Quaternion.Euler(0, 0, 0); }

        //Condiciones para que cuando se llegue al punto de transfor se dirija al siguiente puunto
        if (Vector2.Distance(thisEnemy.transform.position, patrol[currentPatrolIndex].position) < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrol.Length;
            //enemySprite.flipX = patrol[currentPatrolIndex].position.x < transform.position.x;
        }
        thisEnemy.BusquedadeObjetivo(radioBusqueda, layerMaskPlayer, colliders);
    }
    public static void Chase(this GameObject thisEnemy, Rigidbody2D _rb, ref float _Distance, float attackRadius, Transform playerPosition,
                            float chaseVelocity)
    {
        _Distance = Vector2.Distance(playerPosition.position, thisEnemy.transform.position);
        
        if (_Distance > attackRadius)
        {
            EnemyTourn(thisEnemy.transform, playerPosition);
            _rb.MovePosition(Vector2.MoveTowards(thisEnemy.transform.position, playerPosition.position, chaseVelocity * Time.deltaTime));
        }
    }
    public static void EnemyTourn(this Transform transform, Transform playerTransform)
    {
        if (playerTransform.position.x < transform.position.x)
            transform.rotation = Quaternion.Euler(0, 180, 0); // Mira a la izquierda
        else
            transform.rotation = Quaternion.Euler(0, 0, 0); // Mira a la derecha
    }
    public static void EnemyRotattion(this Transform transform, Transform playerTransform)
    {
        Vector3 rotating = playerTransform.position - transform.position;
        float angle = Mathf.Atan2(rotating.y, rotating.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    public static Transform BusquedadeObjetivo(this GameObject thisEnemy, float radioBusqueda, LayerMask layerMaskPlayer, Collider[] colliders)
    {
        // 1. Creamos un "radar" físico que solo detecta colliders en las capas seleccionadas
        int contactos = Physics.OverlapSphereNonAlloc(thisEnemy.transform.position, radioBusqueda, colliders, layerMaskPlayer);

        if (contactos <= 0) return null;
        Transform objetivo = null;
        // 2. Iteramos sobre los colliders detectados para encontrar el más cercano
        foreach (Collider col in colliders)
        {
            // Opcional: Si el script está en el mismo objeto que el collider, nos ignoramos a nosotros mismos
            if (col.transform == thisEnemy.transform) continue;
            // 3. Calculamos la distancia desde este objeto hasta cada objetivo detectado

            // Actualizamos la distancia mínima y el objetivo más cercano
            objetivo = col.transform;
        }
        // 5. Devolvemos el objetivo más cercano encontrado, o null si no se detectó ninguno
        Debug.Log(objetivo);
        return objetivo;
    }
}