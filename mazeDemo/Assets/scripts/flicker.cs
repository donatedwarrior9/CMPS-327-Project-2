using UnityEngine;
using System.Collections;

public class flicker : MonoBehaviour {
    public float intense;
    public float fidelity;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (this.GetComponent<Light>().intensity < intense)
        {
            this.GetComponent<Light>().intensity += (Random.value * fidelity); //turn it off
        }
        else
        {
            this.GetComponent<Light>().intensity -= (Random.value * fidelity); //turn it on
        }
    }
}
