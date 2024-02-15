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
    [SerializeField] private int _maxJumpCount = 1;
    [Range(0, 20)][SerializeField] private float _maxVerticalVelocity = 10;
    [Range(0, 20)][SerializeField] private float _maxHorizontalVelocity = 10;
    [Range(10, 50)] [SerializeField] private float _dashPower = 30;
    [Range(0, 2)] [SerializeField] private float _dashDuration = 0.2f;


    [Header("References")]
    [SerializeField] LayerMask whatIsGround; 
    [SerializeField] Transform groundChecker;
    [SerializeField] TrailRenderer _trailRenderer;
    [SerializeField] private GameObject _VFX;

    #region Getters

    public float Gravity => rigidbody.gravityScale;
    //public bool IsFalling => rigidbody.velocity.y < 0 && !CheckIfOnGround();
    public bool IsOnGround => CheckIfOnGround();
    public float MaxVerticalVelocity => _maxVerticalVelocity;
    public float MaxHorizontalVelocity => _maxVerticalVelocity;

    public bool CanOpenUmbrella => !CheckCanJump();

    #endregion

    private Rigidbody2D rigidbody;

    private bool canJump;
    private int _currentJumpCount = 0;
    private bool canDash = true;
    private bool isDashing;
    private float coolDown = 1;

    private EmotionSadness _emotionSadness;
    private short _lookingDirection = 1;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        _emotionSadness = GetComponent<EmotionSadness>();
    }

    void Start()
    {
        _currentJumpCount = 0;
    }

    void Update()
    {
        PlayerMovement();
        CheckIfOnGround();
        Jumper();
        PlayerDash(); 
    }

    private void PlayerMovement()
    {
        if (isDashing) return;
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        if (horizontalMovement < 0) _lookingDirection = -1;
        else if (horizontalMovement > 0) _lookingDirection = 1;

        _VFX.transform.localScale = new Vector3(_lookingDirection, 1);

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
        if (colliders.Length > 0 && rigidbody.velocity.y <= 0)
            _currentJumpCount = 0;
        
        return (colliders.Length > 0);
    }

    private bool CheckCanJump()
    {
        canJump = !isDashing && (_currentJumpCount < (_maxJumpCount));
        return canJump;
    }

    private void Jumper()
    {
        if (Input.GetKeyDown(jumpKeyCode))
        {
            if (CheckCanJump())
            {
                Jump();
            }
        }

    }
    private void Jump()
    {
        StopVerticalVelocity();
        _currentJumpCount++;
        rigidbody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
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

    public void SetMaxVelocity(Vector2 vel)
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

        SetMaxVelocity(new Vector2(_dashPower, 20));

        //float XmovementDash = rigidbody.velocity.x;
        float XmovementDash = _dashPower * _lookingDirection;
        rigidbody.velocity = new Vector2(XmovementDash, 0f);
        _trailRenderer.emitting = true;

        yield return new WaitForSeconds(_dashDuration);

        SetMaxVelocity(new Vector2(_maxHorizontalVelocity, _maxVerticalVelocity));


        rigidbody.gravityScale = originalGravity;
        _trailRenderer.emitting = false;
        isDashing = false;

        yield return new WaitForSeconds(coolDown);
        canDash = true; 
    }

}
