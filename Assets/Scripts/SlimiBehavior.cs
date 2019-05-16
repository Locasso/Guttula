using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimiBehavior : MonoBehaviour {

    public GameObject controls;
    public RPGTalk talk;
    public LanguageChoose languageManager;
    public Rigidbody2D rb;
    public SpriteRenderer renderer;
    int speed = 100;
    public Vector2 vector = Vector2.right;
    public GameObject chat;
    public AudioSource roar;
    public int smiliCount = 1;
  


	void Start () {

        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        roar = GetComponent<AudioSource>();
        chat = GameObject.Find("ChatBallon");
        chat.SetActive(false);
        languageManager = GameObject.Find("LanguageManager").GetComponent<LanguageChoose>();
        talk = GameObject.Find("Canvas").GetComponent<RPGTalk>();
    }
	

	void Update () {

        Move();	
	}

    void Move()
    {
        rb.AddForce(vector * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "log")
        {
            if (vector == Vector2.right)
            {
                transform.eulerAngles = new Vector2(0, 180);
                vector = Vector2.left;

            }
           else if (vector == Vector2.left)
            {
                transform.eulerAngles = new Vector2(0, 0);
                vector = Vector2.right;

            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "mus" || collision.gameObject.tag == "Player")
        {
            transform.eulerAngles = new Vector2(0, 0);
            vector = Vector2.right;
            chat.SetActive(true);

            if (smiliCount == 1)
            {
                if (languageManager.portuguese)
                {

                    talk.lineToStart = 7;
                    talk.lineToBreak = 7;
                    talk.NewTalk();
                    controls.SetActive(false);
                    smiliCount++;

                }
                else if (languageManager.english)
                {
                    talk.lineToStart = 8;
                    talk.lineToBreak = 8;
                    talk.NewTalk();
                    controls.SetActive(false);
                    smiliCount++;
                }
            }

            
        }
        if (collision.gameObject.tag == "smiliDie")
        {
            roar.Play();
            Destroy(gameObject, 4f);
        }
    }

}
