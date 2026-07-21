using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


using USceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
   [Header("UI Transition References")]
    // Controla la opacidad y la interacción de todo el Canvas de carga (Fondo negro, RawImage, textos, etc.)
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] VideoPlayer _LoadingVideo;
    // Cuánto tiempo tarda en ponerse la pantalla completamente negra o transparente
    [SerializeField] float _fadeDuration = 1f;

    [Header("Scene Settings")]
    // Escena que se cargará automáticamente al arrancar el juego (por defecto, el Menú Principal)
    [SerializeField] string _initialSceneName = "MainMenu";

    // Banderas de control de estado (Booleans)
    bool _isFading;  // True si la pantalla está cambiando de opacidad en este momento
    bool _isLoading; // True si hay un proceso de carga de mapa activo en segundo plano

    private static SceneManager _instance;
    public static SceneManager Instance => _instance;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    void Start()
    {
        LoadScene(_initialSceneName, false);
    }
    // Multiplica de forma suave el Alfa del CanvasGroup desde un valor inicial ('from') hasta uno final ('to')
    private IEnumerator Fade(float from, float to)
    {
        _isFading = true;
        _canvasGroup.blocksRaycasts = true;

        float timeCounter = _fadeDuration;
        while (timeCounter > 0)
        {
            // Calculamos un porcentaje 't' que va de 0.0 a 1.0 a medida que el tiempo se agota
            float t = 1 - (timeCounter / _fadeDuration);

            // Interpolación lineal del alfa basándonos en el tiempo transcurrido
            _canvasGroup.alpha = Mathf.Lerp(from, to, t);

            // Restamos el tiempo del frame anterior al contador
            timeCounter -= Time.deltaTime;
            yield return null; // Esperamos al siguiente frame
        }

        // Al salir del bucle forzamos el valor exacto de destino para corregir decimales flotantes
        _canvasGroup.alpha = to;

        // Si el alfa ha llegado a cero (pantalla totalmente invisible), liberamos el bloqueo de clics
        // para que el jugador pueda interactuar con el nuevo mapa cargado.
        if (_canvasGroup.alpha == 0)
        {
            _canvasGroup.blocksRaycasts = false;
        }

        _isFading = false;
    }
    // --- CARGADOR ASÍNCRONO DE ESCENAS ---
    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        // Control de seguridad: Si no se ha puesto nombre de escena, abortamos para evitar un crash
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("El nombre de la escena está vacío o es nulo.");
            yield break;
        }
        // Carga la escena de forma asíncrona (en la sombra) en modo Aditivo (sin destruir la escena Core actual)
        yield return USceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Buscamos la última escena que ha sido cargada en la lista interna de Unity
        Scene newLoadedScene = USceneManager.GetSceneAt(USceneManager.sceneCount - 1);

        // 📢 ¡MUY IMPORTANTE!: Le decimos a Unity que la nueva escena es la ACTIVA.
        // Esto hace que cualquier objeto que instancies (Instantiate) a partir de ahora nazca dentro de este nuevo mapa.
        USceneManager.SetActiveScene(newLoadedScene);
    }
    // --- FLUJO MAESTRO DE CAMBIO DE MAPA ---
    private IEnumerator FadeAndLoadScene(string sceneName, bool unloadActive)
    {
        _isLoading = true;
        // 1. Ponemos la pantalla en negro (Fade de 0 a 1). Aquí aparece tu RawImage y la carga en pantalla.
        yield return StartCoroutine(Fade(0, 1));
        // 2. Si venimos de un nivel anterior (unloadActive es true), lo destruimos de la memoria RAM.
        if (unloadActive)
        {
            yield return USceneManager.UnloadSceneAsync(USceneManager.GetActiveScene().buildIndex);
        }

        // 3. Cargamos el nuevo nivel en segundo plano y esperamos a que termine.
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        // 4. Retiramos la pantalla de carga de forma suave (Fade de 1 a 0).
        yield return StartCoroutine(Fade(1, 0));

        _isLoading = false;
    }
    // --- MÉTODO PÚBLICO DE DISPARO ---
    // El método que llamará tu menú, zonas de carga o triggers (ej: USceneManager.Instance.LoadScene("Nivel1", true);)
    public void LoadScene(string sceneName, bool transition)
    {
        // Evitamos que el jugador intente cargar otra escena si ya hay un proceso de Fade o de Carga en marcha
        if (_isFading || _isLoading)
        {
            Debug.LogWarning("Ya se está cargando una escena o haciendo un fundido. Petición ignorada.");
            return;
        }
        // Lanzamos la maquinaria
        StartCoroutine(FadeAndLoadScene(sceneName, transition));
    }
    // --- ATAJOS PARA CONTROL MANUAL ---
    // Útiles si necesitas forzar la visualización de la pantalla desde otros sistemas del juego
    public void ShowLoadingScreen()
    {
        if (_canvasGroup.alpha == 1f) return;
        StartCoroutine(Fade(0f, 1f));
    }
    public void HideLoadingScreen()
    {
        if (_canvasGroup.alpha == 0f) return;
        StartCoroutine(Fade(1f, 0f));
    }
}
