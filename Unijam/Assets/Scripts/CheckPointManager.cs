using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : PersistentSingleton<CheckPointManager> {

    // Use this for initialization

    public CheckPoints[] checkPoints;
    public bool[] testindexes;
    public CheckPoints lastchkpnt;
    public CheckPoints firstchkpnt;
	void Start () {
        checkPoints = GetComponentsInChildren<CheckPoints>();
        testindexes = new bool[checkPoints.Length];
        for(int i = 0; i < checkPoints.Length; i++)
        {
            
            testindexes[i] = true;
        }
        //foreach(CheckPoints g in checkPoints)
        //{
        //    if (g.isLast == true)
        //    {
        //        firstchkpnt = g;
        //        lastchkpnt = g;
        //    }
        //}

	}

    public void Sacrifice(GameObject g)
    {
        g.transform.position = lastchkpnt.transform.position;
    }

    public void Death(GameObject g)
    {
        g.transform.position = firstchkpnt.transform.position;
        for (int i = 0; i < checkPoints.Length; i++)
        {
            checkPoints[i].shutdownCheckPoint();
            testindexes[i] = true;
        }

    }

    // Update is called once per frame
    void Update () {

        for (int i = 0; i < checkPoints.Length; i++)
        {
            if (testindexes[i])
            {
                if (checkPoints[i].isSet)
                {
                    lastchkpnt = checkPoints[i];
                    if(!firstchkpnt) firstchkpnt = checkPoints[i];
                    testindexes[i] = false;
                }
            }
        }
        //foreach (CheckPoints g in checkPoints)
        //{
        //    if (g.GetComponent<CheckPoints>().isLast == true)
        //    {
        //        lastchkpnt = g;
        //    }
        //}
    }
}
