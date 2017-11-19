﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlies : MonoBehaviour {

    public int activeActionIndex;

    public bool superJump;
    
    [SerializeField]  public List<GameObject> Fireflies;
    public Vector3[] FirefliesPositions;

    public float speedRepositioning;

    bool canTurn;

    private Action[] actionsTemp;
    private List<Action> actions;

    // Use this for initialization
    void Start()
    {
        superJump = false;
        canTurn = true;
        activeActionIndex = 0;
        FirefliesPositions = new Vector3[Fireflies.Count];
        for (int i=0; i< Fireflies.Count; i++)
        {
            FirefliesPositions[i] = Fireflies[i].transform.position - this.transform.position;
        }
        actions = new List<Action>();
        actionsTemp = GetComponentsInChildren<Action>();
        for (int i = 0; i<actionsTemp.Length; i++)
        {
            actions.Add(actionsTemp[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        canTurn = true;

        for (int i = 0; i < Fireflies.Count; i++)
        {
            Action action = Fireflies[i].GetComponent<Action>();
            if (!action.hasObjectif)
            {
                float distance = FirefliesPositions[i].x + transform.position.x - action.transform.position.x;
                Debug.Log(distance);
                if (Mathf.Abs(distance) > 0.1f)
                {
                    canTurn = false;
                    float movement = speedRepositioning * Time.deltaTime;
                    float signX = distance / Mathf.Abs(distance);
                    action.transform.position += new Vector3(signX * movement, 0, 0);
                }
                else  // The Soul touched the obstacle
                {
                    if (action.isTurning)
                    {
                        if (transform.lossyScale.x < 0 && action.transform.localScale.x * transform.localScale.x > 0)
                        {
                            Debug.Log("plop");
                            action.transform.localScale = new Vector3(- action.transform.localScale.x, action.transform.localScale.y);
                        }
                        if (transform.lossyScale.x > 0 && action.transform.localScale.x * transform.localScale.x < 0)
                        {
                            Debug.Log("plop");
                            action.transform.localScale = new Vector3(-action.transform.localScale.x, action.transform.localScale.y);
                        }

                        action.isTurning = false;
                    }
                }
            }
        }

        if (Input.GetButtonDown("Switch") && canTurn)
        {
            ChangeActive();
            TurnFireflies();
        }
        if (Input.GetButtonDown("Fire"))
        {
            TriggerActive();
        }
        if (Input.GetButtonDown("Jump"))
        {
            superJump = false;
            GetComponent<Engine>().powerJump /= 2;
        }
    }

    // Move fly touched the player, he can now jump really high for 2 seconds
    public void ActivateMove()
    {
        superJump = true;
        GetComponent<Engine>().powerJump *= 2;
    }

    void TriggerActive()
    {
        
        if (actions[activeActionIndex].type == Action.ActionType.Move)
        {
            ThrowFireFly(this.transform.position);
        }
        if (actions[activeActionIndex].type == Action.ActionType.Shoot)
        {
            int sign = (int)this.transform.localScale.x;
            ThrowFireFly(sign);
        }
        
        Transform player = GetComponent<Transform>();
        Action test = actions[activeActionIndex];
        Obstacle obstacle = actions[activeActionIndex].Activate(player.position);
        if (obstacle)
        {
            Debug.Log("hi again");
            if (actions[activeActionIndex].type == Action.ActionType.Cut
                || actions[activeActionIndex].type == Action.ActionType.Destroy
                || actions[activeActionIndex].type == Action.ActionType.Freeze)
            {
                ThrowFireFly(obstacle);
            }
            
        }
    }

    void ThrowFireFly(Obstacle obstacle)
    {
        actions[activeActionIndex].SetObjective(obstacle);
    }
    
    public void ThrowFireFly(Vector3 position)
    {
        actions[activeActionIndex].SetObjective(position);
    }

    public void ThrowFireFly(int direction)
    {
        actions[activeActionIndex].SetDirection(direction);
        GameObject shootCopy = Instantiate(Fireflies[activeActionIndex]);
        shootCopy.GetComponent<Action>().SetDirection(direction);
        shootCopy.transform.position += new Vector3(this.transform.position.x, this.transform.position.y, 0);
        shootCopy.transform.localScale = new Vector3(this.transform.localScale.x, 1, 1);
        DestroyCurrentFireFlies();
    }
    
    public void DestroyCurrentFireFlies()
    {
        //transform.GetChild(activeActionIndex).gameObject.SetActive(false);
        Fireflies[activeActionIndex].gameObject.SetActive(false);
        Fireflies.RemoveAt(activeActionIndex);
        actions.RemoveAt(activeActionIndex);

        ChangeActive();
        TurnFireflies();
    }

    void ChangeActive()
    {
        activeActionIndex++;
        if (activeActionIndex >= Fireflies.Count)
        {
            activeActionIndex = 0;
        }
        
    }
    
    void TurnFireflies()
    {
        int pos = 0;
        for (int i = activeActionIndex; i<Fireflies.Count; i++)
        {
            Fireflies[i].transform.position = FirefliesPositions[pos] + this.transform.position;
            pos++;
        }

        for (int i = 0; i < activeActionIndex; i++)
        {
            Fireflies[i].transform.position = FirefliesPositions[pos] + this.transform.position;
            pos++;
        }
    }

    public void FlipPositions()
    {
        for (int i = 0; i < FirefliesPositions.Length; i++)
        {
            FirefliesPositions[i].x = -FirefliesPositions[i].x;
        }
    }
   
}