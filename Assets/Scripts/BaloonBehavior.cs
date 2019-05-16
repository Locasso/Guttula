using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaloonBehavior : MonoBehaviour {

    public Image baloonFala;

	void Start () {

        baloonFala = this.gameObject.GetComponent<Image>();
	}
	

	void Update () {
		
	}

    public void Clicou()
    {
        baloonFala.gameObject.SetActive(false);
    }
}
