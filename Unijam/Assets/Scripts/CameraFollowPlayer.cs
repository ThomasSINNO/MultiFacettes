﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {

    private GameObject player;
    float mv;
    public float camspeed;
	// Use this for initialization
	void Start () {
        mv = 0;
        camspeed = 5;
        Engine eng =Object.FindObjectOfType<Engine>();
        player = eng.gameObject;

    }

    //void MoveCamera()
    //{
    //    mv += Input.GetAxis("Move Camera")*camspeed;
    //    if (Input.GetAxis("Move Camera") == 0) {
    //        mv = 0;
    //    }
    //}

    // Update is called once per frame
    void Update () {
        this.transform.position = player.transform.position + new Vector3(mv,0,-8);
	}
}
