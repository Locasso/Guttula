using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : TouchLogicV2 {

    private Vector3 finger;
    private Transform myTrans, camTrans;
    float speed = 1f;

	void Start ()
    {
        myTrans = this.transform;
        camTrans = Camera.main.transform;
	}
    void LookAtFinger()
    {
        Vector3 tempTouch = new Vector3(Input.GetTouch(touch2Watch).position.x, Input.GetTouch(touch2Watch).position.y, camTrans.position.y - myTrans.position.y);

        finger = Camera.main.ScreenToWorldPoint(tempTouch);

        myTrans.LookAt(finger);

        myTrans.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    void OnTouchMovedAnywhere()
    {
        LookAtFinger();
    }

    void  OnTouchStayedAnywhere()
    {
        LookAtFinger();
    }

    void OnTouchBeganAnywhere()
    {
        touch2Watch = TouchLogicV2.currTouch;
    }
        
}
