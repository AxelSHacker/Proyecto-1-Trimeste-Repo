using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] AudioSource _audioSource;


    //: ─── SINGLETON INSTANCE ───
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;

    private void Awake()
    {
        // Aseguramos que solo exista un AudioManager en la escena
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    //:  CONTROL DE VOLUMEN MASTER 

    /// <summary>
    /// Recupera el volumen Master actual convertido a valor lineal (0 a 1)
    /// </summary>
    public float GetMasterVolume()
    {
        bool result = _audioMixer.GetFloat("MasterVolume", out float volume);
        
        // Conversion matematica: Convierte los decibelios (dB) del mixer a escala lineal (0-1)
        volume = Mathf.Pow(10, volume / 20);
        
        if (!result)
        {
            Debug.LogWarning("Could not get MasterVolume from AudioMixer");
        }
            
        return volume;
    }

    /// <summary>
    /// Asigna el volumen Master transformando el valor lineal (0-1) a decibelios
    /// </summary>
    public void SetMasterVolume(float volume)
    {
        // 🛡️ Filtro de seguridad: Evita que un valor de 0 rompa el logaritmo generando un NaN
        if (volume <= 0) volume = 0.0001f; 
        
        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    //:  CONTROL DE VOLUMEN MUSICA 

    /// <summary>
    /// Recupera el volumen de la Musica actual convertido a valor lineal (0 a 1)
    /// </summary>
    public float GetMusicVolume()
    {
        bool result = _audioMixer.GetFloat("MusicVolume", out float volume);
        volume = Mathf.Pow(10, volume / 20);
        
        if (!result)
        {
            Debug.LogWarning("Could not get MusicVolume from AudioMixer");
        }
            
        return volume;
    }

    /// <summary>
    /// Asigna el volumen de la Musica transformando el valor lineal (0-1) a decibelios
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        if (volume <= 0) volume = 0.0001f;
        
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    //:  CONTROL DE VOLUMEN EFECTOS (SFX) 

    /// <summary>
    /// Recupera el volumen de los Efectos actual convertido a valor lineal (0 a 1)
    /// </summary>
    public float GetEffectsVolume()
    {
        bool result = _audioMixer.GetFloat("EffectsVolume", out float volume);
        volume = Mathf.Pow(10, volume / 20);
        
        if (!result)
        {
            Debug.LogWarning("Could not get EffectsVolume from AudioMixer");
        }
            
        return volume;
    }

    /// <summary>
    /// Asigna el volumen de los Efectos transformando el valor lineal (0-1) a decibelios
    /// </summary>
    public void SetEffectsVolume(float volume)
    {
        if (volume <= 0) volume = 0.0001f;
        
        _audioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);
    }
    public void ReproducirSFX(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }
}