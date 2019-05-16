using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinhoBehavior : LogBehavior {

	// Use this for initialization
	void Start () {

        fireState = GameObject.Find("Firepit1");
        fireState.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
