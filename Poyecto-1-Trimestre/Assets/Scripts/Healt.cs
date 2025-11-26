using UnityEngine;

public class Healt : MonoBehaviour
{
  
    public int health;
    public int maxHealt;
    public bool noDamage = false;

    
    private void Start()
    {
        health = maxHealt;


    }
    public void Damage(int damage)
    {
        if (noDamage) return;

        health -= damage;
        //Sonido de espada
    }




}
