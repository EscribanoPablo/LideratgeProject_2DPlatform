using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] KeyCode jumpKeyCode = KeyCode.Space;

    [SerializeField] float speedMovement;
    [SerializeField] float jumpSpeed;
    [SerializeField] float _maxVerticalVelocity;

    public float Gravity => rigidbody.gravityScale;
    public bool IsFalling => rigidbody.velocity.y < 0 && !canJump;
    public bool IsOnGround => canJump;
    public float MaxVerticalVelocity => _maxVerticalVelocity;

    Rigidbody2D rigidbody;
    bool canJump;
    [SerializeField] LayerMask whatIsGround; 
    [SerializeField] Transform groundChecker;

    Quaternion startRotation;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startRotation = transform.rotation;
    }

    void Update()
    {
        PlayerMovement();
        CheckIfOnGround();
        Jumper();
        transform.rotation = startRotation; 
    }

    private void PlayerMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        float verticalVelocity = rigidbody.velocity.y;
        if (verticalVelocity < (-_maxVerticalVelocity))
            verticalVelocity = -_maxVerticalVelocity;

        Vector2 direction = new Vector2(horizontalMovement * speedMovement, verticalVelocity);

        rigidbody.velocity = direction; 
    }
    private bool CheckIfOnGround()
    {
        float detectionRadius = 0.15f;
        var colliders = Physics2D.OverlapCircleAll(groundChecker.position, detectionRadius, whatIsGround);
        canJump = (colliders.Length > 0);
        return canJump;
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
            //Debug.Log("jump");
            rigidbody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }

    public void SetGravity(float amount)
    {
        rigidbody.gravityScale = amount;
    }

    public void StopVerticalVelocity()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
    }

    public void SetMaxVerticalVelocity(float amount)
    {
        _maxVerticalVelocity = amount;
    }

}
