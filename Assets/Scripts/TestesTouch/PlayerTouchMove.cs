using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchMove : MonoBehaviour {

    public float speed = 10f, jumpHeight = 10;
    public Transform myTrans, tagGround;
    public LayerMask playerMask;
    public Rigidbody2D myRb;
    Vector2 movement;
    bool isGrounded = false;



	void Start () {

        myTrans = this.transform;
        myRb = this.GetComponent<Rigidbody2D>();
	}
	

	void FixedUpdate ()
    {
        isGrounded = Physics2D.Linecast(myTrans.position, tagGround.position, playerMask);

        Move(Input.GetAxisRaw("Horizontal"));
        if (Input.GetButtonDown("Jump"))
            Jump();
              
	}

    public void Move(float horizontal_input)
    {
        Vector2 moveVel = myRb.velocity;
        moveVel.x = horizontal_input * speed;
        myRb.velocity = moveVel;
 
    }

    public void Jump()
    {
        if(isGrounded)
        myRb.velocity += jumpHeight * Vector2.up;
    }




}
