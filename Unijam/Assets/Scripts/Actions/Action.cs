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
    
    public Obstacle objectif;
    public bool hasObjectif;
    [SerializeField] private float speed;
    [SerializeField] protected float actionRadius;
    [SerializeField] protected float actionRadiusSoul;
    public ActionType type;

    private void Start()
    {
        hasObjectif = false;
        objectif = null;
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
        //this.transform.parent = null;
    }

    public void Update()
    {
        if (objectif)
        {
            LucioleAnimation lucioleScript = GetComponent<LucioleAnimation>();
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
                if (type == ActionType.Destroy || type == ActionType.Cut)
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


                //Destroy(this.gameObject);
                objectif = null;
                
            }
        }
    }
}
