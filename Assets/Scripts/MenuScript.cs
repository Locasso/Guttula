using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    GameObject canvasCredits;

	void Start () {

        canvasCredits = GameObject.Find("CanvasCredits");
        canvasCredits.SetActive(false);
	}
	
    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
    public void Credits()
    {
        canvasCredits.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void CreditsClose()
    {
        canvasCredits.gameObject.SetActive(false);
    }

}
