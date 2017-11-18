using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rop : Obstacle
{
    public new void Start()
    {
        type = Obstacle.ObstacleType.Rop;
        colliderType = ColliderType.None;
    }

    // Animation
    public AnimationClip onCut;

    public override void Animate()
    {
        //ChargeAnimation(onDestroyed, "isDestroyed");
    }

    public override bool Activate(Action.ActionType actionType)
    {
        if (state == State.Default)
        {
            if (actionType == Action.ActionType.Cut || actionType == Action.ActionType.Shoot)
            {
                state = State.Cutted;
                //ChargeAnimation(onCut);
                return true;
            }
        }
        return false;
    }

    public override bool isActivable(Action.ActionType actionType)
    {
        if (state == State.Default)
        {
            if (actionType == Action.ActionType.Cut || actionType == Action.ActionType.Shoot)
            {
                return true;
            }
        }
        return false;
    }
}
