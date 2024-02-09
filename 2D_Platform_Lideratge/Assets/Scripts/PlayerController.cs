using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] KeyCode jumpKeyCode = KeyCode.Space;

    [SerializeField] float speedMovement;
    [SerializeField] float jumpSpeed;

    Rigidbody2D rigidbody;
    bool canJump;
    [SerializeField] LayerMask whatIsGround; 
    [SerializeField] Transform groundChecker;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
       
    }

    void Update()
    {
        PlayerMovement();
        CheckIfOnGround();
        Jumper(); 
    }

    private void PlayerMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontalMovement * speedMovement, rigidbody.velocity.y);

        rigidbody.velocity = direction; 
    }
    private void CheckIfOnGround()
    {
        float detectionRadius = 0.15f;
        var colliders = Physics2D.OverlapCircleAll(groundChecker.position, detectionRadius, whatIsGround);
        canJump = (colliders.Length > 0);
    }

    private void Jumper()
    {
        if (canJump)
        {
            Jump();
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(jumpKeyCode))
        {
            Debug.Log("jump");
            rigidbody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }

}
