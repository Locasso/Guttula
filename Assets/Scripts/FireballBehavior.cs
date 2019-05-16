using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehavior : MonoBehaviour {

    public Rigidbody2D rb;
    public SpriteRenderer player, fireSprite;
    Vector2 vector;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<SpriteRenderer>();
        fireSprite = GetComponent<SpriteRenderer>();

        if (player.flipX == true)
        {
            vector = Vector2.left;
            fireSprite.flipX = true;
        }
        else
        {
            vector = Vector2.right;
            fireSprite.flipY = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        Move();
        Destroy(gameObject, 2f);
	}
    void Move()
    {    
       rb.AddForce(vector * 5);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "log")
        {
            Destroy(gameObject);
        }
    }
}
