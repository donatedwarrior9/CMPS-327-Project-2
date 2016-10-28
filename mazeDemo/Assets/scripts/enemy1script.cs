using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MapGen;
public enum enemystate {move, chase }

public class enemy1script : MonoBehaviour {
    public Vector3 target;
    public GameObject MyPrefab2;
    List<node> path;
    public MapTile[,] map;

    public class node
    {
        public int x;
        public int y;
        public int f;
        public int g;
        public int h;
        public node parent;

        public node()
        {
            this.x = 0;
            this.y = 0;
            this.f = 0;
            this.g = 0;
            this.h = 0;
            this.parent = null;
        }

        public node(int a, int b)
        {
            this.x = a;
            this.y = b;
            this.g = 0;
            this.f = 0;
            this.h = 0;
            this.parent = null;
        }

        public node(int a, int b, node start, node finish)
        {
            this.x = a;
            this.y = b;
            this.g = 0;
            this.f = Mathf.Abs(this.x - finish.x) + Mathf.Abs(this.y - finish.y);
            this.h = this.f + this.g;
            this.parent = null;
        }
        public node(int a, int b, node start, node finish, node parent)
        {
            this.x = a;
            this.y = b;
            this.g = parent.g + 1;
            this.f = Mathf.Abs(this.x - finish.x) + Mathf.Abs(this.y - finish.y);
            this.h = this.f + this.g;
            this.parent = parent;
        }

        public static bool operator ==(node n1, node n2)
        {
            return (n1.x == n2.x && n1.y == n2.y);
        }

        public static bool operator !=(node n1, node n2)
        {
            return !(n1.x == n2.x && n1.y == n2.y);
        }

        public override bool Equals(object n1)
        {
            node n2 = (node)n1;
            return (this.x == n2.x && this.y == n2.y);
        }
        public static bool operator <(node n1, node n2)
        {
            return (n1.g < n2.g);
        }
        public static bool operator >(node n1, node n2)
        {
            return (n1.g > n2.g);
        }
        public static bool operator >=(node n1, node n2)
        {
            return (n1.g >= n2.g);
        }
        public static bool operator <=(node n1, node n2)
        {
            return (n1.g <= n2.g);
        }
    }
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        
	}
   
    List<node> aStar(node start, node finish)
    {
        List<node> neighbors = new List<node>();
        List<node> discovered = new List<node>();
        List<node> visited = new List<node>();
        List<node> walkPath = new List<node>();
        node current = null;
        float posy = 4;
        discovered.Add(start);
        while (discovered.Count > 0)
        {
            int minDist = 10000;
            foreach (node n in discovered)
            {
                if (n.h < minDist)
                {

                    current = n;
                    minDist = n.h;
                }
            }
            discovered.Remove(current);
            visited.Add(current);
            if (current == finish)
            {
                Debug.Log("all done");
                break;
            }

            if (current.y - 1 >= 0)
            {
                if (map[current.x, (current.y - 1)].Walkable)
                {
                    node n = new node((current.x), (current.y - 1), start, finish, current);
                    if (discovered.Contains(n))
                    {
                        neighbors.Add(discovered.Find(x => x == n));
                    }
                    else
                    {
                        neighbors.Add(new node((current.x), (current.y - 1), start, finish, current));
                    }
                }
            }
            if (current.y + 1 < map.GetLength(0))
            {
                if (map[current.x, (current.y + 1)].Walkable)
                {
                    node n = new node((current.x), (current.y + 1), start, finish, current);
                    if (discovered.Contains(n))
                    {
                        neighbors.Add(discovered.Find(x => x == n));
                    }
                    else
                    {
                        neighbors.Add(new node((current.x), (current.y + 1), start, finish, current));
                    }
                }
            }
            if (current.x - 1 >= 0)
            {
                if (map[(current.x - 1), current.y].Walkable)
                {
                    node n = new node((current.x - 1), (current.y), start, finish, current);
                    if (discovered.Contains(n))
                    {
                        neighbors.Add(discovered.Find(x => x == n));
                    }

                    else
                    {
                        neighbors.Add(new node((current.x - 1), (current.y), start, finish, current));
                    }
                }
            }
            if (current.x + 1 < map.GetLength(0))
            {
                if (map[(current.x + 1), current.y].Walkable)
                {
                    node n = new node((current.x + 1), (current.y), start, finish, current);
                    if (discovered.Contains(n))
                    {
                        neighbors.Add(discovered.Find(x => x == n));
                    }
                    else
                    {
                        neighbors.Add(new node((current.x + 1), (current.y), start, finish, current));
                    }
                }
            }

            foreach (node n in neighbors)
            {
                if (visited.Contains(n))
                {
                    continue;
                }
                if (n.g > (current.g + 1) || !discovered.Contains(n))
                {
                    n.g = current.g + 1;
                    n.parent = current;
                    if (!discovered.Contains(n))
                    {
                        discovered.Add(n);
                    }
                }
            }
            neighbors.Clear();
        }
        if (current != finish)
        {
            //Debug.Log("couldnt find path");
            walkPath = null;
            return walkPath;
        }
        else
        {
            //Debug.Log("got to finish at " + current.x + ", " + current.y);
            neighbors.Clear();
            while (current != start)
            {
                walkPath.Add(current);
                Vector3 pos = new Vector3(current.x, 2, current.y);
                Instantiate(MyPrefab2, pos, Quaternion.identity);
                current = current.parent;

            }
            walkPath.Reverse();
            return walkPath;

        }

    }

    void dostartstuff()
    {
        map = GameObject.FindObjectOfType<MapExample>().tiles4;
        GetATarget();
    }

    void GetATarget()
    {
        target = GameObject.FindObjectOfType<pcscript>().transform.position;
    }

}
