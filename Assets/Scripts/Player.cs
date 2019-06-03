using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    // Reference to the speed and gravity
    public float moveSpeed = 10f;
    public float gravity = -10f;
    public float jumpHeight = 7f;

    public float centreRadius = .1f;

    // Reference to controller and animation
    private CharacterController2D controller;
    private SpriteRenderer rend;
    private Animator anim;

    private Vector3 velocity;
    private bool isClimbing = false; // Is in climbing state

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, centreRadius);
    }

    void Start()
    {
        // Getting the components in the game
        controller = GetComponent<CharacterController2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Gathering the Input
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        // If controller is NOT grounded
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        // Get Spacebar input
        bool isJumping = Input.GetButtonDown("Jump");
        // If Player pressed jump
        if (isJumping)
        {
            // Make the controller jump
            Jump();
        }
        // Call the animator to jump
        anim.SetBool("IsGrounded", controller.isGrounded);
        anim.SetFloat("JumpY", velocity.y);

        Run(inputH);
        Climb(inputV);

        // Applies velocity to controller (to get it to move)
        controller.move(velocity * Time.deltaTime);
    }

    void Run(float inputH)
    {
        // Move the character controller left / right with input
        velocity.x = inputH * moveSpeed;
        // Set bool to true is input is pressed
        bool isRunning = inputH != 0; // Detect Movement (Running)
        // Animate the player to running if input is pressed
        anim.SetBool("IsRunning", isRunning);
        // Detecting and fliping character left and right 
        if (isRunning)
        {
            rend.flipX = inputH < 0;
        }
    }

    void Climb(float inputV)
    {
        bool isOverLadder = false; // Is overlapping ladder
        // Get a list of all hit objects overlapping point
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        // Loop through each point
        foreach (var hit in hits)
        {
            // If point overlaps a climbable object
            if(hit.tag == "Ladder")
            {
                // Player is overlapping ladder!
                isOverLadder = true;
                break; // Exit foreach loop
            }
        }
        // If is overlapping ladder and inputV has been made
        if(isOverLadder && inputV !=0)
        {
            // Is Climbing
            isClimbing = true;
        }
        // If is Climbing
        // Perform logic from climbing
    }

    void Jump()
    {
        // Set velocity's Y to height
        velocity.y = jumpHeight;
    }
}