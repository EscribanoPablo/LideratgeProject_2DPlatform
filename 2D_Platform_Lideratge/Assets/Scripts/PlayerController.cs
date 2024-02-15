using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] KeyCode jumpKeyCode = KeyCode.Space;
    [SerializeField] KeyCode dashKeyCode = KeyCode.LeftShift;


    [Header("Attributes")]
    [Range(0, 20)][SerializeField] float speedMovement;
    [Range(0, 20)][SerializeField] float jumpSpeed;
    [Range(0, 20)][SerializeField] private float _maxVerticalVelocity = 10;
    [Range(0, 20)][SerializeField] private float _maxHorizontalVelocity = 10;
    [Range(10, 50)] [SerializeField] private float _dashPower = 30;
    [Range(0, 2)] [SerializeField] private float _dashDuration = 0.2f;


    [Header("References")]
    [SerializeField] LayerMask whatIsGround; 
    [SerializeField] Transform groundChecker;
    [SerializeField] TrailRenderer _trailRenderer;

    #region Getters

    public float Gravity => rigidbody.gravityScale;
    public bool IsFalling => rigidbody.velocity.y < 0 && !canJump;
    public bool IsOnGround => canJump;
    public float MaxVerticalVelocity => _maxVerticalVelocity;
    public float MaxHorizontalVelocity => _maxVerticalVelocity;

    #endregion

    private Rigidbody2D rigidbody;
    private Quaternion startRotation;

    private bool canJump;
    private bool canDash = true;
    private bool isDashing;
    private float coolDown = 1;

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
        PlayerDash(); 
        
        transform.rotation = startRotation; 
    }

    private void PlayerMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontalMovement * speedMovement, rigidbody.velocity.y);

        rigidbody.velocity = direction;

        //Clamp the velocity
        Vector2 clampedVelocity = GetClampedVelocities();
        rigidbody.velocity = new Vector2(clampedVelocity.x, clampedVelocity.y);
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

    private Vector2 GetClampedVelocities()
    {
        float verticalVelocity = rigidbody.velocity.y;
        if (verticalVelocity < (-_maxVerticalVelocity))
            verticalVelocity = -_maxVerticalVelocity;
        
        float horizontalVelocity = Math.Clamp(rigidbody.velocity.x, 
                                            -_maxHorizontalVelocity, _maxHorizontalVelocity);

        return new Vector2(horizontalVelocity, verticalVelocity);
    }

    public void SetGravity(float amount)
    {
        rigidbody.gravityScale = amount;
    }

    public void StopVerticalVelocity()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
    }

    public void SetMaxVelocity(Vector3 vel)
    {
        _maxHorizontalVelocity = vel.x;
        _maxVerticalVelocity = vel.y;
    }

    private void PlayerDash()
    {
        if (isDashing) return;
        if (Input.GetKeyDown(dashKeyCode) && canDash)
        {
            Debug.Log("Dash");
            StartCoroutine(DoDash());
        }
        
    }

    private IEnumerator DoDash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rigidbody.gravityScale;
        rigidbody.gravityScale = 0f;

        float XmovementDash = rigidbody.velocity.x;
        XmovementDash *= _dashPower;
        rigidbody.velocity = new Vector2(XmovementDash, 0f);
        _trailRenderer.emitting = true;

        yield return new WaitForSeconds(_dashDuration);
        
        rigidbody.gravityScale = originalGravity;
        _trailRenderer.emitting = false;
        isDashing = false;

        yield return new WaitForSeconds(coolDown);
        canDash = true; 
    }

}
