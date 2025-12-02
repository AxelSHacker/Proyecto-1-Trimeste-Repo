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
    [SerializeField]
    Renderer transitionParalaxRenderer;
    [SerializeField]
    float materialOffset;
    [SerializeField]
    Parallax parallax;

    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_cam == null) _cam = Camera.main;
        //Iniciamos la ultima posicion de la camara
        _camaraLastPosition = _cam.transform.position;
        //Recuperamos la referencia del renderer
        if (transitionParalaxRenderer == null) transitionParalaxRenderer = GetComponent<Renderer>();



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


        Vector2 cameraVariation = new Vector2(_cam.transform.position.x - _camaraLastPosition.x, 0);

        offSett.x = offSett.x + (cameraVariation.x * speedFaktor);

        //_renderer.material.SetTextureOffset("_MainTex", offSett);
        transitionParalaxRenderer.material.mainTextureOffset = offSett;

        _camaraLastPosition = _cam.transform.position;

        Debug.Log(transitionParalaxRenderer.material.mainTextureOffset);

        if (transitionParalaxRenderer.material.mainTextureOffset.x >= materialOffset)
        {
            parallax._renderer.material = parallax.newBioma;
            parallax.offSett = Vector2.zero;
            parallax._renderer.enabled = true;
            gameObject.SetActive(false);
            
        }


    }





}
