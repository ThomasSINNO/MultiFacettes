using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSouls : MonoBehaviour {

    [SerializeField] private GameObject[] Fireflies;
    public float speed;
    public float maxDist = 1f;
    private float dist = 0;
    private int moving = -1;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach(GameObject gameObject in Fireflies)
        {
            Transform transform = gameObject.transform;
            transform.position += new Vector3(0, moving * speed * Time.deltaTime, 0);
            dist += moving * speed * Time.deltaTime;
            if (dist < -maxDist || dist > maxDist)
            {
                moving = -moving;
            }
        }
    }
}
