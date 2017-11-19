using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour {

    public bool trueDeath;
    [SerializeField] private CheckPointManager checkPointManager;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        player = checkPointManager.player;
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < GetComponent<Collider2D>().bounds.size.x/2)
        {
            if (Mathf.Abs(player.transform.position.y - transform.position.y) < GetComponent<Collider2D>().bounds.size.y / 2)
            {
                if (trueDeath)
                {
                    checkPointManager.Death();
                }
                else
                {
                    checkPointManager.Sacrifice();
                }
            }     
        }
    }
}
