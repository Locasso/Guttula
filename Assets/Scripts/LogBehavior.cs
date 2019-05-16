using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogBehavior : MonoBehaviour {

    public GameObject fireState;

	// Use this for initialization
	void Start () {

        fireState = GameObject.Find("Firepit");
        fireState.SetActive(false);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "fireball")
        {
            fireState.SetActive(true);
            Destroy(gameObject, 6f);
        }
    }
}
