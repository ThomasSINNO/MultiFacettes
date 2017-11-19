using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : PersistentSingleton<CheckPointManager> {

    // Use this for initialization

    public CheckPoint[] checkPoints;
    private CheckPoint lastCheckpoint;
    public GameObject player;
    private Animator animator;
    [SerializeField] private CameraFollowPlayer camera;

	void Start ()
    {
        checkPoints = GetComponentsInChildren<CheckPoint>();
        foreach (CheckPoint cp in checkPoints)
        {
            cp.player = player;
        }
        camera.player = player;
    }

    public void Sacrifice()
    {
        player.GetComponent<Animator>().SetBool("isDying", true);
        GameObject newPlayer = Instantiate(player, lastCheckpoint.transform.position + new Vector3(1,1), Quaternion.identity);
        camera.player = newPlayer;
        foreach (CheckPoint cp in checkPoints)
        {
            cp.player = newPlayer;
        }
        Destroy(player);
        this.player = newPlayer;
    }

    public void Death()
    {
        player.GetComponent<Animator>().SetBool("isDying", true);
        GameObject newPlayer = Instantiate(player, checkPoints[0].transform.position + new Vector3(1, 1), Quaternion.identity);
        camera.player = newPlayer;
        foreach (CheckPoint cp in checkPoints)
        {
            cp.player = newPlayer;
        }
        Destroy(player);
        this.player = newPlayer;

        foreach (CheckPoint cp in checkPoints)
        {
            if (cp!=checkPoints[0]) cp.shutdownCheckPoint();
        }
    }

    // Update is called once per frame
    void Update () {

        foreach (CheckPoint cp in checkPoints)
        {
            if (cp.isSet)
            {
                lastCheckpoint = cp;
            }
        }  
    }
}
