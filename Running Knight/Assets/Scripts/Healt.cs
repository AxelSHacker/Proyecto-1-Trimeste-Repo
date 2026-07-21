using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Healt : MonoBehaviour
{
    public int enemyHealth;
    public int health;
    public int enemysMaxHealt;
    public bool noDamage = false;


    private void Awake()
    {
        enemyHealth = enemysMaxHealt;
    }


    public void Damage(int damage)
    {
        if (noDamage) return;
        if (health >= 0)
        {
            health -= damage;

        }
        if (enemyHealth >= 0)
        {
            enemyHealth -= damage;
        }
    }




}
