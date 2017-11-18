﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Obstacle
{

    public new void Start()
    {
        colliderType = ColliderType.Solid;
        GetComponentInParent<Obstacle>().Start();
        type = Obstacle.ObstacleType.Monster;
    }

    // Animations
    public AnimationClip onFroze;
    public AnimationClip onKill;

    public override bool Activate(Action.ActionType actionType)
    {
        if (state == State.Default)
        {
            switch (actionType)
            {
                case Action.ActionType.Destroy:
                    state = State.Destroyed;
                    ChargeAnimation(onKill);
                    colliderType = ColliderType.None;
                    this.gameObject.SetActive(false);
                    return true;
                case Action.ActionType.Freeze:
                    state = State.Frozen;
                    ChargeAnimation(onFroze);
                    return true;
            }       
        }
        else if (state == State.Frozen && actionType == Action.ActionType.Destroy)
        {
            state = State.Destroyed;
            ChargeAnimation(onKill);
            return true;
        }
        return false;
    }
}