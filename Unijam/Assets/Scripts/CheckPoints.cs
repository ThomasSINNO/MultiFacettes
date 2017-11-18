using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour {
    public GameObject player;
    GameObject pl;

    public Vector3 respawnPosition;
    private Vector3 restartPosition;

    public bool isAlive = true;
    public bool isRestarting = false;

	// Use this for initialization
	void Start () {
        pl = GameObject.Instantiate(player);
        respawnPosition = pl.transform.position;
        restartPosition = respawnPosition;
	}

    void Die()
    {
        isAlive = false;
    }

    void Restart()
    {
        isRestarting = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (isRestarting)
        {
            GameObject.Destroy(pl);
            pl = GameObject.Instantiate(player);
            isRestarting = false;
            respawnPosition = restartPosition;
            GameObject checkPointObject = GameObject.Find("CheckPoints");
            foreach (CheckPointsSpawn checkPoint in checkPointObject.GetComponentsInChildren<CheckPointsSpawn>())
            {
                checkPoint.passed = false;
            }
            return;
        }
		if (!isAlive)
        {
            GameObject.Destroy(pl);
            pl = GameObject.Instantiate(player);
            pl.transform.position = respawnPosition + new Vector3(0, 1, 0);
            isAlive = true;
            return;
        }
        Collider2D checkpoint = null;
        GameObject checkPoints = GameObject.Find("CheckPoints");
        foreach(Transform checkpointTransform in checkPoints.GetComponentInChildren<Transform>())
        {
            if (Mathf.Abs(checkpointTransform.position.x - pl.transform.position.x) < 1 &&
                Mathf.Abs(checkpointTransform.position.x - pl.transform.position.x) < 2)
            {
                checkpoint = checkpointTransform.GetComponent<Collider2D>();
                break;
            }
        }
        if (checkpoint != null)
        {
            CheckPointsSpawn script = checkpoint.gameObject.GetComponent<CheckPointsSpawn>();
            if (!script.passed)
            {
                script.passed = true;
                respawnPosition = script.transform.position;
            }
        }
	}
}
