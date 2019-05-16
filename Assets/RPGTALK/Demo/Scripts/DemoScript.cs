using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DemoScript : MonoBehaviour {

	Rigidbody2D rigid;
	public float speed = 10f;
	Animator anim;
	SpriteRenderer render;

	public bool controls;

	string touching;

	public RPGTalk rpgTalk;

	public GameObject askWho;

	public InputField name;

	public GameObject wall;
	public GameObject particle;

	public RPGTalk rpgTalkToFollow;

	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		render = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		//skip the Talk to the end
		if(Input.GetKeyDown(KeyCode.Return)){
			rpgTalk.EndTalk ();
		}



		//if the user have the controls
		if (controls) {

			//let's move around!
			float moveX = Input.GetAxis ("Horizontal");
			float moveY = Input.GetAxis ("Vertical");
			rigid.MovePosition (new Vector2 (transform.position.x + moveX * speed, transform.position.y + moveY * speed));

			//Not the best way to do it but... change the animator
			if (moveX > 0) {
				anim.SetBool ("side", true);
				anim.SetBool ("top", false);
				anim.SetBool ("bottom", false);
				render.flipX = true;
				anim.speed = 1;
			} else if (moveX < 0) {
				anim.SetBool ("side", true);
				anim.SetBool ("top", false);
				anim.SetBool ("bottom", false);
				render.flipX = false;
				anim.speed = 1;
			} else if (moveY < 0) {
				anim.SetBool ("side", false);
				anim.SetBool ("top", false);
				anim.SetBool ("bottom", true);
				anim.speed = 1;
			} else if (moveY > 0) {
				anim.SetBool ("side", false);
				anim.SetBool ("top", true);
				anim.SetBool ("bottom", false);
				anim.speed = 1;
			} else {
				anim.speed = 0;
			}


			//if the player hits E, check if it is talking with someone
			if(Input.GetKeyDown(KeyCode.E)){
				if (touching == "FunnyGuy") {
					controls = false;
					rpgTalk.lineToStart = 15;
					rpgTalk.lineToBreak = 16;
					rpgTalk.callbackFunction = "WhoAreYou";
					rpgTalk.NewTalk ();
				}
				if (touching == "Girl") {
					controls = false;
					rpgTalk.lineToStart = 33;
					rpgTalk.lineToBreak = -1;
					rpgTalk.callbackFunction = "GiveBackControls";
					rpgTalk.shouldStayOnScreen = true;
					rpgTalk.NewTalk ();
				}
			}



		} else {
			anim.speed = 0;
		}
	}

	//give the controls to player
	public void GiveBackControls(){
		controls = true;
	}

	//Open the screen to enter Player's name
	public void WhoAreYou(){
		//controls = true;
		askWho.SetActive(true);
		name.Select ();
	}

	public void IKnowYouNow(){
		askWho.SetActive (false);
		rpgTalk.variables [0].variableValue = name.text;
		rpgTalk.lineToStart = 17;
		rpgTalk.lineToBreak = 25;
		rpgTalk.callbackFunction = "ByeWall";
		rpgTalk.NewTalk ();
	}

	public void ByeWall(){
		wall.SetActive (false);
		particle.SetActive (true);
		Invoke ("FunnyGuyEnd", 2f);
	}

	void FunnyGuyEnd(){
		rpgTalk.lineToStart = 26;
		rpgTalk.lineToBreak = 29;
		rpgTalk.callbackFunction = "GiveBackControls";
		rpgTalk.NewTalk ();
	}

	//touching who?
	void OnTriggerEnter2D(Collider2D col){
		touching = col.name;
		if(touching == "startFollow"){
			rpgTalkToFollow.NewTalk ();
		}
	}

	void OnTriggerExit2D(Collider2D col){
		touching = "";
	}
}
