using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoll : MonoBehaviour {

    public GameObject player;
    Vector2 pos;
    public int screen = 0;

    void Update()
    {
        pos = new Vector2(player.transform.position.x, player.transform.position.y);
        if (pos.x > transform.position.x + 8f) { transform.position += Vector3.right * 16; screen += 1; }
        if (pos.x < transform.position.x - 8f) { transform.position += Vector3.left * 16; screen -= 1; }
        if (pos.y > transform.position.y + 5f) { transform.position += new Vector3(0, 10, 0); screen -= 4; }
        if (pos.y < transform.position.y - 5f) { transform.position -= new Vector3(0, 10, 0); screen += 4; }

    }
}
