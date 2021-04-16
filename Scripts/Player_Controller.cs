using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public static Player_Controller instance;

    public float moveSpeed;
    public float jumpHeight;

    private bool doubleJumped;
    private bool grounded;
    private bool peeking;
    private bool crouch;
    private bool falling;
    private bool direction;     //direction player is facing
    private bool headbonk;      //if player should not be standing up


    //Using new jump mechanics
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Peek Worlds
        if (Input.GetKey("e") && grounded)
        {
            LevelManager.instance.peekWorld();
            peeking = true;
        }
        else if (Input.GetKeyUp("e") && grounded)
        {
            LevelManager.instance.unPeekWorld();
            peeking = false;
        }
        //Stop player when they are peeking worlds
        if (!peeking)
        {
            //Shift Worlds
            if (Input.GetKeyDown("q") && grounded)
            {
                LevelManager.instance.switchWorld();
            }

            //Player is falling
            if (rb.velocity.y < 0)
            {
                falling = true;     //character has to be falling to be able to wall jump again
                                    //wallJumped = false; //character is only able to wall jump once

                //if character falls of while sliding
                //sliding = false;
                //slideTimerCurrent = 0;
            }

            //If Player is on the ground
            if (rb.velocity.y < 0.005 && rb.velocity.y > -0.005)
            {
                grounded = true;
                doubleJumped = false;
                falling = false;
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            else
            {
                grounded = false;
            }

            //Movement
            if (!crouch)
            {
                rb.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), rb.velocity.y);
            }

            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            //Crouching
            if (Input.GetKey("s") && grounded || headbonk && grounded)
            {
                crouch = true;
            }
            else
            {
                crouch = false;
            }

            //Jump
            if (Input.GetButtonDown("Jump") && grounded && !crouch)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            }

            //if player is falling and Better Jump Mechanics
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

            //Animation Stuff
            //Flip the sprite
            if (rb.velocity.x < 0 || Input.GetKey("a"))
            {
                //sr.flipX = true;
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                direction = true;
            }
            else if (rb.velocity.x > 0 || Input.GetKey("d"))
            {
                //sr.flipX = false;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                direction = false;
            }
        }
        else
        {
            //Stop player when they are peeking
            rb.velocity = new Vector2(0,0);
        }

        //Animation stuff
        anim.SetFloat("velocity", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("doubleJumped", doubleJumped);
        anim.SetBool("crouch", crouch);
    }
}


