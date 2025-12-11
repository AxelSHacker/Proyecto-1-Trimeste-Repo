using System.Collections;

using UnityEngine;


public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource sfxSource;
    [SerializeField] AudioClip menuClip;
    [SerializeField] AudioClip gameClip;

    [SerializeField, Range(1, 3)] float fadeTZime = 2f;
    [SerializeField, Range(0f, 2f)] float pitchTTiime = 1f;
    //Valor minimo del pitch para cambbiar el sonido al mostrar el menu de fin de partida
    [SerializeField] float pitchSlow = 0.6f;


    Coroutine fadeCoroutine;
    Coroutine PitchCoroutine;

    private static MusicManager _instance;
    public static MusicManager Instance => _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //Metodo que indica un gamobject que se debe ser conservado al dscarga la escena
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        audioSource.panStereo = 0;
        audioSource.volume = DataManager.Instance.musicVolumen;
        sfxSource.volume = DataManager.Instance.sfxVolumen;

    }


    void Update()
    {




    }
    public void PlayMainMenuMusic()
    {
        if (audioSource.clip == menuClip) return;
        audioSource.clip = menuClip;
        audioSource.Play();
        //if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        //Iniciamos Coroutine
        //fadeCoroutine = StartCoroutine(FadeAndChangeClip(menuClip));

    }

    public void PlayGameMusic()
    {
        if (audioSource.clip == gameClip) return;

        audioSource.clip = gameClip;
        audioSource.Play();
        //if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        //Iniciamos Coroutine
        //fadeCoroutine = StartCoroutine(FadeAndChangeClip(gameClip));
    }

    public void PitchSlow()
    {
        if (PitchCoroutine != null) StopCoroutine(PitchCoroutine);
        PitchCoroutine = StartCoroutine(PitchChange(true));
    }

    public void PitchRegular()
    {
        if (PitchCoroutine != null) StopCoroutine(PitchCoroutine);
        PitchCoroutine = StartCoroutine(PitchChange(false));
    }

    public void SFXPlayer(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
    private IEnumerator FadeAndChangeClip(AudioClip clip)
    {
        //Usamos la mitad del tiempo indicado prque tenemos que hacer salida y entrada
        float counter = fadeTZime / 2f;
        while (counter > 0f)
        {
            audioSource.volume = DataManager.Instance.musicVolumen;
            audioSource.volume = counter / (fadeTZime / 2f);
            counter -= Time.deltaTime;


        }
        //Cambiamos l clip
        audioSource.clip = clip;
        //Reproducimos l clip nuevo;
        audioSource.Play();
        counter = 0f;

        while (counter < (fadeTZime / 2f))
        {
            audioSource.volume = DataManager.Instance.musicVolumen;
            audioSource.volume = counter / (fadeTZime / 2f);
            counter += Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator PitchChange(bool slow)
    {
        float target = slow ? pitchSlow : 1f;
        float current = audioSource.pitch;
        float count = 0f;

        while (count < pitchTTiime)
        {
            audioSource.panStereo = Mathf.Lerp(current, target, count / pitchTTiime);
            count += Time.deltaTime;
            yield return null;
        }
    }
}
