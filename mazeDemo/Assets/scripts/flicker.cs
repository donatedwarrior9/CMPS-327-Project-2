using UnityEngine;
using System.Collections;

public class flicker : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (this.GetComponent<Light>().intensity < .5f)
        {
            this.GetComponent<Light>().intensity += (Random.value * .10f); //turn it off
        }
        else
        {
            this.GetComponent<Light>().intensity -= (Random.value * .10f); //turn it on
        }
    }
}
