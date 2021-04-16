using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    public float jumpHeight;

    //action states
    private bool grounded;
    private bool falling;
    private bool jumped;
    private bool peeking = false;

    //Using new jump mechanics
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D rb;

    private SpriteRenderer sr;

    private Animator anim;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        /*~~~~~~~~TIME SHIFT~~~~~~~*/
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

        if (!peeking)
        {
            //Shift Worlds
            if (Input.GetKeyDown("q") && grounded)
            {
                LevelManager.instance.switchWorld();
            }

            /*~~~~~~~~~~MOVEMENT~~~~~~~~~~~~*/

            rb.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), rb.velocity.y);
            
            //check if player is falling
            if (rb.velocity.y < 0)
            {
                falling = true;
            }
            else
            {
                falling = false;
            }
            //If Player is on the ground
            if (rb.velocity.y < 0.005 && rb.velocity.y > -0.005)
            {
                grounded = true;
                falling = false;
                jumped = false;
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            else
            {
                grounded = false;
            }
            //Jump
            if (Input.GetButtonDown("Jump") && grounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                jumped = true;
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
            }
            else if (rb.velocity.x > 0 || Input.GetKey("d"))
            {
                //sr.flipX = false;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            //Stop player when they are peeking
            rb.velocity = new Vector2(0,0);
        }

        //Animation
        anim.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("falling", falling);
        anim.SetBool("jumped", jumped);

    }
}
