using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiBehavior : MonoBehaviour {

    Rigidbody2D rb;


	void Start () {

        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		

	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "colEnd1")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
        }
    }
}
