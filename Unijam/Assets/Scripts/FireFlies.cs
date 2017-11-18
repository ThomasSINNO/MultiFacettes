using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlies : MonoBehaviour {

    public int activeActionIndex;
    
    [SerializeField]  public List<GameObject> Fireflies;
    public Vector3[] FirefliesPositions;

    public float speedRepositioning;

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

        for (int i = 0; i < Fireflies.Count - 1; i++)
        {
            Action action = Fireflies[i].GetComponent<Action>();
            if (!action.hasObjectif)
            {
                Vector2 distance = new Vector2(FirefliesPositions[i].x - action.transform.position.x, 0);
                if (distance.magnitude > 0.1f)
                {
                    float movement = speedRepositioning * Time.deltaTime;
                    float percentX = Mathf.Abs(distance.x) / distance.magnitude;
                    float signX = distance.x / Mathf.Abs(distance.x);
                    this.transform.position += new Vector3(signX * movement * percentX, 0, 0);
                }
                else  // The Soul touched the obstacle
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
                }
            }
        }
    }

    void TriggerActive()
    {
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
        for (int i = 0; i < FirefliesPositions.Length - 1; i++)
        {
            FirefliesPositions[i].x = -FirefliesPositions[i].x;
        }
    }
   
}
