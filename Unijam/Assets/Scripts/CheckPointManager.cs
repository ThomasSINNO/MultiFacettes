using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointManager : PersistentSingleton<CheckPointManager> {

    // Use this for initialization

    public CheckPoint[] checkPoints;
    private CheckPoint lastCheckpoint;

    private Vector3 respawnPoint;
    public GameObject player;
    private Animator animator;
    private int justDied;
    [SerializeField] private CameraFollowPlayer camera;

	void Start ()
    {
        justDied = 0;
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
        Application.LoadLevel(Application.loadedLevel);
        player.transform.position = checkPoints[1].transform.position;
        justDied = 2;
        respawnPoint = lastCheckpoint.transform.position;
    }

    public void Death()
    {
        player.GetComponent<Animator>().SetBool("isDying", true);
        Application.LoadLevel(Application.loadedLevel);
        player.transform.position = checkPoints[1].transform.position;
        justDied = 2;
        respawnPoint = checkPoints[0].transform.position;
    }

    // Update is called once per frame
    void Update () {
        player = GameObject.Find("PlayerFinal");
        if (justDied!=0)
        {
            player.transform.position = checkPoints[1].transform.position;
            justDied -=1;
        }
        foreach (CheckPoint cp in checkPoints)
        {
            if (cp.isSet)
            {
                lastCheckpoint = cp;
            }
        }  
    }
}
