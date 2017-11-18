using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDeath : MonoBehaviour {

    public bool harddeath;
    private Engine eng;
    private GameObject player;
    public Vector3 sizec,posp,posc;
    public CheckPointManager checkPointManager;

	// Use this for initialization
	void Start () {
        eng = Object.FindObjectOfType<Engine>();
        player = eng.gameObject;
        posp = player.transform.position;
        posc = this.transform.position;
        sizec = this.GetComponent<Collider2D>().bounds.size;
        checkPointManager = Object.FindObjectOfType<CheckPointManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(posp.x - posc.x) < sizec.x/2)
        {
            print("0");
            if (Mathf.Abs(posp.y - posc.y) < sizec.y/2)
            {
                print("1");
                if (player)
                {
                    if (harddeath)
                    {
                        checkPointManager.Death(player);
                    }
                    else
                    {
                        checkPointManager.Sacrifice(player);
                    }
                }
                eng = Object.FindObjectOfType<Engine>();
                player = eng.gameObject;
            }
            
        }
        posp = player.transform.position;
    }
}
