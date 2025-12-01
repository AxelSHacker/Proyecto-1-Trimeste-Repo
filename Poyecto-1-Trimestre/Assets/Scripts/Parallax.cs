using UnityEngine;

public class Parallax : MonoBehaviour
{
    #region Variables
    [Range(0f, 1f), SerializeField]
    float speedFaktor = 0.5f;
    [SerializeField]
    float distance;
    //Offset aplicado a la textura
    [SerializeField]
    Vector2 offSett = Vector2.zero;
    //Referencia a la camra de juego
    [SerializeField]
    Camera _cam;
    [SerializeField]
    //Para almacenar la posicion de la camara en el frame anterior
    Vector2 _camaraLastPosition;

    //Refencia al renderer del fondo
    [SerializeField]
    Renderer _renderer;
    [SerializeField]
    Material[] parallayTransition;

    enum EstadoTransicion { BiomaA, Trans1, Trans2, BiomaB }
    EstadoTransicion estado = EstadoTransicion.BiomaA;

    float uvTrans1;
    float uvTrans2;
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_cam == null) _cam = Camera.main;
        //Iniciamos la ultima posicion de la camara
        _camaraLastPosition = _cam.transform.position;
        //Recuperamos la referencia del renderer
        if (_renderer == null) _renderer = GetComponent<Renderer>();

        Vector2 backGroundHHalfSize = new Vector2((_cam.orthographicSize * Screen.width) / Screen.height, _cam.orthographicSize);
        //Ajustamos la escala segun la pantalla
        transform.localScale = new Vector3(backGroundHHalfSize.x * 2, backGroundHHalfSize.y * 2, transform.localScale.z);
        //Ajustamos la escada de la textura
        //_renderer.material.SetTextureScale("_MainText", backGroundHHalfSize);

        //_renderer.material.mainTextureScale = backGroundHHalfSize / 5;

        // Calculamos UV reales de las transiciones
        uvTrans1 = GetUVLength(parallayTransition[0]);
        uvTrans2 = GetUVLength(parallayTransition[1]);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cameraVaiation = new Vector2(_cam.transform.position.x - _camaraLastPosition.x, 0);

        offSett.x = offSett.x + (cameraVaiation.x * speedFaktor);

        //_renderer.material.SetTextureOffset("_MainTex", offSett);
        _renderer.material.mainTextureOffset = offSett;

        _camaraLastPosition = _cam.transform.position;
        ParallaxTransition();


    }

    // Devuelve el UV real basado en width/height del sprite
    float GetUVLength(Material mat)
    {
        Texture tex = mat.mainTexture;
        return (float)tex.width / tex.height;
    }

    void ParallaxTransition()
    {

        float offset = _renderer.material.mainTextureOffset.x;
        

        switch (estado)
        {
            case EstadoTransicion.BiomaA:
                if (_cam.transform.position.x >= distance)
                {
                    _renderer.material = parallayTransition[0];
                    offSett = Vector2.zero;
                    _renderer.material.mainTextureOffset = offSett;
                    
                    estado = EstadoTransicion.Trans1;
                }
                break;

            case EstadoTransicion.Trans1:
                if (offset >= uvTrans1)   // Trans1 terminada
                {
                    _renderer.material = parallayTransition[1];
                    offSett = Vector2.zero;
                    _renderer.material.mainTextureOffset = offSett;
                    
                    estado = EstadoTransicion.Trans2;
                }
                break;

            case EstadoTransicion.Trans2:
                if (offset >= uvTrans2)   // Trans2 terminada
                {
                    _renderer.material = parallayTransition[2];
                    offSett = Vector2.zero;
                    _renderer.material.mainTextureOffset = offSett;
                    estado = EstadoTransicion.BiomaB;
                }
                break;
        }
    }


}
