using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueScript : MonoBehaviour {

    public GameObject music; 

    public void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        music = GameObject.Find("BackgroundMusic");
        Destroy(music);
    }
}
