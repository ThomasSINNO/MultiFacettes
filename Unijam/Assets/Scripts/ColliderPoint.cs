using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPoint : MonoBehaviour {

    public Side side;
    private Vector3 direction;

    void Update()
    {
        direction = transform.TransformDirection(Vector3.down);
    }

    public Side Collide(Vector3 speed)
    {
       
        switch(side)
        {
            case (Side.Left):
                {
                    if (Physics2D.Raycast(transform.position, direction, Mathf.Max(- speed.x * Time.fixedDeltaTime, 0.1f, 1)))
                    {
                        Debug.DrawRay(transform.position, direction, Color.blue);
                        return side;
                    }
                } break;
            case (Side.Right):
                {
                    if (Physics2D.Raycast(transform.position, direction, Mathf.Max(speed.x * Time.fixedDeltaTime, 0.1f, 1)))
                    {
                        Debug.DrawRay(transform.position, direction * speed.x * Time.fixedDeltaTime, Color.blue);
                        return side;
                    }
                }
                break;
            case (Side.Top):
                {
                    if (Physics2D.Raycast(transform.position, direction, Mathf.Max(speed.y * Time.fixedDeltaTime, 0.1f, 1)))
                    {
                        Debug.DrawRay(transform.position, direction * speed.y * Time.fixedDeltaTime, Color.blue);
                        return side;
                    }
                }
                break;
        }

        return Side.None;
    }

    public RaycastHit2D Collide(ColliderPoint colliderPoint, Vector3 speed)
    {
        Debug.DrawRay(transform.position, direction, Color.blue);
        Debug.DrawRay(colliderPoint.transform.position, direction , Color.blue);
        
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, direction, - speed.y * Time.fixedDeltaTime, 1);
        RaycastHit2D hit2 = Physics2D.Raycast(colliderPoint.transform.position, direction, - speed.y * Time.fixedDeltaTime, 1);
        //if (hit1.distance != hit2.distance && hit1.distance-hit2.distance < 2f)
        //{

        //}
        
        if (hit1)
        {
            return hit1;
        }
        else
        {
            return hit2;
        }
    }

}
