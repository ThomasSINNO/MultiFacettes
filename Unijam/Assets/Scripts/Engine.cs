using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public Vector3 speed;
    public float gravity = 10f;
    public float powerJump;
    public float wallJumpImpulse;
    public float slownessWall = 1;  // coheficient that decrease speed.y when the player is against a wall

    public float dashCoolDown;
    public float decrementDashPower = 2;  // diminution of the speed procured by the dash by seconds
    private float timeSinceLastDash;
    public float dashPower = 5;
    private float dashValue = 0;

    public enum Type
    {
        Player,
        Platform
    };

    public Type type;

    private Animator animator;
    private bool collisionLeft = false;
    private bool collisionRight = false;
    private bool isAgainstAWall = false;
    public bool isJumping = false;
    private int lastWallJump = 0;   // 0 - No jump, 1 - Right wall jump, -1 - Left wall jump

    public ColliderPoint[] collisionPointsBottom;
    private ColliderPoint[] collisionPointsSides;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (transform.Find("Collision Bottom"))
        {
            collisionPointsBottom = transform.Find("Collision Bottom").GetComponentsInChildren<ColliderPoint>();
        }
        if (transform.Find("Collision Sides"))
        {
            collisionPointsSides = transform.Find("Collision Sides").GetComponentsInChildren<ColliderPoint>();
        }

        timeSinceLastDash = dashCoolDown;
    }

    public void InvertColliders()
    {
        foreach (ColliderPoint collider in collisionPointsSides)
        {
            if (collider.side == Side.Right) collider.side = Side.Left;
            else if (collider.side == Side.Left) collider.side = Side.Right;
        }
    }

    public bool Move()
    {
        if (type == Type.Player)
        {
            if (isAgainstAWall)
            {
                speed.y -= gravity * slownessWall * Time.fixedDeltaTime;
            }
            else
            {
                speed.y -= gravity * Time.fixedDeltaTime;
            }
            CollisionTest();
        }
        else speed.y -= gravity * Time.fixedDeltaTime;
        transform.position += new Vector3(speed.x, speed.y) * Time.deltaTime;
        if (Mathf.Abs(speed.x) > 0.2 || Mathf.Abs(speed.y) >0.2) return true;
        return false;
    }

    public void Jump()
    {
        if (collisionLeft && lastWallJump != -1)
        {
            speed.y = powerJump;
            lastWallJump = -1;
            speed.x += wallJumpImpulse; // we add a small impulsion
        }
        else if (collisionRight && lastWallJump != 1)
        {
            speed.y = powerJump;
            lastWallJump = 1;
            speed.x -= wallJumpImpulse; // we add a small impulsion
        }
        else if (!isJumping)
        {
            speed.y = powerJump;
            isJumping = true;   // normal jump cost 1 point of isJumping
        }
    }

    //// return the extra speed procured by the dash
    //public void Dash()
    //{
    //    if (timeSinceLastDash > dashCoolDown)
    //    {
    //        if (speed.x < 0)
    //        {
    //            dashValue = - dashPower;
    //        }
    //        if (speed.x > 0)
    //        {
    //            dashValue = dashPower;
    //        }
    //    }
    //}

    void CollisionTest()
    {
        isAgainstAWall = false;  // reboot
        foreach (ColliderPoint point in collisionPointsSides)
        {
            Side collisionSide = point.Collide(speed);
            switch (collisionSide)
            {
                case (Side.Left):
                    {
                        // update the wall jumps
                        if (!collisionLeft && isJumping)
                        {
                            collisionLeft = true;
                            collisionRight = false;
                        }
                        //Debug.Log("Collision Left");
                        if (speed.x < 0) speed.x = 0;
                        isAgainstAWall = true;
                    }
                    break;
                case (Side.Right):
                    {
                        // update the wall jumps
                        if (!collisionRight && isJumping)
                        {
                            collisionRight = true;
                            collisionLeft = false;
                        }
                        if (speed.x > 0) speed.x = 0;
                        isAgainstAWall = true;
                    }
                    break;
                case (Side.Top):
                    {
                        if (speed.y > 0) speed.y = 0;
                    }
                    break;
            }
        }

        RaycastHit2D hit = collisionPointsBottom[0].Collide(collisionPointsBottom[1], speed);
        if (hit)
        {
            // We reboot the wall jumps
            collisionRight = false;
            collisionLeft = false;
            lastWallJump = 0;

            isJumping = false;
            animator.SetBool("isJumping", false);

            speed.y = 0;
            //if (hit.collider.transform.GetComponent<MovingPlatform>())
            //{
            //    MovingPlatform platform = hit.collider.transform.GetComponent<MovingPlatform>();
            //    if (platform.direction == MovingPlatform.Direction.X) speed.x += hit.collider.transform.GetComponent<Engine>().speed.x;
            //    if (platform.direction == MovingPlatform.Direction.Y && speed.y >=0) speed.y += hit.collider.transform.GetComponent<Engine>().speed.y;
            //}
            //if (hit.collider.transform.GetComponent<JumpingPlatform>())
            //{
            //    JumpingPlatform platform = hit.collider.transform.GetComponent<JumpingPlatform>();
            //    speed.y = platform.jumpPower;
            //}
            //if (hit.collider.transform.GetComponent<KillingPlatform>())
            //{
            //    transform.position = new Vector3(-19, -10);
            //    speed = new Vector3(0, 0);
            //}

        }
    }

    void Update()
    {

        animator.SetBool("isJumping", isJumping);
        if (type ==Type.Player)
        {
            timeSinceLastDash += Time.deltaTime;

            if (dashValue < 0)
            {
                dashValue += decrementDashPower * Time.deltaTime;
            }
            else
            {
                dashValue -= decrementDashPower * Time.deltaTime;
            }

            speed.x += dashValue;
        }
    }    
}