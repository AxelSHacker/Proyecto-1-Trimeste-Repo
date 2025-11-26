using UnityEngine;

public class Healt : MonoBehaviour
{
    private static Healt _instance;

    public static Healt Instance => _instance;
    public int health;
    public int maxHealt;

    //[SerializeField]
    // AudioSource aS;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        
    }
    private void Start()
    {
        health = maxHealt;

       
    }
    public void Damage(int damage)
    {
        health -= damage;
        //Sonido de espada
    }
        
       
           
            
}
