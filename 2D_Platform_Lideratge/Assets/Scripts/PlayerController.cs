using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource), typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] KeyCode jumpKeyCode = KeyCode.Space;
    [SerializeField] KeyCode dashKeyCode = KeyCode.LeftShift;
    [SerializeField] KeyCode _crouchKeyCode = KeyCode.LeftControl;


    [Header("Attributes")]
    [Range(0, 20)][SerializeField] float speedMovement;
    [Range(0, 20)][SerializeField] float jumpSpeed;
    [SerializeField] private int _maxJumpCount = 1;
    [Range(0, 20)][SerializeField] private float _maxVerticalVelocity = 10;
    [Range(0, 20)][SerializeField] private float _maxHorizontalVelocity = 10;
    [Range(10, 50)] [SerializeField] private float _dashPower = 30;
    [Range(0, 2)] [SerializeField] private float _dashDuration = 0.2f;
    [Range(0, 5)] [SerializeField] private float _timeToCrouchJump = 1f;
    [Range(1, 5)] [SerializeField] private float _crouchJumpMultiplier = 2f;
    Vector2 _startPosition;



    [Header("References")]
    [SerializeField] LayerMask whatIsGround; 
    [SerializeField] Transform groundChecker;
    [SerializeField] TrailRenderer _trailRenderer;
    [SerializeField] private GameObject _VFX;
    [SerializeField] private CapsuleCollider2D _standCollider;
    [SerializeField] private CapsuleCollider2D _crouchCollider;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _deathParticles;
    [SerializeField] private ParticleSystem _dustParticles;


    #region Getters

    public float Gravity => rigidbody.gravityScale;
    //public bool IsFalling => rigidbody.velocity.y < 0 && !CheckIfOnGround();
    public bool IsOnGround => CheckIfOnGround();
    public float MaxVerticalVelocity => _maxVerticalVelocity;
    public float MaxHorizontalVelocity => _maxVerticalVelocity;

    public bool CanOpenUmbrella = false;

    public Vector2 SpawnPosition { get { return spawnPosition; } set { spawnPosition = value; } }

    #endregion

    private Rigidbody2D rigidbody;

    private bool canJump;
    private int _currentJumpCount = 0;
    private bool canDash = true;
    private bool isDashing;
    private float coolDown = 1;
    private bool _isCrouching = false;
    private float _crouchingTime = 0;
    private bool _isCrouchJumpReady = false;

    private bool _isDead = false;

    private EmotionSadness _emotionSadness;
    private short _lookingDirection = 1;

    private Vector3 spawnPosition;

    private float originalGravityScale;

    private AudioSource[] _audioSources = new AudioSource[2];

    private void Awake()
    {
        if (GameManager.GetGameManager().GetPlayer() == null)
        {
            GameManager.GetGameManager().m_Player = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }

        rigidbody = GetComponent<Rigidbody2D>();
        _emotionSadness = GetComponent<EmotionSadness>();

        _audioSources = GetComponents<AudioSource>();
        _startPosition = transform.position;
    }

    void Start()
    {
        spawnPosition = transform.position;
        _currentJumpCount = 0;
        originalGravityScale = rigidbody.gravityScale;

        //SoundManager.PlayMusic(AudioNames.LVLMUSIC);
    }

    void Update()
    {
        // Debug.Log("Is dead = " + _isDead);
        if(_isDead)
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.gravityScale = 0;
            return;
        }

        _animator.SetBool("Falling", !CheckIfOnGround() && rigidbody.velocity.y < 0.1f);

        PlayerCrouch();
        Jumper();

        if (_isCrouching) return;
        PlayerMovement();
        PlayerDash(); 
    }

    private void PlayerMovement()
    {
        if (isDashing) return;
        
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        if (horizontalMovement < 0) _lookingDirection = -1;
        else if (horizontalMovement > 0) _lookingDirection = 1;
        
        _animator.SetBool("Walking", horizontalMovement != 0);
        _VFX.transform.localScale = new Vector3(_lookingDirection, 1);

        Vector2 direction = new Vector2(horizontalMovement * speedMovement, rigidbody.velocity.y);

        rigidbody.velocity = direction;

        //Clamp the velocity
        Vector2 clampedVelocity = GetClampedVelocities();
        rigidbody.velocity = new Vector2(clampedVelocity.x, clampedVelocity.y);
    }
    private bool CheckIfOnGround()
    {
        float detectionRadius = 0.05f;
        var colliders = Physics2D.OverlapCircleAll(groundChecker.position, detectionRadius, whatIsGround);
        if (colliders.Length > 0 && rigidbody.velocity.y <= 0)
            _currentJumpCount = 0;

        
        _animator.SetBool("IsGrounded", colliders.Length > 0);

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
                CanOpenUmbrella = false;
                Jump();
            }
            else
            {
                GetComponent<EmotionSadness>().OpenUmbrella();
            }

        }

    }
    private void Jump()
    {
        if (_isCrouching) _isCrouching = false;
        
        float jumpForce = _isCrouchJumpReady ? (jumpSpeed * _crouchJumpMultiplier) : jumpSpeed;
        _currentJumpCount++;
        StopVerticalVelocity();
        rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        _animator.SetTrigger("Jump");
        _dustParticles.Play();

        if(!_isCrouchJumpReady) 
            PlaySound("JUMP");
        else
            PlaySound("LONGJUMP");

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
            //Debug.Log("Dash");
            PlaySound("DASH");
            StartCoroutine(DoDash());

        }

    }

    private void PlayerCrouch()
    {

        if (!CheckIfOnGround())
        {
            _animator.SetBool("Crouching", false);
            _isCrouching = false;
            _crouchCollider.enabled = false;
            _standCollider.enabled = true;
            _crouchingTime = 0;
            _isCrouchJumpReady = false;

            _isCrouching = false;
            return;
        }


        if (Input.GetKey(_crouchKeyCode))
        {
            if(!_isCrouching)
            {
                _animator.SetBool("Crouching", true);
                _isCrouching = true;
                _crouchCollider.enabled = true;
                _standCollider.enabled = false;
                rigidbody.velocity = Vector2.zero;
            }

            _crouchingTime += Time.deltaTime;
            _isCrouchJumpReady = _crouchingTime >= _timeToCrouchJump;
        }
        else
        {
            if(_isCrouching)
            {
                _animator.SetBool("Crouching", false);
                _isCrouching = false;
                _crouchCollider.enabled = false;
                _standCollider.enabled = true;
                _crouchingTime = 0;
                _isCrouchJumpReady = false;
            }
        }

    }

    private IEnumerator DoDash()
    {
        canDash = false;
        isDashing = true;
        
        rigidbody.gravityScale = 0f;

        SetMaxVelocity(new Vector2(_dashPower, 20));

        //float XmovementDash = rigidbody.velocity.x;
        float XmovementDash = _dashPower * _lookingDirection;
        rigidbody.velocity = new Vector2(XmovementDash, 0f);
        _trailRenderer.emitting = true;

        yield return new WaitForSeconds(_dashDuration);

        SetMaxVelocity(new Vector2(_maxHorizontalVelocity, _maxVerticalVelocity));


        rigidbody.gravityScale = originalGravityScale;
        _trailRenderer.emitting = false;
        isDashing = false;

        yield return new WaitForSeconds(coolDown);
        canDash = true; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Deadzone") )
        {
            if(!_isDead) 
                StartCoroutine(CODeath());
        }
    }

    public IEnumerator CODeath()
    {
        _isDead = true;
        //Die anim
        _deathParticles.Play();
        _VFX.SetActive(false);
        SoundManager.PlaySFX(AudioNames.DIE, _audioSources[0]);
        //TODO

        yield return new WaitForSeconds(1);

        //Show telon
        UIManager.ShowTelon();
        yield return new WaitForSeconds(2);
        //Restar scene
        GameManager.GetGameManager().Restart();
        UIManager.HideTelon();
        yield return null;
    }

    public void Restart()
    {
        _isDead = false;
        _VFX.SetActive(true);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        transform.position = GameManager.GetGameManager().SpawnPosition;
        rigidbody.gravityScale = originalGravityScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundChecker.position, 0.05f);
    }

    public void PlayAgain()
    {
        transform.position = _startPosition; 
    }

    public void PlaySound(string soundName)
    {
        AudioSource au = _audioSources[0].isPlaying ? _audioSources[1] : _audioSources[0];
        SoundManager.PlaySFX(soundName, au);
    }
}
