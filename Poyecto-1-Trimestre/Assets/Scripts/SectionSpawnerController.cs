using UnityEngine;

public class SectionSpawnerController : MonoBehaviour
{

    //Coleccion de secciones del nivel
    
    public Section[] sectionPrefabs;
    //Transform en el que spawnean las secciones
    
    public Transform sectionParent;
    //Ultima seccion creada
    
    public Section currentSection;
    //SEcciones que se vana generar a inicio

    public int sectionInitialload = 4;

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
        else{ Destroy(this); }
    }
    void Start()
    {
        if (sectionParent == null) sectionParent = transform;

        for (int i = 0; i < sectionInitialload; i++)
        {
            SpawnSection();
        }
    }
    /// <summary>
    ///Crea una seccion nueva a continuacuion de la ultima 
    /// </summary>
    public void SpawnSection()
    {
        //Obtener una seccion aleatorio
        int randomIndex = Random.Range(0, sectionPrefabs.Length);
        Section nextSection = sectionPrefabs[randomIndex];
        //Vecto parsa calcular la posicion a la derecha
        Vector3 nextPositionOffset = Vector3.zero;
        nextPositionOffset.x = currentSection.HalfWidth + nextSection.HalfWidth;
        //Instanciamos la nueva seccion y la almacenamos como ultima seccion creada
        currentSection = Instantiate(nextSection,
                                     currentSection.transform.position + nextPositionOffset,
                                     Quaternion.identity, sectionParent);
        
        currentSection.gameCamera = gameCamera;                 

    }

}
