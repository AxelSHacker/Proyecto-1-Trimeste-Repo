
using UnityEngine;
using UnityEngine.SceneManagement;

public class SectionSpawnerController : MonoBehaviour
{

    //Coleccion de secciones del nivel

    public Section[] sectionPrefabs;
    //Transform en el que spawnean las secciones

    public Transform sectionParent;
    //Ultima seccion creada

    public Section currentSection;
    //SEcciones que se vana generar a inicio

    public int sectionInitialload;

    //Referencia a la camara del juego
    public Camera gameCamera;

    private static SectionSpawnerController _instance;
    public static SectionSpawnerController Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else { Destroy(this); }
    }
    void Start()
    {
        if (sectionParent == null) sectionParent = transform;

        for (int i = 0; i < sectionInitialload; i++)
        {
            SpawnRandomSection();
        }



    }
    /// <summary>
    ///Crea una seccion nueva a continuacuion de la ultima 
    /// </summary>
    public void SpawnRandomSection()
    {
        // //Obtener una seccion aleatorio
        // int randomIndex = Random.Range(0, sectionPrefabs.Length);
        // Section nextSection = sectionPrefabs[randomIndex];
        // //Vecto parsa calcular la posicion a la derecha
        // Vector3 nextPositionOffset = Vector3.zero;
        // nextPositionOffset.x = currentSection.HalfWidth + nextSection.HalfWidth;
        // //Instanciamos la nueva seccion y la almacenamos como ultima seccion creada
        // currentSection = Instantiate(nextSection,
        //                              currentSection.transform.position + nextPositionOffset,
        //                              Quaternion.identity, sectionParent);

        // currentSection.gameCamera = gameCamera;

        // 1. Elegimos el prefab aleatorio
        int randomIndex = Random.Range(0, sectionPrefabs.Length);
        Section sectionPrefab = sectionPrefabs[randomIndex];

        // 2. Instanciamos la nueva sección (de momento en la posición de la sección actual)
        Section newSection = Instantiate(sectionPrefab, currentSection.transform.position, Quaternion.identity, sectionParent);

        // 3. Ahora que newSection YA EXISTE en la escena, sus componentes (Grid, etc.) están $100\%$ despiertos.
        // Calculamos el desplazamiento exacto X
        float offset = currentSection.HalfWidth + newSection.HalfWidth;

        // 4. Reposicionamos la nueva sección a la derecha
        newSection.transform.position += new Vector3(offset, 0f, 0f);

        // 5. Asignamos la cámara y actualizamos la referencia de la sección actual
        newSection.gameCamera = gameCamera;
        currentSection = newSection;
    }



}
