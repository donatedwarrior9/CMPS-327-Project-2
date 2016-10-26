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


    void Awake()
    {
        PrimGenerator primGen = new PrimGenerator();

        // generate a map of size 20x20 with no extra walls removed
        //MapTile[,] tiles1 = primGen.MapGen(20, 20, 0.0f);

        // generate a map of size 30x30 with half of the walls removed after generation
        //tiles4 = primGen.MapGen(100, 100, 0.5f);

        PerlinGenerator perlinGen = new PerlinGenerator();

        // generates a map of size 20x20 with a large constraint (generates a tightly-packed map)
        //MapTile[,] tiles3 = perlinGen.MapGen(20, 20, 5.0f);

        // generates a map of size 100x100 with a medium constraint (generates a more open map)
        tiles4 = perlinGen.MapGen(50, 50, 7.5f);
        

        // generates a map of size 20x20 with a small constraint (generates a very open map)
        //MapTile[,] tiles5 = perlinGen.MapGen(20, 20, 20.0f);

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
    }

    void Start ()
    {
        
    }
}
