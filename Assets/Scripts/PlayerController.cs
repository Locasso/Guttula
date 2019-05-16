using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

    public float moveForce, jumpForce;
    public bool pulou = false;
    public Rigidbody2D rb;
    public GameObject cubeTest, player, controls, fireball, rockTranspas;
    public bool maior, menor;
    public Animator anim;
    public int touchPos;
    public AudioSource walk, jump;
    public PhysicsMaterial2D playerPhsyx;
    public JumpCollider playerFeet;



	void Start ()
    {
        rb = this.GetComponent<Rigidbody2D>();
        playerFeet = GameObject.Find("PlayerBody").GetComponent<JumpCollider>();
        rockTranspas = GameObject.Find("Stone");

	}

    private void FixedUpdate()
    {
       
    

    Vector2 moveVec = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), 0) * moveForce;
        rb.AddForce(moveVec);

        if (CrossPlatformInputManager.GetAxis("Horizontal") > 0)
        {
            player.GetComponent<SpriteRenderer>().flipX = false;

        }
        else if (CrossPlatformInputManager.GetAxis("Horizontal") < 0)
        {
            //player.transform.eulerAngles = new Vector2(0, 180);
            player.GetComponent<SpriteRenderer>().flipX = true;

        }
        else if (CrossPlatformInputManager.GetAxis("Horizontal") == 0)
        {
            anim.SetTrigger("Parou");
        }

    }

    private void Update()
    {
        if (playerFeet.greenMode == true)
            rockTranspas.GetComponent<BoxCollider2D>().enabled = false;
        else
            rockTranspas.GetComponent<BoxCollider2D>().enabled = true;

    }

    public void Andou()
    {
        if(!pulou)
        walk.Play();
    }
    public void Parou()
    {
        walk.Stop();
    }
    public void Action()
    {
        if (playerFeet.redMode == true)
        {
            Instantiate(fireball, this.transform.position, this.transform.rotation);
        }
 
        
    }
    public void AppearControls()
    {
        controls.SetActive(true);
    }

    public void Jump()
    {
        if (!pulou)
        {
            rb.AddForce(Vector2.up * jumpForce);
            anim.SetTrigger("Pulou");
            jump.Play();
            walk.Stop();
            pulou = true;
        }
    }
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.gameObject.tag == "chao")
        { //Check the collider is ground

            transform.up = col.contacts[0].normal;
            rb.freezeRotation = true;
            rb.sharedMaterial = null;

        }
        if (col.collider.gameObject.name == "Vulcan")
        { //Check the collider is ground

            rb.freezeRotation = false;
            rb.sharedMaterial = playerPhsyx;

        }
    }

}
