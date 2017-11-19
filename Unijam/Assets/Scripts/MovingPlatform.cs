using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    // Use this for initialization

    // Position of the two limit points of the movement
    public float start;
    public float end;

    private float timer;

    public enum Direction
    {
        X,
        Y
    }

    public Direction direction;

    public float maxSpeed = 1;

    private Engine engine;

	void Start () {
        engine = GetComponent<Engine>();
        engine.type = Engine.Type.Platform;

        switch (direction)
        {
            case (Direction.X):
                {
                    engine.speed = new Vector3(maxSpeed, 0, 0);
                }
                break;
            case (Direction.Y):
                {
                    engine.speed = new Vector3(0, maxSpeed, 0);
                }
                break;
        }

        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {

        timer -= Time.deltaTime;

        switch (direction)
        {
            case (Direction.X):
                {
                    if ((transform.position.x > end || transform.position.x < start) && timer <= 0)
                    {
                        engine.speed.x = -engine.speed.x;
                        timer = 0.1f;
                    }
                }
                break;
            case (Direction.Y):
                {
                    if ((transform.position.y > end || transform.position.y < start) && timer <= 0)
                    {
                        engine.speed.y = -engine.speed.y;
                        timer = 0.1f;
                    }
                }
                break;
        }
    }

    void FixedUpdate ()
    {
        engine.Move();
    }
}
