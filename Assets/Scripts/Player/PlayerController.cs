using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerPulse))]
[RequireComponent(typeof(PlayerHover))]
public class PlayerController : MonoBehaviour
{
    private PlayerStateMachine _playerStateMachine;

    [Header("Movement")]

    [SerializeField] private Transform _orientation;

    [Tooltip("Player walking speed.")]
    [SerializeField] private float _normalSpeed = 4f;

    [Tooltip("Player running/sprinting speed.")]
    [SerializeField] private float _sprintSpeed = 7f;

    [Tooltip("Amount removed from walking/running speed while carrying a heavy object.")]
    [SerializeField] private float _carryHeavySpeedDifference = 1f;

    [Tooltip("Amount of movement control while airborne. 0 = none, 1 = full.")]
    [Range(0f, 1f)]
    [SerializeField] private float _airMultiplier = 0.5f;

    [Tooltip("Ignore very small movement inputs.")]
    [Range(0f, 1f)]
    [SerializeField] private float _movementDeadzone = 0.05f;

    [Tooltip("Rigidbody drag while grounded.")]
    [SerializeField] private float _groundDrag = 0f;

    [Tooltip("Variable used by the animator to determine whether the player is carrying.")]
    public bool _isCarrying = false;

    private bool _isCarryingHeavy = false;

    private float _speed;

    [Header("Sprinting")]

    [Tooltip("Kept for compatibility with existing scripts.")]
    [SerializeField] private float _maxSprintAccelerationTime = 1f;

    private float _currentSprintTime = 0f;
    private bool _isSprinting = false;

    [Header("Jumping")]

    [Tooltip("Initial vertical velocity. This is the SHORT jump.")]
    [SerializeField] private float _shortJumpForce = 10f;

    [Tooltip("Additional vertical velocity given when the player holds jump long enough.")]
    [SerializeField] private float _fullJumpForce = 5f;

    [Tooltip("How long Jump must be held before the player gets the FULL jump.")]
    [SerializeField] private float _fullJumpHoldTime = 0.15f;

    [Tooltip("Minimum time between jumps.")]
    [SerializeField] private float _jumpCooldown = 0.1f;

    private float _jumpHoldTimer = 0f;

    private bool _jumpHolding = false;
    private bool _fullJumpApplied = false;

    [Header("Coyote Time")]

    [Tooltip("How long after leaving a ledge the player can still jump.")]
    [SerializeField] private float _coyoteTime = 0.2f;

    private float _lastGroundedTime;

    private bool _letJumpGo;
    private bool _readyToJump = true;

    [Header("Ground Check")]

    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundMask;

    private bool _grounded;

    [Header("Effects")]

    [Tooltip("The particle system used for the player's hover VFX.")]
    [SerializeField] private ParticleSystem _particleSystem;

    [Header("Animation")]

    [SerializeField] private Animator _animator;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 _direction;

    private Rigidbody _rb;

    private PlayerPulse _playerPulse;
    private PlayerHover _playerHover;

    private bool _dialogueIsPlaying = false;

    // Public properties
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

    // Kept for compatibility with other scripts.
    public float JumpForce => _shortJumpForce;

    public float JumpCooldown => _jumpCooldown;
    public float AirMultiplier => _airMultiplier;

    public bool ReadyToJump => _readyToJump;
    public bool LetJumpGo => _letJumpGo;

    public float PlayerHeight => _playerHeight;
    public LayerMask GroundMask => _groundMask;

    public bool Grounded => _grounded;

    public float HorizontalInput => horizontalInput;
    public float VerticalInput => verticalInput;

    public Rigidbody RB => _rb;
    public Vector3 Direction => _direction;

    public PlayerPulse PlayerPulse => _playerPulse;
    public PlayerHover PlayerHover => _playerHover;

    // Hover VFX reference.
    public ParticleSystem ParticleSystem => _particleSystem;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _playerPulse = GetComponent<PlayerPulse>();
        _playerHover = GetComponent<PlayerHover>();

        _playerStateMachine =
            new PlayerStateMachine(this);

        _animator =
            GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _playerStateMachine.Initialize(
            _playerStateMachine.idleState
        );

        _speed = _normalSpeed;
    }

    private void Update()
    {
        _playerStateMachine.Execute();

        // Ground check.
        bool isCurrentlyGrounded =
            Physics.CheckBox(
                transform.position +
                -transform.up *
                (_playerHeight * 0.5f),

                new Vector3(
                    0.4f,
                    0.25f,
                    0.3f
                ),

                transform.rotation,
                _groundMask
            );

        if (isCurrentlyGrounded)
        {
            _lastGroundedTime =
                Time.time;
        }

        _grounded =
            isCurrentlyGrounded;

        PlayerInput();

        HandleJumpHold();

        Sprint();

        // Handle drag.
        if (_grounded)
        {
            _rb.drag =
                _groundDrag;
        }
        else
        {
            _rb.drag = 0f;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void PlayerInput()
    {
        horizontalInput =
            Input.GetAxisRaw("Horizontal");

        verticalInput =
            Input.GetAxisRaw("Vertical");

        // Only jump when Space is initially pressed.
        if (Input.GetButtonDown("Jump") &&
            _readyToJump &&
            (Time.time -
             _lastGroundedTime <=
             _coyoteTime) &&
            !_isCarryingHeavy)
        {
            Jump();
        }

        ResetJump();
    }

    private void MovePlayer()
    {
        _direction =
            _orientation.forward *
            verticalInput
            +
            _orientation.right *
            horizontalInput;

        // Prevent diagonal movement from being faster.
        if (_direction.magnitude > 1f)
        {
            _direction.Normalize();
        }

        // Remove tiny inputs.
        if (_direction.magnitude <
            _movementDeadzone)
        {
            _direction =
                Vector3.zero;
        }

        // Keep movement on the gravity plane.
        Vector3 movementDirection =
            Vector3.ProjectOnPlane(
                _direction,
                transform.up
            );

        if (movementDirection.sqrMagnitude >
            0.001f)
        {
            movementDirection.Normalize();
        }
        else
        {
            movementDirection =
                Vector3.zero;
        }

        // Either walking speed or running speed.
        Vector3 targetVelocity =
            movementDirection *
            _speed;

        // Reduced control while airborne.
        if (!_grounded)
        {
            targetVelocity *=
                _airMultiplier;
        }

        // Preserve vertical velocity.
        Vector3 verticalVelocity =
            Vector3.Project(
                _rb.velocity,
                transform.up
            );

        // Directly control horizontal velocity.
        _rb.velocity =
            targetVelocity +
            verticalVelocity;
    }

    private void Jump()
    {
        _readyToJump = false;
        _letJumpGo = false;

        _jumpHolding = true;
        _fullJumpApplied = false;
        _jumpHoldTimer = 0f;

        // Remove existing vertical velocity.
        Vector3 verticalVelocity =
            Vector3.Project(
                _rb.velocity,
                transform.up
            );

        _rb.velocity -=
            verticalVelocity;

        // Initial short jump.
        _rb.AddForce(
            transform.up *
            _shortJumpForce,
            ForceMode.VelocityChange
        );

        // IMPORTANT:
        // Do NOT play the particle system here.
        //
        // The particle system is the hover VFX.

        _playerStateMachine.TransitionTo(
            _playerStateMachine.jumpState
        );
    }

    private void HandleJumpHold()
    {
        if (!_jumpHolding)
        {
            return;
        }

        // Released before the full jump threshold.
        if (!Input.GetButton("Jump"))
        {
            _jumpHolding = false;
            _letJumpGo = true;

            return;
        }

        _jumpHoldTimer +=
            Time.deltaTime;

        // Held long enough for full jump.
        if (_jumpHoldTimer >=
            _fullJumpHoldTime &&
            !_fullJumpApplied)
        {
            _fullJumpApplied = true;
            _jumpHolding = false;

            _rb.AddForce(
                transform.up *
                _fullJumpForce,
                ForceMode.VelocityChange
            );
        }
    }

    private void Sprint()
    {
        bool wantsToSprint =
            Input.GetAxisRaw("Sprint") >
            0f;

        // Sprint is instant.
        _isSprinting =
            wantsToSprint &&
            !_isCarryingHeavy;

        _currentSprintTime =
            _isSprinting
                ? _maxSprintAccelerationTime
                : 0f;

        float normalSpeed =
            _isCarryingHeavy
                ? Mathf.Max(
                    0f,
                    _normalSpeed -
                    _carryHeavySpeedDifference
                )
                : _normalSpeed;

        float sprintSpeed =
            _isCarryingHeavy
                ? Mathf.Max(
                    0f,
                    _sprintSpeed -
                    _carryHeavySpeedDifference
                )
                : _sprintSpeed;

        _speed =
            _isSprinting
                ? sprintSpeed
                : normalSpeed;
    }

    public void CarryObject(
        bool carrying,
        bool isHeavy
    )
    {
        _isCarrying = carrying;
        _isCarryingHeavy = isHeavy;

        if (_isCarrying)
        {
            _animator.SetLayerWeight(
                1,
                1f
            );
        }
        else
        {
            _animator.SetLayerWeight(
                1,
                0f
            );
        }
    }

    private void ResetJump()
    {
        if (!_readyToJump &&
            Input.GetButtonUp("Jump"))
        {
            _letJumpGo = true;
        }

        if (_grounded &&
            _letJumpGo)
        {
            StartCoroutine(
                ResetJumpCooldown()
            );

            _letJumpGo = false;
        }
    }

    private IEnumerator ResetJumpCooldown()
    {
        yield return new WaitForSeconds(
            _jumpCooldown
        );

        _readyToJump = true;
    }

    private void SpeedControl()
    {
        // Kept for compatibility.
        //
        // MovePlayer() already directly controls
        // horizontal velocity.
        Vector3 verticalVelocity =
            Vector3.Project(
                _rb.velocity,
                transform.up
            );

        Vector3 flatVelocity =
            _rb.velocity -
            verticalVelocity;

        if (flatVelocity.magnitude >
            _speed)
        {
            flatVelocity =
                flatVelocity.normalized *
                _speed;

            _rb.velocity =
                flatVelocity +
                verticalVelocity;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color =
            Color.green;

        Vector3 boxSize =
            new Vector3(
                0.4f,
                0.25f,
                0.3f
            );

        Vector3 boxCenter =
            transform.position +
            -transform.up *
            (_playerHeight * 0.5f);

        Quaternion boxRotation =
            transform.rotation;

        Matrix4x4 oldMatrix =
            Gizmos.matrix;

        Gizmos.matrix =
            Matrix4x4.TRS(
                boxCenter,
                boxRotation,
                Vector3.one
            );

        Gizmos.DrawWireCube(
            Vector3.zero,
            boxSize
        );

        Gizmos.matrix =
            oldMatrix;

        Gizmos.color =
            Color.blue;

        Vector3 origin =
            transform.position;

        Vector3 direction =
            -transform.up;

        float distance =
            _playerHeight * 0.5f +
            0.2f;

        Gizmos.DrawLine(
            origin,
            origin +
            direction * distance
        );
    }
}