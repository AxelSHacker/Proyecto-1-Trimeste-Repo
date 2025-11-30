using UnityEngine;

public class Section : MonoBehaviour
{
    [Range(2, 100), SerializeField]
    int columns;
    [Range(2, 100), SerializeField]
    int rows;

    [SerializeField]
    Grid grid;
    [SerializeField]
     public Camera gameCamera;
    //Propiedad solo de lectura que devuelve la mitad del ancho de la seccion en unidades
    //Mitad de columnas multiplicado por el tamno de anocho de una celda del grid
    public float HalfWidth
    {
        get
        {
            return ((columns / 2) * grid.cellSize.x);
        }
    }


    void Update()
    {
        //Calculamos el lado izquierdo de la pantalla en el mundo
        //a propiedad ortographic size es el alto de la camara
        //screen.width es el ancho de la pantalla en pixels
        //screen.height es el ato de la pantalla en pixels
        //con estos datos realitamos una relga de tres:
        //altura Ortho -- Anchura Ortho
        //        height -- width
        //anchura Ortho == (altura Ortho * width) / height
        float leftSideOfScreen = gameCamera.transform.position.x - gameCamera.orthographicSize * Screen.width / Screen.height;
        if (transform.position.x <(leftSideOfScreen- HalfWidth))
        {
            DestroySection();
        }
    }
    void OnDrawGizmos()
    {
        //Si hay un grid , intenta obtrenerlo
        if (grid == null) grid = GetComponentInChildren<Grid>();
        //Si no lo ha podido obtener, cortamos la ejecucion del metodo.
        if (grid == null) return;
        //Seteamo el color del gizzmo segun si las columnas y files son pares o no
        if (columns % 2 == 0 && rows % 2 == 0)
        {
            Gizmos.color = Color.green;

        }
        else { Gizmos.color = Color.red; }

        //Mostramos el gizzmo
        Gizmos.DrawWireCube(transform.position, new Vector3(columns * grid.cellSize.x,
                                                             rows * grid.cellSize.y,
                                                             0f));
    }
/// <summary>
/// Manda generar una seccion nueva y destruye la actual
/// </summary>
    private void DestroySection()
    {
        SectionSpawnerController.Instance.SpawnSection();
        Destroy(gameObject);
    }



}
