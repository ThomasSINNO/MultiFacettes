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

    public bool isTurning;
    // The objectif can be an obstacle or a position (i.e the player position for move)
    public Obstacle objectif;
    public Vector3 objectifPosition;
    public int direction;

    public bool hasObjectif;  // is Flying to something, obstacle or just in a direction with shoot

    [SerializeField] private float speed;
    [SerializeField] protected float actionRadius;
    [SerializeField] protected float actionRadiusSoul;
    public ActionType type;

    private void Start()
    {
        isTurning = false;
        direction = 0;
        hasObjectif = false;
        objectif = null;
        objectifPosition = Vector3.one;
        speed = 1f;
    }

    public Obstacle Activate(Vector3 positionPlayer)
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(positionPlayer, actionRadius))
        {
            Obstacle obstacle = collider.gameObject.GetComponent<Obstacle>();
            if (obstacle)
            {
                if (obstacle.isActivable(type))
                {
                    return obstacle;
                }
            }
        }
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(positionPlayer, actionRadius, 1<<LayerMask.NameToLayer("NoCollision")))
        {
            Obstacle obstacle = collider.gameObject.GetComponent<Obstacle>();
            if (obstacle)
            {
                if (obstacle.isActivable(type))
                {
                    return obstacle;
                }
            }
        }
        return null;
    }

    public void SetObjective(Obstacle obstacle)
    {
        objectif = obstacle;
        hasObjectif = true;
    }

    public void SetObjective(Vector3 obj)
    {
        objectifPosition = obj;
        hasObjectif = true;
    }

    public void SetDirection(int dir)
    {
        direction = dir;
    }

    public void UpdatePositionObjectif()
    {
        Vector3 globalPosition = GameObject.Find(this.name).transform.position;
        Vector2 distance = new Vector2(objectifPosition.x - globalPosition.x,
                                        objectifPosition.y - globalPosition.y);
        if (distance.magnitude > 0.1f)
        {
            float movement = speed * Time.deltaTime;
            float percentX = Mathf.Abs(distance.x) / distance.magnitude;
            float signX = distance.x / Mathf.Abs(distance.x);
            float signY = distance.y / Mathf.Abs(distance.y);
            this.transform.position += new Vector3(signX * movement * percentX, signY * movement * (1 - percentX), 0);
        }
        else  // The Soul touched the obstacle
        {
            if (type == ActionType.Cut || type == ActionType.Destroy)
            {
                objectif.GetComponent<Obstacle>().Animate();
                Destroy(objectif.GetComponent<PolygonCollider2D>());
            }
            else if (type == ActionType.Freeze)
            {
                objectif.Activate(ActionType.Freeze);
            }

            FireFlies script = GetComponentInParent<FireFlies>();
            if (type == ActionType.Move)
            {
                script.ActivateMove();
            }
            script.DestroyCurrentFireFlies();
            objectif = null;

        }
    }

    public void UpdateObstacleObjectif()
    {
        Vector3 globalPosition = GameObject.Find(this.name).transform.position;
        Vector3 globalObjectif = GameObject.Find(objectif.name).transform.position;
        Vector2 distance = new Vector2(globalObjectif.x - globalPosition.x,
                                        globalObjectif.y - globalPosition.y);
        if (distance.magnitude > 0.1f)
        {
            float movement = speed * Time.deltaTime;
            float percentX = Mathf.Abs(distance.x) / distance.magnitude;
            float signX = distance.x / Mathf.Abs(distance.x);
            float signY = distance.y / Mathf.Abs(distance.y);
            this.transform.position += new Vector3(signX * movement * percentX, signY * movement * (1 - percentX), 0);
        }
        else  // The Soul touched the obstacle
        {
            if (type == ActionType.Cut || type == ActionType.Destroy)
            {
                objectif.GetComponent<Obstacle>().Animate();
                Destroy(objectif.GetComponent<PolygonCollider2D>());
            }
            else if (type == ActionType.Freeze)
            {
                objectif.Activate(ActionType.Freeze);
            }

            FireFlies script = GetComponentInParent<FireFlies>();
            script.DestroyCurrentFireFlies();
            objectif = null;

        }
    }

    public void Update()
    {
        if (hasObjectif)
        {
            if (objectif)
            {
                UpdateObstacleObjectif();
            }
            else
            {
                UpdatePositionObjectif();
            }
        }

        // does something only if the shoot is activated (direction != 0)
        if (type == ActionType.Shoot && direction != 0)
        {
            this.transform.position += new Vector3(direction * speed * Time.deltaTime, 0, 0);
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(this.transform.position, actionRadius))
            {
                Obstacle obstacle = collider.gameObject.GetComponent<Obstacle>();
                if (obstacle)
                {
                    if (obstacle.isActivable(type))
                    {
                        SetObjective(obstacle);
                        direction = 0;
                    }
                }
            }
        }
    }
}
