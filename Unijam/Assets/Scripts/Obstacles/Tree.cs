using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Obstacle
{
    public new void Start()
    {
        base.Start();
        colliderType = ColliderType.Solid;
        type = Obstacle.ObstacleType.Tree;
    }

    public new void Update()
    {
        base.Update();

        // Restart the collider to fetch the tree is cut
        Collider2D collider = GetComponent<Collider2D>();
        collider = new Collider2D();
    }

    // Animations
    public AnimationClip onCut;
    public AnimationClip onDestroyed;

    public override bool Activate(Action.ActionType actionType)
    {
        if (state == State.Default)
        {
            switch (actionType)
            {
                case Action.ActionType.Cut:
                    state = State.Cutted;
                    ChargeAnimation(onCut);
                    colliderType = ColliderType.None;
                    this.transform.position += new Vector3(2, 0, 0);  // simulate the falling of the tree, temporary !
                    this.gameObject.SetActive(false);
                    return true;
                case Action.ActionType.Destroy:
                    state = State.Destroyed;
                    ChargeAnimation(onDestroyed);
                    colliderType = ColliderType.None;
                    this.gameObject.SetActive(false);
                    return true;
            }
        }
        return false;
    }
}
