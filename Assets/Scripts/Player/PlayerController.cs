using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerPulse))]
[RequireComponent(typeof(PlayerHover))]
public class PlayerController : MonoBehaviour
{
    private PlayerStateMachine _playerStateMachine;

    [Header("Movement")]

    [SerializeField] private Transform _orientation;

    [Tooltip("Normal walking speed.")]
    [SerializeField] private float _normalSpeed = 4f;

    [Tooltip("Running/sprinting speed.")]
    [SerializeField] private float _sprintSpeed = 7f;

    [Tooltip("Amount removed from normal and sprint speed when carrying a heavy object.")]
    [SerializeField] private float _carryHeavySpeedDifference = 1f;

    [Tooltip("How quickly the player reaches their target speed.")]
    [SerializeField] private float _movementAcceleration = 40f;

    [Tooltip("How quickly the player stops when releasing movement.")]
    [SerializeField] private float _groundDeceleration = 50f;

    [Tooltip("How quickly the player changes direction.")]
    [SerializeField] private float _turnResponsiveness = 25f;

    [Tooltip("Extra responsiveness when sharply reversing direction.")]
    [SerializeField] private float _sharpTurnMultiplier = 2f;

    [Tooltip("Movement control while airborne.")]
    [Range(0f, 1f)]
    [SerializeField] private float _airMultiplier = 0.85f;

    [Range(0f, 1f)]
    [SerializeField] private float _movementDeadzone = 0.05f;

    [SerializeField] private float _groundDrag = 0f;

    [Tooltip("Variable used by the animator to determine whether the player is carrying.")]
    public bool _isCarrying = false;

    private bool _isCarryingHeavy = false;

    private float _speed;

    [Header("Sprinting")]

    // Kept for compatibility with existing scripts.
    [SerializeField] private float _maxSprintAccelerationTime = 1f;

    [Tooltip("How long the player takes to brake from sprint speed toward walking speed.")]
    [SerializeField] private float _sprintReleaseBrakingTime = 0.1f;

    private float _currentSprintTime = 0f;
    private bool _isSprinting = false;

    private bool _wasSprinting = false;
    private float _sprintBrakeTimer = 0f;

    [Header("Jumping")]

    [Tooltip("Initial force of the short jump.")]
    [SerializeField] private float _shortJumpForce = 10f;

    [Tooltip("Additional force added when the jump button is held long enough.")]
    [SerializeField] private float _fullJumpForce = 5f;

    [Tooltip("How long jump must be held to receive the full jump.")]
    [SerializeField] private float _fullJumpHoldTime = 0.15f;

    [Tooltip("Gravity multiplier while rising.")]
    [SerializeField] private float _jumpGravityMultiplier = 1f;

    [Tooltip("Gravity multiplier near the apex of the jump.")]
    [Range(0.1f, 1f)]
    [SerializeField] private float _apexGravityMultiplier = 0.7f;

    [Tooltip("Gravity multiplier while falling.")]
    [SerializeField] private float _fallGravityMultiplier = 1.35f;

    [Tooltip("Vertical velocity range considered to be the jump apex.")]
    [SerializeField] private float _apexThreshold = 1.5f;

    [SerializeField] private float _jumpCooldown = 0.1f;

    private float _jumpHoldTimer = 0f;

    private bool _jumpHolding = false;
    private bool _fullJumpApplied = false;

    [Header("Coyote Time")]

    [SerializeField] private float _coyoteTime = 0.2f;

    private float _lastGroundedTime;

    private bool _letJumpGo;
    private bool _readyToJump = true;

    [Header("Ground Check")]

    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundMask;

    private bool _grounded;

    [Header("Hover VFX")]

    [Tooltip("The particle system used for the player's hover VFX.")]
    [SerializeField] private ParticleSystem _particleSystem;

    [Header("Movement VFX")]

    [Tooltip("Smoke particle effect played at the player's feet while sprinting.")]
    [SerializeField] private ParticleSystem _sprintSmoke;

    [Header("Animation")]

    [SerializeField] private Animator _animator;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 _direction;

    private Rigidbody _rb;

    private PlayerPulse _playerPulse;
    private PlayerHover _playerHover;

    private bool _dialogueIsPlaying = false;


    // =========================================================
    // PUBLIC PROPERTIES
    // =========================================================

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

    // Kept for compatibility with existing scripts.
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

    public ParticleSystem ParticleSystem => _particleSystem;


    // =========================================================
    // UNITY
    // =========================================================

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

        _speed = GetNormalSpeed();

        // Make sure sprint smoke starts off.
        if (_sprintSmoke != null)
        {
            _sprintSmoke.Stop();
        }
    }

    private void Update()
    {
        _playerStateMachine.Execute();

        GroundCheck();

        PlayerInput();

        HandleJumpHold();

        Sprint();

        UpdateSprintSmoke();
    }

    private void FixedUpdate()
    {
        ApplyJumpGravity();

        MovePlayer();
    }


    // =========================================================
    // GROUND CHECK
    // =========================================================

    private void GroundCheck()
    {
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
            _lastGroundedTime = Time.time;
        }

        _grounded = isCurrentlyGrounded;

        _rb.drag =
            _grounded
                ? _groundDrag
                : 0f;
    }


    // =========================================================
    // INPUT
    // =========================================================

    private void PlayerInput()
    {
        horizontalInput =
            Input.GetAxisRaw("Horizontal");

        verticalInput =
            Input.GetAxisRaw("Vertical");

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


    // =========================================================
    // MOVEMENT
    // =========================================================

    private void MovePlayer()
    {
        _direction =
            _orientation.forward * verticalInput +
            _orientation.right * horizontalInput;

        // Prevent diagonal movement from being faster.
        if (_direction.magnitude > 1f)
        {
            _direction.Normalize();
        }

        // Ignore tiny inputs.
        if (_direction.magnitude < _movementDeadzone)
        {
            _direction = Vector3.zero;
        }

        // Keep movement on the player's gravity plane.
        Vector3 movementDirection =
            Vector3.ProjectOnPlane(
                _direction,
                transform.up
            );

        if (movementDirection.sqrMagnitude > 0.001f)
        {
            movementDirection.Normalize();
        }
        else
        {
            movementDirection = Vector3.zero;
        }

        // Current horizontal velocity.
        Vector3 currentHorizontalVelocity =
            Vector3.ProjectOnPlane(
                _rb.velocity,
                transform.up
            );


        // =====================================================
        // NO MOVEMENT INPUT
        // =====================================================

        if (movementDirection == Vector3.zero)
        {
            if (_grounded)
            {
                // Stop quickly and predictably.
                currentHorizontalVelocity =
                    Vector3.MoveTowards(
                        currentHorizontalVelocity,
                        Vector3.zero,
                        _groundDeceleration *
                        Time.fixedDeltaTime
                    );
            }

            // In the air we intentionally keep
            // horizontal velocity.
        }


        // =====================================================
        // MOVEMENT INPUT
        // =====================================================

        else
        {
            float targetSpeed = _speed;


            // -------------------------------------------------
            // Sprint release braking
            // -------------------------------------------------

            if (!_isSprinting &&
                _sprintBrakeTimer <
                _sprintReleaseBrakingTime &&
                _wasSprinting &&
                _grounded)
            {
                _sprintBrakeTimer +=
                    Time.fixedDeltaTime;

                float brakeT =
                    Mathf.Clamp01(
                        _sprintBrakeTimer /
                        _sprintReleaseBrakingTime
                    );

                targetSpeed =
                    Mathf.Lerp(
                        GetSprintSpeed(),
                        GetNormalSpeed(),
                        brakeT
                    );
            }


            // -------------------------------------------------
            // Air movement
            // -------------------------------------------------

            if (!_grounded)
            {
                targetSpeed *= _airMultiplier;
            }

            Vector3 targetVelocity =
                movementDirection *
                targetSpeed;


            // -------------------------------------------------
            // Turn responsiveness
            // -------------------------------------------------

            float responsiveness =
                _turnResponsiveness;

            if (currentHorizontalVelocity.sqrMagnitude >
                0.01f)
            {
                float directionDot =
                    Vector3.Dot(
                        currentHorizontalVelocity.normalized,
                        movementDirection
                    );

                // We're trying to reverse direction.
                if (directionDot < 0f)
                {
                    responsiveness *=
                        _sharpTurnMultiplier;
                }
            }


            // Smoothly turn toward desired direction.
            Vector3 newVelocity;

            if (currentHorizontalVelocity.sqrMagnitude >
                0.01f)
            {
                Vector3 newDirection =
                    Vector3.RotateTowards(
                        currentHorizontalVelocity.normalized,
                        movementDirection,
                        responsiveness *
                        Time.fixedDeltaTime,
                        0f
                    );

                float currentSpeed =
                    currentHorizontalVelocity.magnitude;

                float acceleration =
                    _movementAcceleration;

                if (!_grounded)
                {
                    acceleration *=
                        _airMultiplier;
                }

                float newSpeed =
                    Mathf.MoveTowards(
                        currentSpeed,
                        targetSpeed,
                        acceleration *
                        Time.fixedDeltaTime
                    );

                newVelocity =
                    newDirection *
                    newSpeed;
            }
            else
            {
                newVelocity =
                    targetVelocity;
            }

            currentHorizontalVelocity =
                newVelocity;
        }


        // =====================================================
        // APPLY VELOCITY
        // =====================================================

        Vector3 verticalVelocity =
            Vector3.Project(
                _rb.velocity,
                transform.up
            );

        _rb.velocity =
            currentHorizontalVelocity +
            verticalVelocity;
    }


    // =========================================================
    // SPRINTING
    // =========================================================

    private void Sprint()
    {
        bool previousSprintState =
            _isSprinting;

        bool wantsToSprint =
            Input.GetAxisRaw("Sprint") > 0f;

        _isSprinting =
            wantsToSprint &&
            !_isCarryingHeavy;

        // Sprinting is instant.
        //
        // You are either walking or sprinting.

        if (_isSprinting)
        {
            _currentSprintTime =
                _maxSprintAccelerationTime;

            _speed =
                GetSprintSpeed();

            _sprintBrakeTimer = 0f;
        }
        else
        {
            _currentSprintTime = 0f;

            _speed =
                GetNormalSpeed();
        }

        // Detect the exact moment sprint is released.
        if (previousSprintState &&
            !_isSprinting)
        {
            _sprintBrakeTimer = 0f;
        }

        _wasSprinting =
            previousSprintState;
    }


    // =========================================================
    // SPRINT SMOKE VFX
    // =========================================================

    private void UpdateSprintSmoke()
    {
        // If no particle system has been assigned,
        // don't do anything.
        if (_sprintSmoke == null)
        {
            return;
        }

        // Smoke should ONLY happen when:
        //
        // 1. Player is sprinting
        // 2. Player is grounded
        // 3. Player is actually moving

        bool shouldPlaySmoke =
            _isSprinting &&
            _grounded &&
            _direction.sqrMagnitude > 0.01f;

        if (shouldPlaySmoke)
        {
            if (!_sprintSmoke.isPlaying)
            {
                _sprintSmoke.Play();
            }
        }
        else
        {
            if (_sprintSmoke.isPlaying)
            {
                _sprintSmoke.Stop();
            }
        }
    }


    // =========================================================
    // SPEED
    // =========================================================

    private float GetNormalSpeed()
    {
        if (_isCarryingHeavy)
        {
            return Mathf.Max(
                0f,
                _normalSpeed -
                _carryHeavySpeedDifference
            );
        }

        return _normalSpeed;
    }

    private float GetSprintSpeed()
    {
        if (_isCarryingHeavy)
        {
            return Mathf.Max(
                0f,
                _sprintSpeed -
                _carryHeavySpeedDifference
            );
        }

        return _sprintSpeed;
    }


    // =========================================================
    // JUMP
    // =========================================================

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

        _playerStateMachine.TransitionTo(
            _playerStateMachine.jumpState
        );
    }


    // =========================================================
    // VARIABLE JUMP
    // =========================================================

    private void HandleJumpHold()
    {
        if (!_jumpHolding)
        {
            return;
        }

        // Released before full jump.
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


    // =========================================================
    // JUMP GRAVITY
    // =========================================================

    private void ApplyJumpGravity()
    {
        if (_grounded)
        {
            return;
        }

        GravityBody gravityBody =
            GetComponent<GravityBody>();

        if (gravityBody == null)
        {
            return;
        }

        Vector3 gravityDirection =
            gravityBody.GravityDirection;

        if (gravityDirection == Vector3.zero)
        {
            return;
        }

        Vector3 verticalVelocity =
            Vector3.Project(
                _rb.velocity,
                transform.up
            );

        float verticalSpeed =
            Vector3.Dot(
                verticalVelocity,
                transform.up
            );

        float gravityMultiplier;

        // Rising.
        if (verticalSpeed > _apexThreshold)
        {
            gravityMultiplier =
                _jumpGravityMultiplier;
        }

        // Apex.
        else if (
            Mathf.Abs(verticalSpeed) <=
            _apexThreshold)
        {
            gravityMultiplier =
                _apexGravityMultiplier;
        }

        // Falling.
        else
        {
            gravityMultiplier =
                _fallGravityMultiplier;
        }

        // GravityBody supplies the main gravity.
        //
        // This modifies it to create the desired
        // jump arc.

        float baseGravity = 30f;

        float extraGravity =
            gravityMultiplier - 1f;

        _rb.AddForce(
            gravityDirection *
            baseGravity *
            extraGravity,
            ForceMode.Acceleration
        );
    }


    // =========================================================
    // CARRYING
    // =========================================================

    public void CarryObject(
        bool carrying,
        bool isHeavy)
    {
        _isCarrying =
            carrying;

        _isCarryingHeavy =
            isHeavy;

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


    // =========================================================
    // JUMP RESET
    // =========================================================

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


    // =========================================================
    // SPEED CONTROL
    // =========================================================

    private void SpeedControl()
    {
        // Kept for compatibility with other scripts.
        //
        // Movement is directly controlled
        // inside MovePlayer().
    }


    // =========================================================
    // GIZMOS
    // =========================================================

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