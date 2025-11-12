using UnityEngine;

public class Parallax : MonoBehaviour
{
    #region Variables
    [Range(0f, 1f), SerializeField]
    float speedFaktor = 0.5f;
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
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cameraVaiation = new Vector2(_cam.transform.position.x - _camaraLastPosition.x, 0);

        offSett.x = offSett.x + (cameraVaiation.x * speedFaktor);

        //_renderer.material.SetTextureOffset("_MainTex", offSett);
        _renderer.material.mainTextureOffset = offSett;

        _camaraLastPosition = _cam.transform.position;
    }
}
