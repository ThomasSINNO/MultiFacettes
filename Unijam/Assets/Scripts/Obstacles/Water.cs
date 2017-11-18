using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Obstacle
{
    public new void Start()
    {
        colliderType = ColliderType.Liquid;
        type = Obstacle.ObstacleType.Water;
    }

    public override void Animate()
    {
        //ChargeAnimation(onDestroyed, "isDestroyed");
    }

    // Animations
    public AnimationClip onFroze;

    public override bool Activate(Action.ActionType actionType)
    {
        if (state == State.Default)
        {
            switch (actionType)
            {
                case Action.ActionType.Freeze:
                    state = State.Frozen;
                    colliderType = ColliderType.Liquid;
                    //ChargeAnimation(onFroze);
                    return true;
                case Action.ActionType.Move:
                    // doesn't affect the water
                    return true;
            }
        }
        return false;
    }

    public override bool isActivable(Action.ActionType actionType)
    {
        if (state == State.Default)
        {
            switch (actionType)
            {
                case Action.ActionType.Freeze:
                    return true;
                case Action.ActionType.Move:
                    return true;
            }
        }
        return false;
    }
}
