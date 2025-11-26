using UnityEngine;

public class SpaceShipPlatform : MonoBehaviour
{
    #region Referencias 
    [Header("References"), SerializeField]
    Rigidbody2D shipRB;
    #endregion
    #region Variables
    [SerializeField]
    float velocity;
    [SerializeField]
    float maxVelocity;
    [SerializeField]
    bool playerCheck;
    #endregion
    #region Funciones
    private void FixedUpdate()
    {
        Movement();
    }
    private void Movement()
    {
        if (playerCheck) return;

        shipRB.AddForceX(velocity, ForceMode2D.Force);


    }
    #endregion


}
