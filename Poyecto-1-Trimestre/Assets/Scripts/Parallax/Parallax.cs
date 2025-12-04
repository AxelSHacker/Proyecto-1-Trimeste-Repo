using UnityEngine;
using UnityEngine.SceneManagement;

public class Parallax : MonoBehaviour
{
    #region Variables
    [Range(0f, 1f), SerializeField]
    public float speedFaktor = 0.5f;
    [SerializeField]
    float distance;
    //Offset aplicado a la textura

    public Vector2 offSett = Vector2.zero;
    //Referencia a la camra de juego
    [SerializeField]
    Camera _cam;
    [SerializeField]
    //Para almacenar la posicion de la camara en el frame anterior
    Vector2 _camaraLastPosition;
    [SerializeField]
    int materialLoops;

    //Refencia al renderer del fondo

    public Renderer _renderer;
    [SerializeField]
    GameObject transitionRendererObject;
    [SerializeField]
    public Material newBioma;
    [SerializeField]
    float activateTransition;
    [SerializeField]
    TransitionParallax transitionParallax;
    Vector2 cameraVariation;

    float scaleFaktor;
    public bool paralaxStarted = false;
    bool platform;


    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_cam == null) _cam = Camera.main;
        //Iniciamos la ultima posicion de la camara
        _camaraLastPosition = _cam.transform.position;
        //Recuperamos la referencia del renderer
        if (_renderer == null) _renderer = GetComponent<Renderer>();

        if (SceneManager.GetActiveScene().name == "Platform 2D")
        {
            platform = true;
        }
        else
        {
            platform = false;
        }

        scaleFaktor = transform.localScale.x;

        Vector2 backGroundHHalfSize = new Vector2((_cam.orthographicSize * Screen.width) / Screen.height, _cam.orthographicSize);
        //Ajustamos la escala segun la pantalla
        //transform.localScale = new Vector3(backGroundHHalfSize.x * 2, backGroundHHalfSize.y * 2, transform.localScale.z);
        //Ajustamos la escada de la textura
        //_renderer.material.SetTextureScale("_MainText", backGroundHHalfSize);

        //_renderer.material.mainTextureScale = backGroundHHalfSize / 5;
    }



    // Update is called once per frame
    void Update()
    {
        if (platform)
        {
            if (_renderer.material.mainTextureOffset.x >= materialLoops)
            {
                materialLoops++;
                _renderer.material.mainTextureOffset = new Vector2(
                    _renderer.material.mainTextureOffset.x % 1f,
                    _renderer.material.mainTextureOffset.y);


            }
            if (paralaxStarted)
            {
                _renderer.enabled = true;
                _renderer.material.mainTextureOffset = Vector2.zero;
                _renderer.material = newBioma;
                offSett = Vector2.zero;
                paralaxStarted = false;
                Debug.Log("Colegon");

            }

            ParallaxTransition();

        }
        else
        {
            transitionRendererObject = null;
            newBioma = null;
            transitionParallax = null;
        }



        cameraVariation = new Vector2(_cam.transform.position.x - _camaraLastPosition.x, 0);

        float widhtWorldParalax1 = _renderer.bounds.size.x;

        offSett.x = offSett.x + (cameraVariation.x * speedFaktor) / widhtWorldParalax1;

        //_renderer.material.SetTextureOffset("_MainTex", offSett);
        _renderer.material.mainTextureOffset = offSett;

        _camaraLastPosition = _cam.transform.position;




    }

    void ParallaxTransition()
    {
        //Debug.Log(_renderer.material.mainTextureOffset);


        if (materialLoops >= distance)
        {
            _renderer.enabled = false;
            transitionParallax.transitionParalaxRenderer.material.mainTextureOffset = Vector2.zero;
            transitionParallax.transitionParalaxRenderer.enabled = true;
            transitionParallax.endTransition = true;
            distance = 500f;

        }


    }






}


































