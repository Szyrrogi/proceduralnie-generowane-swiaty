using UnityEngine;

public class Noise : MonoBehaviour
{
    public BlockManager blockManager;

    [Header("Pole")]
    [Min(1)] public int rozmiar = 16;
    [Min(1)] public int maksWysokosc = 8;

    [Header("Szum")]
    public int seed = 0;
    [Min(0.0001f)] public float skalaSzumu = 20f;
    [Range(1, 8)] public int oktawy = 4;
    [Range(0f, 1f)] public float persystencja = 0.5f;
    [Min(1f)] public float lakunarnosc = 2f;
    public Vector2 przesuniecie;

    [Header("Odświeżanie")]
    public bool autoOdswiezanie = true;

    private bool doOdswiezenia;
    private float ostatniaSkala;
    
    private Vector2[] przesunieciaOktaw;

    void Start()
    {
        Generuj();
    }

    void OnValidate()
    {
        if (autoOdswiezanie && Application.isPlaying)
            doOdswiezenia = true;
    }

    void Update()
    {
        if (autoOdswiezanie && blockManager.skala != ostatniaSkala)
            doOdswiezenia = true;

        if (doOdswiezenia)
        {
            doOdswiezenia = false;
            Generuj();
        }
    }

    public float Szum(float x, float z)
    {
        float amplituda = 1f;
        float czestotliwosc = 1f;
        float suma = 0f;
        float maksSuma = 0f;

        for (int i = 0; i < oktawy; i++)
        {
            float px = x / skalaSzumu * czestotliwosc + przesunieciaOktaw[i].x;
            float pz = z / skalaSzumu * czestotliwosc + przesunieciaOktaw[i].y;

            suma += Mathf.PerlinNoise(px, pz) * amplituda;
            maksSuma += amplituda;

            amplituda *= persystencja;
            czestotliwosc *= lakunarnosc;
        }

        return suma / maksSuma;
    }

    public void Generuj()
    {
        System.Random prng = new System.Random(seed);
        przesunieciaOktaw = new Vector2[oktawy];
        
        for (int i = 0; i < oktawy; i++)
        {
            float ox = prng.Next(-100000, 100000) + przesuniecie.x;
            float oz = prng.Next(-100000, 100000) + przesuniecie.y;
            przesunieciaOktaw[i] = new Vector2(ox, oz);
        }

        Wyczysc();
        ostatniaSkala = blockManager.skala;

        for (int x = 0; x < rozmiar; x++)
        {
            for (int z = 0; z < rozmiar; z++)
            {
                int wysokosc = Mathf.Max(1, Mathf.RoundToInt(Szum(x, z) * maksWysokosc));
                
                for (int y = 0; y < wysokosc; y++)
                {
                    string blockType = (y == wysokosc - 1) ? "Grass" : "Stone";
                    blockManager.PostawBlok(blockType, new Vector3Int(x, y, z), transform);
                }
            }
        }
    }

    public void Wyczysc()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject b = transform.GetChild(i).gameObject;
            if (Application.isPlaying) Destroy(b);
            else DestroyImmediate(b);
        }
    }
}