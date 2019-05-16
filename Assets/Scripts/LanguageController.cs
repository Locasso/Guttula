using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class LanguageController : MonoBehaviour {

    public LanguageChoose languageManager;
    public JumpCollider jumpCollider;
    public RPGTalk talk;
    public GameObject controls;


    [Header("Tela 1")]
    public Text fala_1;

	void Start () {

        languageManager = GameObject.Find("LanguageManager").GetComponent<LanguageChoose>();
        jumpCollider = GameObject.Find("PlayerBody").GetComponent<JumpCollider>();
        talk = GameObject.Find("Canvas").GetComponent<RPGTalk>();
        controls = GameObject.Find("MobileSingleStickControl");

        if (languageManager.portuguese)
        {
            
            talk.lineToStart = 1;
            talk.lineToBreak = 1;
            talk.NewTalk();
            controls.SetActive(false);
        }  //fala_1.text = "Aqui ele começa a sua trajetória. Um ser tão ingênuo, e incapaz de entender qualquer coisa. O que ele fará agora?";
        else if (languageManager.english)
        {    
            talk.lineToStart = 2;
            talk.lineToBreak = 2;
            talk.NewTalk();
            controls.SetActive(false);
        }
    }
	
	void Update () {


    }
}
