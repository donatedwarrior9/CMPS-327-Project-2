using UnityEngine;
using System.Collections;
using MapGen;
using System.Collections.Generic;
public enum playerstate {move, idle}

public class pcscript : MonoBehaviour {

    public MapTile[,] map;
    public GameObject MyPrefab;
    public GameObject MyPrefab2;
    public AudioSource audio;
    int currpoint = 0;
    Vector3 targetWaypoint;
    [Range(.1f, 30f)]
    public float speed;
    playerstate state;
    bool paused;
    List<node> neighbors;
    List<node> discovered;
    List<node> visited;
    List<node> path;
    List<KeyValuePair<playerstate, playerstate>> pairs = new List<KeyValuePair<playerstate, playerstate>>
    { };

    public class node {
        public int x;
        public int y;
        public int f;
        public int g;
        public int h;
        public node parent;

        public node() {
            this.x = 0;
            this.y = 0;
            this.f = 0;
            this.g = 0;
            this.h = 0;
            this.parent = null;
        }

        public node(int a, int b) {
            this.x = a;
            this.y = b;
            this.g = 0;
            this.f = 0;
            this.h = 0;
            this.parent = null;
        }

        public node(int a, int b, node start, node finish) {
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
    void Start()
    {
        map = GameObject.FindObjectOfType<MapExample>().tiles4;
        node finish = null;
        node start = null;
        foreach (MapTile a in map)
        {
            if (a.IsGoal)
            {
                finish = new node(a.X, a.Y);
                Debug.Log("found finish at " + a.X + ", " + a.Y);
            }

            else if (a.IsStart)
            {
                start = new node(a.X, a.Y);
            }
        }
        aStar(start, finish);

        if (path != null)
        {
            targetWaypoint = new Vector3(path[currpoint].x, 1, path[currpoint].y);
            paused = false;
            state = playerstate.idle;
        }
        else
        {
            paused = true;
            state = playerstate.move;
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            paused = !paused;
        }
        if (paused)
            state = playerstate.idle;
        else
            state = playerstate.move;
        doStates();
    }
    void aStar(node start, node finish) {
        neighbors = new List<node>();
        discovered = new List<node>();
        visited = new List<node>();
        path = new List<node>();
        node current = null;
        float posy = 4;
        discovered.Add(start);
        while (discovered.Count > 0) {
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

            foreach(node n in neighbors)
            {
                if (visited.Contains(n)) {
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
            Debug.Log("couldnt find path");
            audio.Play();
            path = null;
        }
        else
        {
            Debug.Log("got to finish at " + current.x + ", " + current.y);
            neighbors.Clear();
            while (current != start)
            {
                neighbors.Add(current);
                current = current.parent;
                
            }
            for (int i = neighbors.Count-1; i>=0; i--)
            {
                path.Add(neighbors[i]);
                Vector3 pos = new Vector3(neighbors[i].x, 2, neighbors[i].y);
                Instantiate(MyPrefab2, pos, Quaternion.identity);
            }
        }
       
    }
    void Move(Vector3 target)
    {
        this.transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
    void doStartStuff()
    {
        map = GameObject.FindObjectOfType<MapExample>().tiles4;
        node finish = null;
        node start = null;
        foreach (MapTile a in map)
        {
            if (a.IsGoal)
            {
                finish = new node(a.X, a.Y);
                Debug.Log("found finish at " + a.X + ", " + a.Y);
            }

            else if (a.IsStart)
            {
                start = new node(a.X, a.Y);
            }
        }
        aStar(start, finish);

        if (path != null)
        {
            targetWaypoint = new Vector3(path[currpoint].x, 1, path[currpoint].y);
            state = playerstate.move;
        }
        else
            state = playerstate.idle;
    }
    void doStates()
    {
        switch (state)
        {
            case playerstate.move:
                if (path != null)
                {
                    if (currpoint < path.Count)
                    {
                        Move(targetWaypoint);
                        if (this.transform.position == targetWaypoint)
                        {
                            currpoint++;
                            if (currpoint < path.Count)
                                targetWaypoint = new Vector3(path[currpoint].x, 1, path[currpoint].y);
                        }
                    }
                }
                break;
            case playerstate.idle:
                break;
        }
    }
}
