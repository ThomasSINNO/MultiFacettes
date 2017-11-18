using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlies : MonoBehaviour {

    public int activeActionIndex;
    
    [SerializeField]  private List<GameObject> Fireflies;  // ignore the debug error
    public Vector3[] FirefliesPositions;

    private Action[] actionsTemp;
    private List<Action> actions;

    // Use this for initialization
    void Start()
    {
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
        Action test = actions[activeActionIndex];
        Obstacle obstacle = actions[activeActionIndex].Activate(player.position);
        if (obstacle)
        {
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

    public void DestroyCurrentFireFlies()
    {
        DestroyFireFlies(activeActionIndex);
    }

    public void DestroyFireFlies(int index)
    {
        //transform.GetChild(activeActionIndex).gameObject.SetActive(false);
        Fireflies[index].gameObject.SetActive(false);
        Fireflies.RemoveAt(index);
        actions.RemoveAt(index);
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
    
}
