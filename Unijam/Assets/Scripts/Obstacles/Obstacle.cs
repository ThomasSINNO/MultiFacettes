using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour {

    public enum ObstacleType
    {
        Tree, 
        Monster, 
        Water, 
        Rop, 
        Wall, 
        TypeCount
    }

    protected enum State
    {
        Default,
        Frozen,
        Cutted, 
        Destroyed,
        StateCount
    }

    public enum ColliderType
    {
        Solid, 
        Liquid, 
        None, 
        ColliderTypeCount
    }

    public ColliderType colliderType;
    protected State state = State.Default;

    [System.NonSerialized] public ObstacleType type;

    public void Start()
    {
    }

    public abstract bool Activate(Action.ActionType actionType);
    public abstract bool isActivable(Action.ActionType actionType);

    public abstract void Animate();


    public void ChargeAnimation(AnimationClip animation, string name)
    {
        Animator animatorHandler = GetComponent<Animator>();
        animatorHandler.SetBool(name, true);
    }

    protected void Update()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (colliderType == ColliderType.None)
            collider.enabled = false;
        else
            collider.enabled = true;
    }
}
