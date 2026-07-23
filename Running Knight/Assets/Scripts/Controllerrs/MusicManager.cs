using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip menuClip;
    [SerializeField] AudioClip[] gameClip;

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

    }
    public void PlayMainMenuMusic()
    {
        if (audioSource.clip == menuClip) return;
        audioSource.clip = menuClip;
        audioSource.Play();
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeAndChangeClip(menuClip));
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
    public void PlayRandomSong()
    {
        if (gameClip.Length == 0) return;

        // Elegir canción aleatoria
        AudioClip clip = gameClip[Random.Range(0, gameClip.Length)];
        audioSource.clip = clip;
        audioSource.Play();

        // Iniciar coroutine
        StartCoroutine(WaitForSongToEnd());
    }
    public void SFXPlayer(AudioClip audioClip)
    {
        sfxSource.PlayOneShot(audioClip);
    }
    private IEnumerator FadeAndChangeClip(AudioClip clip)
    {
        //Usamos la mitad del tiempo indicado prque tenemos que hacer salida y entrada
        float counter = fadeTZime / 2f;
        while (counter > 0f)
        {
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
            audioSource.pitch = Mathf.Lerp(current, target, count / pitchTTiime);
            count += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator WaitForSongToEnd()
    {
        // Esperar mientras se está reproduciendo
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        // Cuando termine, reproducir otra aleatoria
        PlayRandomSong();
    }
}
