using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Animator animator;

    public bool isMoving;

    public float maxSpeed;
    
    private Engine engine;

    // Use this for initialization
    void Start () {

        engine = GetComponent<Engine>();
        engine.type = Engine.Type.Player;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        engine.speed.x = maxSpeed * Input.GetAxis("Move");
        Flip(engine.speed.x);
        animator.SetBool("isMoving", this.gameObject.GetComponent<Player>().isMoving);
    }

    void FixedUpdate()
    {
        isMoving = engine.Move();

        if (Input.GetButtonDown("Jump"))
        {
            engine.Jump();
        }

        //if (Input.GetButtonDown("Dash"))
        //{
        //    engine.Dash();
        //}
    }

    void Flip(float horizontal)
    {
        bool flipGameObject = ((transform.localScale.x < 0.0f) ? (horizontal > 0.01f) : (horizontal < 0.0f));
        if (flipGameObject)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }
    
}
