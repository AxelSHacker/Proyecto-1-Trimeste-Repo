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
        
    }

    
    void Update()
    {
        
    }
}
