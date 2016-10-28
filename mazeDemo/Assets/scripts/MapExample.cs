using UnityEngine;
using System.Collections;
// includes the MapGen namespace methods
using MapGen;

public class MapExample : MonoBehaviour
{
    public MapTile[,] tiles4;

    public GameObject walkable;
    
    public GameObject notWalkable;

    public GameObject goalPlane;

    public GameObject startPlane;

    public GameObject pc;

    public GameObject enemy1;


    void Awake()
    {
        createN();
    }

    void Start ()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            destroy();
            createN();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            destroy();
            createM();
        }
    }

    void destroy()
    {
        GameObject[] mapGen = GameObject.FindGameObjectsWithTag("pc");
        GameObject[] players = GameObject.FindGameObjectsWithTag("map");
        foreach(GameObject g in mapGen)
        {
            Destroy(g);
        }
        foreach(GameObject g in players)
        {
            Destroy(g);
        }
    }

    void createN()
    {
        PerlinGenerator perlinGen = new PerlinGenerator();
        // generates a map of size 100x100 with a medium constraint (generates a more open map)
        tiles4 = perlinGen.MapGen(75, 75, 7.5f);
        
        foreach (MapTile a in tiles4)
        {
            if (a.IsGoal)
            {
                Instantiate(goalPlane, new Vector3(a.X, 0, a.Y), Quaternion.identity);
            }

            else if (a.IsStart)
            {
                Instantiate(startPlane, new Vector3(a.X, 0, a.Y), Quaternion.identity);
                Instantiate(pc, new Vector3(a.X, .5f, a.Y), Quaternion.identity);
            }

            else if (a.Walkable)
            {
                Instantiate(walkable, new Vector3(a.X, 0, a.Y), Quaternion.identity);
            }

            else
            {
                Instantiate(notWalkable, new Vector3(a.X, 1, a.Y), Quaternion.identity);
            }
        }
        foreach (MapTile a in tiles4)
        {
            if (a.Walkable)
            {
                int chance = Mathf.RoundToInt(Random.Range(.5f, 500.5f));
                if (chance == 1)
                {
                    Instantiate(enemy1, new Vector3(a.X, .5f, a.Y), Quaternion.identity);
                }
                
            }
        }
    }

    void createM()
    {
        PrimGenerator primGen = new PrimGenerator();
        // generate a map of size 20x20 with no extra walls removed
        tiles4 = primGen.MapGen(75, 75, 0.05f);

        foreach (MapTile a in tiles4)
        {
            if (a.IsGoal)
            {
                Instantiate(goalPlane, new Vector3(a.X, 0, a.Y), Quaternion.identity);
            }

            else if (a.IsStart)
            {
                Instantiate(startPlane, new Vector3(a.X, 0, a.Y), Quaternion.identity);
                Instantiate(pc, new Vector3(a.X, .5f, a.Y), Quaternion.identity);
            }

            else if (a.Walkable)
            {
                Instantiate(walkable, new Vector3(a.X, 0, a.Y), Quaternion.identity);
            }

            else
            {
                Instantiate(notWalkable, new Vector3(a.X, 1, a.Y), Quaternion.identity);
            }
        }
        foreach (MapTile a in tiles4)
        {
            if (a.Walkable)
            {
                int chance = Mathf.RoundToInt(Random.Range(.5f, 500.5f));
                if (chance == 1)
                {
                    Instantiate(enemy1, new Vector3(a.X, .5f, a.Y), Quaternion.identity);
                }

            }
        }
    }
}
