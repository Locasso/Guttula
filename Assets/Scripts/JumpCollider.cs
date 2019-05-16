using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCollider : MonoBehaviour {

    public PlayerController player;
    public GameObject playerPrefab, body, yetiGO, yetiGO2;
    public bool redMode, greenMode, txtRight3;
    public Animator yetiAnim, yetiAnim2;
    public GameObject controls;
    public RPGTalk talk;
    public LanguageChoose languageManager;
    public int fireCount = 1, leafCount = 1;




    public void Start()
    {
        yetiGO = GameObject.Find("Yeti");
        yetiGO2 = GameObject.Find("Yeti2");
        yetiAnim = yetiGO.GetComponent<Animator>();
        yetiAnim2 = yetiGO2.GetComponent<Animator>();
        languageManager = GameObject.Find("LanguageManager").GetComponent<LanguageChoose>();
        talk = GameObject.Find("Canvas").GetComponent<RPGTalk>();
        //controls = GameObject.Find("MobileSingleStickControl");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "chao")
        {
            player.pulou = false;
        }
    
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.gameObject.name == "chao")
        { //Check the collider is ground

            transform.up = col.contacts[0].normal;
            playerPrefab.transform.up = transform.up;
            
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "firerock")
        {
            playerPrefab.GetComponent<SpriteRenderer>().color = Color.red;
            redMode = true;
            greenMode = false;
            if (fireCount == 1)
            {
                if (languageManager.portuguese)
                {

                    talk.lineToStart = 5;
                    talk.lineToBreak = 5;
                    talk.NewTalk();
                    controls.SetActive(false);
                    fireCount++;

                }
                else if (languageManager.english)
                {
                    talk.lineToStart = 6;
                    talk.lineToBreak = 6;
                    talk.NewTalk();
                    controls.SetActive(false);
                    fireCount++;

                }
            }
        }
        if (collision.gameObject.tag == "leafmode")
        {
            playerPrefab.GetComponent<SpriteRenderer>().color = Color.green;
            redMode = false;
            greenMode = true;

            if (leafCount == 1)
            {
                if (languageManager.portuguese)
                {

                    talk.lineToStart = 11;
                    talk.lineToBreak = 11;
                    talk.NewTalk();
                    controls.SetActive(false);
                    leafCount++;
                }
                else if (languageManager.english)
                {
                    talk.lineToStart = 12;
                    talk.lineToBreak = 12;
                    talk.NewTalk();
                    controls.SetActive(false);
                    leafCount++;
                }
            }
        }
        if(collision.gameObject.tag == "yetiGoAway")
        {
            yetiGO.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 100);
            yetiGO.GetComponent<SpriteRenderer>().flipX = true;
            yetiAnim.SetTrigger("Moveu");
            yetiAnim2.SetTrigger("Hold");
            Destroy(yetiGO, 4f);        
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;

            if (languageManager.portuguese)
            {

                talk.lineToStart = 3;
                talk.lineToBreak = 3;
                talk.NewTalk();
                controls.SetActive(false);
            }
            else if (languageManager.english)
            {
                talk.lineToStart = 4;
                talk.lineToBreak = 4;
                talk.NewTalk();
                controls.SetActive(false);
            }

        }

        if(collision.gameObject.tag == "musStone")
        {
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;

            if (languageManager.portuguese)
            {

                talk.lineToStart = 13;
                talk.lineToBreak = 13;
                talk.NewTalk();
                controls.SetActive(false);
            }
            else if (languageManager.english)
            {
                talk.lineToStart = 14;
                talk.lineToBreak = 14;
                talk.NewTalk();
                controls.SetActive(false);
            }
        }
        if (collision.gameObject.tag == "colLava")
        {
            
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;

            if (languageManager.portuguese)
            {

                talk.lineToStart = 15;
                talk.lineToBreak = 15;
                talk.NewTalk();
                controls.SetActive(false);
            }
            else if (languageManager.english)
            {
                talk.lineToStart = 16;
                talk.lineToBreak = 16;
                talk.NewTalk();
                controls.SetActive(false);
            }
        }

        if (collision.gameObject.tag == "colEnd2")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
        }
    }
}
