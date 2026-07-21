using UnityEngine;

public class ScriptSchield : MonoBehaviour
{
    #region References
    [Header("References"), SerializeField]
    Animator _anim;
    [SerializeField] AudioClip[] shieldImpact;

    #endregion






    #region OnTrigger/OnCollider
    void OnTriggerEnter2D(Collider2D other)
    {
        int randomIndex = UnityEngine.Random.Range(0, shieldImpact.Length);
        
        if (other.gameObject.CompareTag("Proyectil") || other.gameObject.CompareTag("Sword"))
        {
            _anim.SetTrigger("Impact");
            MusicManager.Instance.SFXPlayer(shieldImpact[randomIndex]);
            if (other.gameObject.CompareTag("Proyectil"))
            {
                Destroy(other.gameObject);
            }

        }

    }







    #endregion
}
