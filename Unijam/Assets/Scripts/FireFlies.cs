using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlies : MonoBehaviour {

    public int activeActionIndex;
    
    [SerializeField]  private List<GameObject> Fireflies;  // ignore the debug error
    public Vector3[] FirefliesPositions;
    private bool superJump;

    private Action[] actionsTemp;
    private List<Action> actions;

    // Use this for initialization
    private void Start()
    {
        superJump = false;
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

    // return true is a fly is in direction of an objectif, player, etc...
    private bool IsFliesMoving()
    {
        foreach(Action action in actions)
        {
            if (action.hasObjectif)
            {
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Switch")) {
            ChangeActive();
            TurnFireflies();
        }
        if (Input.GetButtonDown("Fire")){
            TriggerActive();
        }
        // only one super jump alawed
        if (Input.GetButtonDown("Jump") && superJump)
        {
            superJump = false;
            GetComponent<Engine>().powerJump /= 2;
        }
    }

    public void TriggerActive()
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
            if (actions[activeActionIndex].type == Action.ActionType.Cut
                || actions[activeActionIndex].type == Action.ActionType.Destroy
                || actions[activeActionIndex].type == Action.ActionType.Freeze)
            {
                ThrowFireFly(obstacle);
            }
        }
    }

    // Move fly touched the player, he can now jump really high for 2 seconds
    public void ActivateMove()
    {
        superJump = true;
        GetComponent<Engine>().powerJump*=2;
    }

    public void ThrowFireFly(Obstacle obstacle)
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

    public void ChangeActive()
    {
        activeActionIndex++;
        if (activeActionIndex >= Fireflies.Count)
        {
            activeActionIndex = 0;
        }
        
    }

    public void TurnFireflies()
    {
        // can't change if a fire fly is activate
        if (IsFliesMoving())
        {
            return;
        }

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
