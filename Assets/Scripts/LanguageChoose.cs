using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageChoose : MonoBehaviour {

     public bool portuguese, english;

	void Start () {

        DontDestroyOnLoad(gameObject);
        portuguese = true;
        english = false;
    }

    public void English()
    {
        english = true;
        portuguese = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void Portuguese()
    {
        english = false;
        portuguese = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
