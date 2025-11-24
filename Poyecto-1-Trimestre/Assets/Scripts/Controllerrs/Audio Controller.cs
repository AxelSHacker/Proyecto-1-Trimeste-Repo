using UnityEngine;

public class AudioController : MonoBehaviour
{
    private static AudioController _instance;

    public static AudioController Instance => _instance;

    public AudioSource audioSource;
    public AudioClip jump;
    public AudioClip pickUpCoin;
    public AudioClip spaceShipMovement;
    public AudioClip swordSwing1;
    public AudioClip swordSwing2;
    public AudioClip swordSwing3
    ;

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SoundsPlayer(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
