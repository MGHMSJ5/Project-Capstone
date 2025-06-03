using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerPulse))]
[RequireComponent(typeof(PlayerHover))]
public class PlayerController : MonoBehaviour
{
    private PlayerStateMachine _playerStateMachine;
    [Header("Movement")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private float _normalSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField]
    [Tooltip("This is the amount that will be REMOVED from normal and sprint speed when carrying a heavy object")]
    private float _carryHeavySpeedDifference;
    [Tooltip("This is the variable that needs to be used in the animator to see if the player is carrying or not.")]
    private bool _isCarrying = false;
    private bool _isCarryingHeavy = false;
    private float _speed;
    [SerializeField] private float _groundDrag;

    [Header("Sprinting")]
    
    [SerializeField] private float _maxSprintAccelerationTime = 1f;
    private float _currentSprintTime = 0.0f;
    private bool _isSprinting = false;

    [Header("Jumping")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplier;
    public ParticleSystem particleSystem;
    [Header("Coyote Time")]
    [SerializeField] private float _coyoteTime = 0.2f; // This is for coyote time
    private float _lastGroundedTime;
    private bool _letJumpGo; // Bool used to check when player is holding the button
    private bool _readyToJump = true; // Bool used for when the player can jump

    [Header("Ground Check")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundMask;
    private bool _grounded;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private float _walkingAnimationSpeed = 1f;
    [SerializeField] private float _sprintAnimationSpeed = 2.5f;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 _direction;
    private Rigidbody _rb;

    private PlayerPulse _playerPulse;
    private PlayerHover _playerHover;

    // (Later) use this bool in the dialogue manager to set it to true when in dialgue, change this script so that player won't be able to move when it is true
    // Also make it so that the right animation will be played
    private bool _dialogueIsPlaying = false;

    // Exposing the following variables safely, for use in other scripts
    public PlayerStateMachine PlayerStateMachine => _playerStateMachine;
    public bool DialogueIsPlaying => _dialogueIsPlaying;
    public Transform Orientation => _orientation;
    public float Speed => _speed;
    public float GroundDrag => _groundDrag;
    public float NormalSpeed => _normalSpeed;
    public float SprintSpeed => _sprintSpeed;
    public bool IsSprinting => _isSprinting;
    public float MaxSprintAccelerationTime => _maxSprintAccelerationTime;
    public float CurrentSprintTime => _currentSprintTime;
    public float JumpForce => _jumpForce;
    public float JumpCooldown => _jumpCooldown;
    public float AirMultiplier => _airMultiplier;
    public bool ReadyToJump => _readyToJump;
    public float PlayerHeight => _playerHeight;
    public LayerMask GroundMask => _groundMask;
    public bool Grounded => _grounded;
    public float HorizontalInput => horizontalInput;
    public float VerticalInput => verticalInput;
    public Rigidbody RB => _rb;
    public Vector3 Direction => _direction;
    public PlayerPulse PlayerPulse => _playerPulse;
    public PlayerHover PlayerHover => _playerHover;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _playerPulse = GetComponent<PlayerPulse>();
        _playerHover = GetComponent<PlayerHover>();

        // Initialize the State Macine
        _playerStateMachine = new PlayerStateMachine(this);
    }
    void Start()
    {
        _playerStateMachine.Initialize(_playerStateMachine.idleState);
        _speed = _normalSpeed;
    }

    void Update()
    {
        // Update the current State
        _playerStateMachine.Execute();

        //ground check with coyote time
        bool isCurrentlyGrounded = Physics.Raycast(transform.position, -transform.up, _playerHeight * 0.5f + 0.2f, _groundMask);

    if (isCurrentlyGrounded)
    {
    _lastGroundedTime = Time.time;
    }

    _grounded = isCurrentlyGrounded;

        PlayerInput();
        SpeedControl();

        //handle drag
        if (_grounded)
        {
            _rb.drag = _groundDrag;
        }
        else
        {
            _rb.drag = 0;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();

        if (horizontalInput == 0 && verticalInput == 0)
        {
            _speed = _isCarryingHeavy ? _normalSpeed - _carryHeavySpeedDifference : _normalSpeed;
            _currentSprintTime = 0;
            _animator.speed = _walkingAnimationSpeed;
            _isSprinting = false;
        }
        else
        {
            Sprint();
        }
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Jump") && _readyToJump && (Time.time - _lastGroundedTime <= _coyoteTime) && !_isCarryingHeavy)
        {
            _readyToJump = false;
            _letJumpGo = false;
            Jump();
        }

        ResetJump();
    }

    private void MovePlayer()
    {
        //calculate movement direction
        _direction = _orientation.forward * verticalInput + _orientation.right * horizontalInput;
        if (_grounded)
        {
            _rb.MovePosition(_rb.position + _direction.normalized * _speed * Time.fixedDeltaTime);
            //_rb.velocity = _direction.normalized * _speed;
        }
        else if (!_grounded) //in the air
        {
            _rb.MovePosition(_rb.position + _direction.normalized * _sprintSpeed * _airMultiplier * Time.fixedDeltaTime);
        }
        // Elise: Could you write a comment that would explain how this works?
        // Thomas: Check the velocity of the player to change the Speed variable of the animator to change the animation
        _animator.SetFloat("Speed", _rb.velocity.magnitude);
    }

    private void Jump()
    {
        //reset velocity along the gravity direction
        _rb.velocity -= Vector3.Project(_rb.velocity, -transform.up);

        //use impulse, because the force is only applied once
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void Sprint()
    {
        if (Input.GetAxis("Sprint") == 1)
        {   //increase the sprint time, but don't go over the max acceleration time
            _currentSprintTime = Mathf.Min(_currentSprintTime + Time.deltaTime, _maxSprintAccelerationTime);
            _animator.speed = _sprintAnimationSpeed;
            _isSprinting = true;
        }
        else
        {   //decrease the sprint time, but don't go below 0
            _currentSprintTime = Mathf.Max(_currentSprintTime - Time.deltaTime, 0f);
            _animator.speed = _walkingAnimationSpeed;
            _isSprinting = false;
        }
        //normalize the time value between 0 and 1
        float t = _currentSprintTime / _maxSprintAccelerationTime;
        //smoothly transition the speed based on sprinting time
        _speed = Mathf.Lerp
            (
            _isCarryingHeavy ? _normalSpeed - _carryHeavySpeedDifference : _normalSpeed,
            _isCarryingHeavy ? _sprintSpeed - _carryHeavySpeedDifference : _sprintSpeed, 
            t
            );
    }

    public void CarryObject(bool carrying, bool isHeavy)
    {
        _isCarrying = carrying;
        _isCarryingHeavy = isHeavy;

    }

    private void ResetJump()
    {
        // If the player has jumped and let go of the jump button
        if (!_readyToJump && Input.GetButtonUp("Jump"))
        {
            _letJumpGo = true;
        }
        // If the player is in the air and let go of the button
        if (!_grounded && Input.GetButtonDown("Jump"))
        {
            _letJumpGo = false;
        }
        // If the player is on the ground and has let go of the jump button
        if (_grounded && _letJumpGo)
        {
            _readyToJump = true;
        }

        
    }

    private void SpeedControl()
    {
        Vector3 flatVel = _rb.velocity - Vector3.Project(_rb.velocity, -transform.up);

        //limit velocity if needed
        if (flatVel.magnitude > _speed)
        {
            Vector3 limitedVel = flatVel.normalized * _speed;
            _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
        }
    }
}
