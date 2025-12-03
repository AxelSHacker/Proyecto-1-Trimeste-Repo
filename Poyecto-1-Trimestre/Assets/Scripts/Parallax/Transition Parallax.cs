using UnityEngine;

public class TransitionParallax : MonoBehaviour
{
    #region Variables
    [Range(0f, 1f), SerializeField]
    float speedFaktor = 0.5f;

    //Offset aplicado a la textura
    [SerializeField]
    Vector2 offSett = Vector2.zero;
    //Referencia a la camra de juego
    //Para almacenar la posicion de la camara en el frame anterior
    [SerializeField]
    Camera _cam;
    [SerializeField]
    //Refencia al renderer del fondo
    Vector2 _camaraLastPosition;

    public Renderer transitionParalaxRenderer;

    [SerializeField]
    Parallax parallax;

    public float scaleFaktor;

    public bool endTransition = false;

    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_cam == null) _cam = Camera.main;
        //Iniciamos la ultima posicion de la camara
        _camaraLastPosition = _cam.transform.position;
        //Recuperamos la referencia del renderer
        if (transitionParalaxRenderer == null) transitionParalaxRenderer = GetComponent<Renderer>();
        scaleFaktor = transform.localScale.x;



        Vector2 backGroundHHalfSize = new Vector2((_cam.orthographicSize * Screen.width) / Screen.height, _cam.orthographicSize);
        //Ajustamos la escala segun la pantalla
        //transform.localScale = new Vector3(backGroundHHalfSize.x * 2, backGroundHHalfSize.y * 2, transform.localScale.z);
        //Ajustamos la escada de la textura
        //transitionParalaxRenderer.material.SetTextureScale("_MainText", backGroundHHalfSize);

        //transitionParalaxRenderer.material.mainTextureScale = backGroundHHalfSize / 5;
    }



    // Update is called once per frame
    void Update()
    {
        if (endTransition)
        {
            transitionParalaxRenderer.material.mainTextureOffset = Vector2.zero;
            offSett = Vector2.zero;
            endTransition = false;
            Debug.Log("Illo");
            
        }

        Vector2 cameraVariation = new Vector2(_cam.transform.position.x - _camaraLastPosition.x, 0);

        float widhtWorld = transitionParalaxRenderer.bounds.size.x;

        offSett.x = offSett.x + (cameraVariation.x * speedFaktor) / widhtWorld;

        transitionParalaxRenderer.material.mainTextureOffset = offSett;

        _camaraLastPosition = _cam.transform.position;


        if (transitionParalaxRenderer.material.mainTextureOffset.x >= 0.5f && transitionParalaxRenderer.enabled == true)
        {

            //transitionParalaxRenderer.material.mainTextureOffset = new Vector2(
                //transitionParalaxRenderer.material.mainTextureOffset.x % 1f,
                //transitionParalaxRenderer.material.mainTextureOffset.y);


            
            parallax.paralaxStarted = true;
            transitionParalaxRenderer.enabled = false;
            

        }






    }











}
