using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {

    public GameObject player;

    void Update () {
        this.transform.position = player.transform.position + new Vector3(0,0,-8);
	}
}
