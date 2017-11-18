using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour {

    public enum ActionType
    {
        Cut,
        Destroy,
        Shoot,
        Move,
        Freeze
    };

    [SerializeField] protected float actionRadius;
    public ActionType type;

    public bool Activate(Vector3 positionPlayer)
    {
        bool somethingDestroy = false;
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(positionPlayer, actionRadius))
        {
            Obstacle obstacle = collider.gameObject.GetComponent<Obstacle>();
            if (obstacle)
            {
                if (obstacle.Activate(type))
                {
                    somethingDestroy = true;
                    Destroy(this);
                    break;
                }
                    
            }
        }
        if (somethingDestroy)
        {
            Debug.Log("something destroyed !");
        }
        return somethingDestroy;
    }
}
