using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlies : MonoBehaviour {

    public int activeActionIndex;
    
    [SerializeField] GameObject[] Fireflies;
    public Vector3[] FirefliesPositions;

    public Action[] actions;

    // Use this for initialization
    void Start()
    {
        actions = GetComponentsInChildren<Action>();
        FirefliesPositions = new Vector3[Fireflies.Length];
        for (int i=0; i< Fireflies.Length; i++)
        {
            FirefliesPositions[i] = Fireflies[i].transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Switch")) {
            ChangeActive();
            TurnFireflies();
        }
        if (Input.GetButtonDown("Fire")){
            TriggerActive();
        }
    }

    void TriggerActive()
    {
        Transform player = GetComponent<Transform>();
        Obstacle obstacle = actions[activeActionIndex].Activate(player.position);
        if (obstacle)
        {
            //DestroyCurrentFireFlies();
            if (actions[activeActionIndex].type == Action.ActionType.Cut
                || actions[activeActionIndex].type == Action.ActionType.Destroy
                || actions[activeActionIndex].type == Action.ActionType.Freeze)
                Debug.Log("1");
            ThrowFireFly(obstacle);
        }
    }

    void ThrowFireFly(Obstacle obstacle)
    {
        Debug.Log("2");
        actions[activeActionIndex].setObjective(obstacle);
    }

    public void DestroyCurrentFireFlies()
    {
        Debug.Log("firefly");
        //transform.GetChild(activeActionIndex).gameObject.SetActive(false);
        Fireflies[activeActionIndex].SetActive(false);
    }

    void ChangeActive()
    {
        if (activeActionIndex < Fireflies.Length - 1)
        {
            activeActionIndex += 1;
        }
        else activeActionIndex = 0;
    }

    void TurnFireflies()
    {
        for (int i = Fireflies.Length -1; i > 0; i--)
        {
            Fireflies[i].transform.position = FirefliesPositions[i - 1] + this.transform.position;
        }
        Fireflies[0].transform.position = FirefliesPositions[4] + this.transform.position;

        Vector3 vec = FirefliesPositions[4];
        for (int i = 4; i > 0; i--)
        {
            FirefliesPositions[i] = FirefliesPositions[i - 1];
        }
        FirefliesPositions[0] = vec;
    }
}
