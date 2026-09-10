using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerPulse))]
[RequireComponent(typeof(PlayerHover))]
[RequireComponent(typeof(PlayerVibration))]
public class PlayerController : MonoBehaviour
{
    private PlayerStateMachine _playerStateMachine;


    // =========================================================
    // MOVEMENT
    // =========================================================

    [Header("Movement")]

    [SerializeField] private Transform _orientation;

    [Tooltip("Normal walking speed.")]
    [SerializeField] private float _normalSpeed = 4f;

    [Tooltip("Full sprint speed after the player has finished gearing up.")]
    [SerializeField] private float _sprintSpeed = 7f;

    [Tooltip("Speed used while the player is gearing up for the sprint.")]
    [SerializeField] private float _sprintGearUpMovementSpeed = 2.5f;

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


    // =========================================================
    // SPRINTING
    // =========================================================

    [Header("Sprinting")]

    [Tooltip("How long the player must hold sprint before the full sprint is activated.")]
    [SerializeField] private float _maxSprintAccelerationTime = 0.35f;

    [Tooltip("How long the player takes to brake from sprint speed toward walking speed.")]
    [SerializeField] private float _sprintReleaseBrakingTime = 0.25f;

    [Tooltip("Animation speed immediately after sprint is released.")]
    [SerializeField] private float _sprintReleaseAnimationSpeed = 2.2f;

    [Tooltip("Require movement input before the sprint can begin gearing up.")]
    [SerializeField] private bool _requireMovementForSprint = true;

    [Tooltip("How long Sprint must be held while completely stationary before the next movement starts at full sprint without the gear-up animation.")]
    [SerializeField] private float _stationarySprintHoldTime = 1f;

    [Tooltip("Minimum horizontal speed required for the sprint-release animation to play.")]
    [SerializeField] private float _minimumSprintReleaseSpeed = 1.5f;

    private float _currentSprintTime = 0f;

    private float _stationarySprintHoldTimer = 0f;

    private bool _stationarySprintPreCharged = false;

    private bool _isSprinting = false;

    private bool _isGearingUp = false;

    private bool _isBrakingFromSprint = false;

    private bool _wasSprinting = false;

    private float _sprintBrakeTimer = 0f;


    // =========================================================
    // JUMPING
    // =========================================================

    [Header("Jumping")]

    [Tooltip("Initial force of the short jump.")]
    [SerializeField] private float _shortJumpForce = 10f;

    [Tooltip("Additional force added when the jump button is held long enough.")]
    [SerializeField] private float _fullJumpForce = 5f;

    [Tooltip("How long jump must be held to receive the full jump.")]
    [SerializeField] private float _fullJumpHoldTime = 0.15f;

    [Tooltip("Gravity multiplier while rising.")]
    [SerializeField] private float _jumpGravityMultiplier = 1f;

    [Tooltip("Gravity multiplier when the player releases jump while ascending.")]
    [SerializeField] private float _jumpReleaseGravityMultiplier = 2.5f;

    [Tooltip("Gravity multiplier near the apex of the jump.")]
    [Range(0.1f, 1f)]
    [SerializeField] private float _apexGravityMultiplier = 0.6f;

    [Tooltip("Gravity multiplier while falling.")]
    [SerializeField] private float _fallGravityMultiplier = 1.35f;

    [Tooltip("Vertical velocity range considered to be the jump apex.")]
    [SerializeField] private float _apexThreshold = 2f;

    [Tooltip("How long jump input is remembered before landing.")]
    [SerializeField] private float _jumpBufferTime = 0.1f;

    [SerializeField] private float _jumpCooldown = 0.1f;

    [Tooltip("Volume of the jump sound effect.")]
    [Range(0f, 1f)]
    [SerializeField] private float _jumpSfxVolume = 1f;

    private float _jumpHoldTimer = 0f;

    private float _lastJumpPressedTime = -Mathf.Infinity;

    private bool _jumpHolding = false;

    private bool _fullJumpApplied = false;


    // =========================================================
    // COYOTE TIME
    // =========================================================

    [Header("Coyote Time")]

    [SerializeField] private float _coyoteTime = 0.12f;

    private float _lastGroundedTime;

    private bool _letJumpGo;

    private bool _readyToJump = true;


    // =========================================================
    // GROUND CHECK
    // =========================================================

    [Header("Ground Check")]

    [SerializeField] private float _playerHeight;

    [SerializeField] private LayerMask _groundMask;

    private bool _grounded;


    // =========================================================
    // HOVER VFX
    // =========================================================

    [Header("Hover VFX")]

    [Tooltip("The particle system used for the player's hover VFX.")]
    [SerializeField] private ParticleSystem _particleSystem;


    // =========================================================
    // MOVEMENT VFX
    // =========================================================

    [Header("Movement VFX")]

    [Tooltip("Smoke particle effect used for ground, jump, and landing smoke.")]
    [SerializeField] private ParticleSystem _sprintSmoke;

    [Tooltip("Allow sprint smoke to continue into the ascending part of a jump.")]
    [SerializeField] private bool _continueSmokeIntoJump = true;

    [Tooltip("Stop creating new smoke once the player reaches the apex of the jump.")]
    [SerializeField] private bool _stopSmokeAtApex = true;

    [Tooltip("Vertical velocity threshold used to determine when the player is near the apex.")]
    [SerializeField] private float _smokeApexThreshold = 1.5f;


    // =========================================================
    // GROUND SMOKE - LEG POSITIONS
    // =========================================================

    [Header("Ground Smoke - Leg Positions")]

    [Tooltip("Local left leg smoke offset. X = left/right, Y = height, Z = backwards.")]
    [SerializeField]
    private Vector3 _leftSmokeOffset =
        new Vector3(-0.25f, 0f, -0.35f);

    [Tooltip("Local right leg smoke offset. X = left/right, Y = height, Z = backwards.")]
    [SerializeField]
    private Vector3 _rightSmokeOffset =
        new Vector3(0.25f, 0f, -0.35f);

    [Tooltip("Number of smoke puffs emitted per second.")]
    [SerializeField] private float _groundSmokeEmissionRate = 10f;

    [Tooltip("How far above the actual ground surface the smoke should be spawned.")]
    [SerializeField] private float _groundSmokeSurfaceOffset = 0.15f;

    [Tooltip("How far from the player to search for the ground surface.")]
    [SerializeField] private float _groundSmokeRaycastDistance = 2f;

    private float _groundSmokeEmissionTimer = 0f;

    private bool _emitFromLeftLeg = true;


    // =========================================================
    // LANDING SMOKE
    // =========================================================

    [Header("Landing Smoke")]

    [Tooltip("Enable the two smoke clouds that appear when landing.")]
    [SerializeField] private bool _enableLandingSmoke = true;

    [Tooltip("How far to the left/right the landing smoke spawns.")]
    [SerializeField] private float _landingSmokeSideDistance = 0.3f;

    [Tooltip("How far backwards the landing smoke spawns.")]
    [SerializeField] private float _landingSmokeBackwardDistance = 0.2f;

    [Tooltip("How far above the ground the landing smoke is spawned.")]
    [SerializeField] private float _landingSmokeSurfaceOffset = 0.15f;

    [Tooltip("Minimum downward speed required before landing smoke is spawned.")]
    [SerializeField] private float _minimumLandingSmokeImpactSpeed = 1f;


    // =========================================================
    // JUMP SMOKE - BODY
    // =========================================================

    [Header("Jump Smoke - Body")]

    [Tooltip("Offset of the single centered smoke trail while jumping. Z moves it behind the player.")]
    [SerializeField]
    private Vector3 _jumpSmokeOffset =
        new Vector3(0f, 0f, -0.35f);

    private float _jumpSmokeEmissionTimer = 0f;


    // =========================================================
    // ANIMATION
    // =========================================================

    [Header("Animation")]

    [Tooltip("Animator used for the player's movement animations.")]
    [SerializeField] private Animator _animator;

    [Tooltip("Enable automatic animation speed scaling based on actual player movement speed.")]
    [SerializeField] private bool _enableAnimationSpeedScaling = true;

    [Tooltip("Animation playback speed while walking.")]
    [SerializeField] private float _walkAnimationSpeed = 1f;

    [Tooltip("VERY fast animation playback speed while gearing up into a sprint.")]
    [SerializeField] private float _sprintGearUpAnimationSpeed = 3.5f;

    [Tooltip("Animation playback speed during the normal sustained sprint.")]
    [SerializeField] private float _sprintAnimationSpeed = 1.5f;

    [Tooltip("Minimum allowed animation playback speed.")]
    [SerializeField] private float _minimumAnimationSpeed = 0.8f;

    [Tooltip("Maximum allowed animation playback speed.")]
    [SerializeField] private float _maximumAnimationSpeed = 4f;

    [Tooltip("How quickly animation speed changes. Higher values respond faster.")]
    [SerializeField] private float _animationSpeedSmoothing = 10f;

    private float _currentAnimationSpeed = 1f;


    // =========================================================
    // INPUT / MOVEMENT
    // =========================================================

    private float horizontalInput;

    private float verticalInput;

    private Vector3 _direction;

    private Rigidbody _rb;

    private PlayerPulse _playerPulse;

    private PlayerHover _playerHover;

    private PlayerVibration _playerVibration;

    private bool _dialogueIsPlaying = false;

    private bool _wasGrounded;

    private float _lastAirborneDownwardSpeed;


    // =========================================================
    // PUBLIC PROPERTIES
    // =========================================================

    public PlayerStateMachine PlayerStateMachine =>
        _playerStateMachine;

    public bool DialogueIsPlaying =>
        _dialogueIsPlaying;

    public Transform Orientation =>
        _orientation;

    public float Speed =>
        _speed;

    public float GroundDrag =>
        _groundDrag;

    public float NormalSpeed =>
        _normalSpeed;

    public float SprintSpeed =>
        _sprintSpeed;

    public bool IsSprinting =>
        _isSprinting;

    public bool IsGearingUp =>
        _isGearingUp;

    public bool IsBrakingFromSprint =>
        _isBrakingFromSprint;

    public float MaxSprintAccelerationTime =>
        _maxSprintAccelerationTime;

    public float CurrentSprintTime =>
        _currentSprintTime;

    public float SprintProgress =>
        Mathf.Clamp01(
            _currentSprintTime /
            Mathf.Max(
                0.01f,
                _maxSprintAccelerationTime
            )
        );

    public float SprintBrakeProgress =>
        Mathf.Clamp01(
            _sprintBrakeTimer /
            Mathf.Max(
                0.01f,
                _sprintReleaseBrakingTime
            )
        );

    public float JumpForce =>
        _shortJumpForce;

    public float JumpCooldown =>
        _jumpCooldown;

    public float AirMultiplier =>
        _airMultiplier;

    public bool ReadyToJump =>
        _readyToJump;

    public bool LetJumpGo =>
        _letJumpGo;

    public float PlayerHeight =>
        _playerHeight;

    public LayerMask GroundMask =>
        _groundMask;

    public bool Grounded =>
        _grounded;

    public float HorizontalInput =>
        horizontalInput;

    public float VerticalInput =>
        verticalInput;

    public Rigidbody RB =>
        _rb;

    public Vector3 Direction =>
        _direction;

    public PlayerPulse PlayerPulse =>
        _playerPulse;

    public PlayerHover PlayerHover =>
        _playerHover;

    public ParticleSystem ParticleSystem =>
        _particleSystem;


    // =========================================================
    // UNITY
    // =========================================================

    private void Awake()
    {
        _rb =
            GetComponent<Rigidbody>();

        _playerPulse =
            GetComponent<PlayerPulse>();

        _playerHover =
            GetComponent<PlayerHover>();

        _playerVibration =
            GetComponent<PlayerVibration>();

        _playerStateMachine =
            new PlayerStateMachine(this);

        _animator =
            GetComponentInChildren<Animator>();

        _currentAnimationSpeed =
            _walkAnimationSpeed;
    }


    private void Start()
    {
        _playerStateMachine.Initialize(
            _playerStateMachine.idleState
        );

        _speed =
            GetNormalSpeed();

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

        UpdateMovementAnimation();
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


        if (!isCurrentlyGrounded)
        {
            float verticalSpeed =
                Vector3.Dot(
                    _rb.velocity,
                    transform.up
                );

            if (verticalSpeed < 0f)
            {
                _lastAirborneDownwardSpeed =
                    Mathf.Max(
                        _lastAirborneDownwardSpeed,
                        -verticalSpeed
                    );
            }
        }


        if (!_wasGrounded &&
            isCurrentlyGrounded)
        {
            if (_playerVibration != null &&
                _lastAirborneDownwardSpeed >=
                _playerVibration.MinimumLandingImpactSpeed)
            {
                _playerVibration.PlayLandingVibration();
            }


            if (_enableLandingSmoke &&
                _lastAirborneDownwardSpeed >=
                _minimumLandingSmokeImpactSpeed)
            {
                EmitLandingSmoke();
            }


            _lastAirborneDownwardSpeed =
                0f;
        }


        if (isCurrentlyGrounded)
        {
            _lastGroundedTime =
                Time.time;
        }

        _grounded =
            isCurrentlyGrounded;

        _wasGrounded =
            isCurrentlyGrounded;

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


        if (Input.GetButtonDown("Jump"))
        {
            _lastJumpPressedTime =
                Time.time;
        }


        bool jumpBuffered =
            Time.time -
            _lastJumpPressedTime <=
            _jumpBufferTime;

        bool coyoteJumpAvailable =
            Time.time -
            _lastGroundedTime <=
            _coyoteTime;

        if (jumpBuffered &&
            coyoteJumpAvailable &&
            _readyToJump &&
            !_isCarryingHeavy)
        {
            Jump();

            _lastJumpPressedTime =
                -Mathf.Infinity;
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

        if (_direction.magnitude > 1f)
        {
            _direction.Normalize();
        }

        if (_direction.magnitude <
            _movementDeadzone)
        {
            _direction =
                Vector3.zero;
        }

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

        Vector3 currentHorizontalVelocity =
            Vector3.ProjectOnPlane(
                _rb.velocity,
                transform.up
            );


        if (movementDirection == Vector3.zero)
        {
            if (_grounded)
            {
                currentHorizontalVelocity =
                    Vector3.MoveTowards(
                        currentHorizontalVelocity,
                        Vector3.zero,
                        _groundDeceleration *
                        Time.fixedDeltaTime
                    );
            }
        }
        else
        {
            float targetSpeed;


            if (_isSprinting)
            {
                if (_isGearingUp)
                {
                    targetSpeed =
                        GetSprintGearUpSpeed();
                }
                else
                {
                    targetSpeed =
                        GetSprintSpeed();
                }
            }
            else
            {
                targetSpeed =
                    GetNormalSpeed();
            }


            if (_isBrakingFromSprint &&
                _grounded)
            {
                _sprintBrakeTimer =
                    Mathf.MoveTowards(
                        _sprintBrakeTimer,
                        _sprintReleaseBrakingTime,
                        Time.fixedDeltaTime
                    );

                float brakeT =
                    Mathf.Clamp01(
                        _sprintBrakeTimer /
                        Mathf.Max(
                            0.01f,
                            _sprintReleaseBrakingTime
                        )
                    );

                targetSpeed =
                    Mathf.Lerp(
                        GetSprintSpeed(),
                        GetNormalSpeed(),
                        brakeT
                    );

                if (brakeT >= 1f)
                {
                    _isBrakingFromSprint =
                        false;
                }
            }


            Vector3 targetVelocity =
                movementDirection *
                targetSpeed;


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

                if (directionDot < 0f)
                {
                    responsiveness *=
                        _sharpTurnMultiplier;
                }
            }


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
            Input.GetAxisRaw("Sprint") >
            0f;

        // Check the raw input directly.
        // This allows the stationary pre-charge to complete
        // before movement is processed by FixedUpdate.
        bool hasMovementInput =
            Mathf.Abs(horizontalInput) >
            _movementDeadzone ||
            Mathf.Abs(verticalInput) >
            _movementDeadzone;


        // =====================================================
        // STATIONARY SPRINT PRE-CHARGE
        // =====================================================

        if (wantsToSprint &&
            !hasMovementInput &&
            !_isCarryingHeavy)
        {
            _stationarySprintHoldTimer =
                Mathf.MoveTowards(
                    _stationarySprintHoldTimer,
                    Mathf.Max(
                        0f,
                        _stationarySprintHoldTime
                    ),
                    Time.deltaTime
                );

            if (_stationarySprintHoldTimer >=
                _stationarySprintHoldTime)
            {
                _stationarySprintPreCharged =
                    true;
            }
        }
        else if (!wantsToSprint)
        {
            _stationarySprintHoldTimer =
                0f;

            _stationarySprintPreCharged =
                false;
        }


        // =====================================================
        // DETERMINE WHETHER SPRINT CAN START
        // =====================================================

        bool wantsToStartSprint =
            wantsToSprint &&
            !_isCarryingHeavy &&
            (!_requireMovementForSprint ||
             hasMovementInput);


        // =====================================================
        // START / CONTINUE SPRINT
        // =====================================================

        if (wantsToStartSprint)
        {
            _isSprinting =
                true;

            _isBrakingFromSprint =
                false;

            _sprintBrakeTimer =
                0f;


            // =================================================
            // PRE-CHARGED STATIONARY SPRINT
            // =================================================

            if (_stationarySprintPreCharged)
            {
                // Skip the gear-up phase completely.
                _currentSprintTime =
                    _maxSprintAccelerationTime;

                _isGearingUp =
                    false;
            }


            // =================================================
            // NORMAL SPRINT
            // =================================================

            else
            {
                _currentSprintTime =
                    Mathf.MoveTowards(
                        _currentSprintTime,
                        _maxSprintAccelerationTime,
                        Time.deltaTime
                    );

                _isGearingUp =
                    _currentSprintTime <
                    _maxSprintAccelerationTime;
            }


            // Pre-charge is consumed once movement starts.
            _stationarySprintPreCharged =
                false;

            _stationarySprintHoldTimer =
                0f;
        }


        // =====================================================
        // SPRINT RELEASED
        // =====================================================

        else
        {
            _isSprinting =
                false;

            _isGearingUp =
                false;

            _currentSprintTime =
                0f;
        }


        // =====================================================
        // DETECT SPRINT RELEASE
        // =====================================================

        bool sprintWasJustReleased =
            previousSprintState &&
            !_isSprinting;


        if (sprintWasJustReleased)
        {
            Vector3 horizontalVelocity =
                Vector3.ProjectOnPlane(
                    _rb.velocity,
                    transform.up
                );

            float horizontalSpeed =
                horizontalVelocity.magnitude;

            if (_grounded &&
                horizontalSpeed >=
                _minimumSprintReleaseSpeed)
            {
                _isBrakingFromSprint =
                    true;

                _sprintBrakeTimer =
                    0f;
            }
            else
            {
                _isBrakingFromSprint =
                    false;

                _sprintBrakeTimer =
                    0f;
            }
        }


        // =====================================================
        // CANCEL BRAKING IF SPRINT STARTS AGAIN
        // =====================================================

        if (_isSprinting)
        {
            _isBrakingFromSprint =
                false;

            _sprintBrakeTimer =
                0f;
        }


        // =====================================================
        // SPEED VALUE
        // =====================================================

        if (_isSprinting)
        {
            if (_isGearingUp)
            {
                _speed =
                    GetSprintGearUpSpeed();
            }
            else
            {
                _speed =
                    GetSprintSpeed();
            }
        }
        else
        {
            _speed =
                GetNormalSpeed();
        }


        _wasSprinting =
            previousSprintState;
    }


    // =========================================================
    // ANIMATION
    // =========================================================

    private void UpdateMovementAnimation()
    {
        if (_animator == null)
        {
            return;
        }

        if (!_enableAnimationSpeedScaling)
        {
            _animator.speed =
                1f;

            return;
        }


        float horizontalSpeed =
            Vector3.ProjectOnPlane(
                _rb.velocity,
                transform.up
            ).magnitude;


        float targetAnimationSpeed;


        if (horizontalSpeed < 0.1f)
        {
            targetAnimationSpeed =
                _walkAnimationSpeed;
        }
        else if (_isBrakingFromSprint)
        {
            float brakeT =
                Mathf.Clamp01(
                    _sprintBrakeTimer /
                    Mathf.Max(
                        0.01f,
                        _sprintReleaseBrakingTime
                    )
                );

            targetAnimationSpeed =
                Mathf.Lerp(
                    _sprintReleaseAnimationSpeed,
                    _walkAnimationSpeed,
                    brakeT
                );
        }
        else if (_isGearingUp)
        {
            targetAnimationSpeed =
                _sprintGearUpAnimationSpeed;
        }
        else if (_isSprinting)
        {
            targetAnimationSpeed =
                _sprintAnimationSpeed;
        }
        else
        {
            targetAnimationSpeed =
                _walkAnimationSpeed;
        }


        targetAnimationSpeed =
            Mathf.Clamp(
                targetAnimationSpeed,
                _minimumAnimationSpeed,
                _maximumAnimationSpeed
            );


        _currentAnimationSpeed =
            Mathf.Lerp(
                _currentAnimationSpeed,
                targetAnimationSpeed,
                _animationSpeedSmoothing *
                Time.deltaTime
            );

        _animator.speed =
            _currentAnimationSpeed;
    }


    // =========================================================
    // SPRINT SMOKE VFX
    // =========================================================

    private void UpdateSprintSmoke()
    {
        if (_sprintSmoke == null)
        {
            return;
        }

        bool isRunning =
            _isSprinting &&
            !_isGearingUp &&
            _direction.sqrMagnitude >
            0.01f;


        if (_grounded)
        {
            _jumpSmokeEmissionTimer =
                0f;

            if (isRunning)
            {
                _groundSmokeEmissionTimer +=
                    Time.deltaTime;

                float emissionInterval =
                    1f /
                    Mathf.Max(
                        0.01f,
                        _groundSmokeEmissionRate
                    );

                while (_groundSmokeEmissionTimer >=
                       emissionInterval)
                {
                    _groundSmokeEmissionTimer -=
                        emissionInterval;

                    EmitGroundSmoke();
                }

                if (!_sprintSmoke.isPlaying)
                {
                    _sprintSmoke.Play();
                }
            }
            else
            {
                _groundSmokeEmissionTimer =
                    0f;

                if (_sprintSmoke.isPlaying)
                {
                    _sprintSmoke.Stop();
                }
            }

            return;
        }


        if (!_continueSmokeIntoJump ||
            !isRunning)
        {
            _jumpSmokeEmissionTimer =
                0f;

            if (_sprintSmoke.isPlaying)
            {
                _sprintSmoke.Stop();
            }

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

        bool stillAscending =
            verticalSpeed > 0f;

        bool shouldEmitSmoke;

        if (_stopSmokeAtApex)
        {
            shouldEmitSmoke =
                stillAscending &&
                verticalSpeed >
                _smokeApexThreshold;
        }
        else
        {
            shouldEmitSmoke =
                stillAscending;
        }


        if (shouldEmitSmoke)
        {
            _jumpSmokeEmissionTimer +=
                Time.deltaTime;

            float emissionInterval =
                1f /
                Mathf.Max(
                    0.01f,
                    _groundSmokeEmissionRate
                );

            while (_jumpSmokeEmissionTimer >=
                   emissionInterval)
            {
                _jumpSmokeEmissionTimer -=
                    emissionInterval;

                EmitJumpSmoke();
            }

            if (!_sprintSmoke.isPlaying)
            {
                _sprintSmoke.Play();
            }
        }
        else
        {
            _jumpSmokeEmissionTimer =
                0f;

            if (_sprintSmoke.isPlaying)
            {
                _sprintSmoke.Stop();
            }
        }
    }


    // =========================================================
    // GROUND SMOKE
    // =========================================================

    private void EmitGroundSmoke()
    {
        Vector3 movementDirection =
            Vector3.ProjectOnPlane(
                _rb.velocity,
                transform.up
            );

        if (movementDirection.sqrMagnitude <
            0.01f)
        {
            return;
        }

        movementDirection.Normalize();

        Vector3 upDirection =
            transform.up;

        Vector3 rightDirection =
            Vector3.Cross(
                upDirection,
                movementDirection
            ).normalized;

        Vector3 backwardsDirection =
            -movementDirection;


        bool usingLeftLeg =
            _emitFromLeftLeg;

        float side =
            usingLeftLeg
                ? -1f
                : 1f;

        float sideDistance =
            usingLeftLeg
                ? Mathf.Abs(
                    _leftSmokeOffset.x
                )
                : Mathf.Abs(
                    _rightSmokeOffset.x
                );

        Vector3 sideOffset =
            rightDirection *
            side *
            sideDistance;

        float backwardDistance =
            usingLeftLeg
                ? Mathf.Abs(
                    _leftSmokeOffset.z
                )
                : Mathf.Abs(
                    _rightSmokeOffset.z
                );

        Vector3 backwardOffset =
            backwardsDirection *
            backwardDistance;

        float verticalOffsetValue =
            usingLeftLeg
                ? _leftSmokeOffset.y
                : _rightSmokeOffset.y;

        Vector3 verticalOffset =
            upDirection *
            verticalOffsetValue;

        Vector3 approximatePosition =
            transform.position +
            sideOffset +
            backwardOffset +
            verticalOffset;

        Vector3 rayOrigin =
            approximatePosition +
            upDirection *
            0.5f;

        RaycastHit hit;

        bool foundGround =
            Physics.Raycast(
                rayOrigin,
                -upDirection,
                out hit,
                _groundSmokeRaycastDistance,
                _groundMask,
                QueryTriggerInteraction.Ignore
            );

        Vector3 worldPosition;

        if (foundGround)
        {
            worldPosition =
                hit.point +
                hit.normal *
                _groundSmokeSurfaceOffset;
        }
        else
        {
            worldPosition =
                approximatePosition +
                upDirection *
                _groundSmokeSurfaceOffset;
        }

        ParticleSystem.EmitParams emitParams =
            new ParticleSystem.EmitParams();

        emitParams.position =
            worldPosition;

        _sprintSmoke.Emit(
            emitParams,
            1
        );

        _emitFromLeftLeg =
            !_emitFromLeftLeg;
    }


    // =========================================================
    // LANDING SMOKE
    // =========================================================

    private void EmitLandingSmoke()
    {
        if (_sprintSmoke == null)
        {
            return;
        }

        Vector3 upDirection =
            transform.up;

        Vector3 movementDirection =
            Vector3.ProjectOnPlane(
                _rb.velocity,
                upDirection
            );


        if (movementDirection.sqrMagnitude <
            0.01f)
        {
            movementDirection =
                Vector3.ProjectOnPlane(
                    transform.forward,
                    upDirection
                );
        }

        if (movementDirection.sqrMagnitude <
            0.01f)
        {
            return;
        }

        movementDirection.Normalize();


        Vector3 rightDirection =
            Vector3.Cross(
                upDirection,
                movementDirection
            ).normalized;

        Vector3 backwardsDirection =
            -movementDirection;


        Vector3 leftPosition =
            transform.position
            - rightDirection *
              _landingSmokeSideDistance
            + backwardsDirection *
              _landingSmokeBackwardDistance;


        Vector3 rightPosition =
            transform.position
            + rightDirection *
              _landingSmokeSideDistance
            + backwardsDirection *
              _landingSmokeBackwardDistance;


        leftPosition =
            GetLandingSmokeGroundPosition(
                leftPosition,
                upDirection
            );

        rightPosition =
            GetLandingSmokeGroundPosition(
                rightPosition,
                upDirection
            );


        ParticleSystem.EmitParams leftEmitParams =
            new ParticleSystem.EmitParams();

        leftEmitParams.position =
            leftPosition;

        _sprintSmoke.Emit(
            leftEmitParams,
            1
        );


        ParticleSystem.EmitParams rightEmitParams =
            new ParticleSystem.EmitParams();

        rightEmitParams.position =
            rightPosition;

        _sprintSmoke.Emit(
            rightEmitParams,
            1
        );
    }


    // =========================================================
    // LANDING SMOKE GROUND POSITION
    // =========================================================

    private Vector3 GetLandingSmokeGroundPosition(
        Vector3 approximatePosition,
        Vector3 upDirection)
    {
        RaycastHit hit;

        Vector3 rayOrigin =
            approximatePosition +
            upDirection *
            0.5f;

        bool foundGround =
            Physics.Raycast(
                rayOrigin,
                -upDirection,
                out hit,
                _groundSmokeRaycastDistance,
                _groundMask,
                QueryTriggerInteraction.Ignore
            );

        if (foundGround)
        {
            return hit.point +
                   hit.normal *
                   _landingSmokeSurfaceOffset;
        }

        return approximatePosition +
               upDirection *
               _landingSmokeSurfaceOffset;
    }


    // =========================================================
    // JUMP SMOKE
    // =========================================================

    private void EmitJumpSmoke()
    {
        Vector3 movementDirection =
            Vector3.ProjectOnPlane(
                _rb.velocity,
                transform.up
            );

        if (movementDirection.sqrMagnitude <
            0.01f)
        {
            return;
        }

        movementDirection.Normalize();

        Vector3 upDirection =
            transform.up;

        Vector3 backwardsDirection =
            -movementDirection;

        Vector3 backwardOffset =
            backwardsDirection *
            Mathf.Abs(
                _jumpSmokeOffset.z
            );

        Vector3 verticalOffset =
            upDirection *
            _jumpSmokeOffset.y;

        Vector3 worldPosition =
            transform.position +
            backwardOffset +
            verticalOffset;

        ParticleSystem.EmitParams emitParams =
            new ParticleSystem.EmitParams();

        emitParams.position =
            worldPosition;

        _sprintSmoke.Emit(
            emitParams,
            1
        );
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


    private float GetSprintGearUpSpeed()
    {
        if (_isCarryingHeavy)
        {
            return Mathf.Max(
                0f,
                _sprintGearUpMovementSpeed -
                _carryHeavySpeedDifference
            );
        }

        return _sprintGearUpMovementSpeed;
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
        SoundManager.PlaySound(
            SoundType.JUMP,
            _jumpSfxVolume
        );

        _readyToJump =
            false;

        _letJumpGo =
            false;

        _jumpHolding =
            true;

        _fullJumpApplied =
            false;

        _jumpHoldTimer =
            0f;

        Vector3 verticalVelocity =
            Vector3.Project(
                _rb.velocity,
                transform.up
            );

        _rb.velocity -=
            verticalVelocity;

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

        if (!Input.GetButton("Jump"))
        {
            _jumpHolding =
                false;

            _letJumpGo =
                true;

            return;
        }

        _jumpHoldTimer +=
            Time.deltaTime;

        if (_jumpHoldTimer >=
            _fullJumpHoldTime &&
            !_fullJumpApplied)
        {
            _fullJumpApplied =
                true;

            _jumpHolding =
                false;

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

        if (gravityDirection ==
            Vector3.zero)
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

        if (!_jumpHolding &&
            verticalSpeed > 0f)
        {
            gravityMultiplier =
                _jumpReleaseGravityMultiplier;
        }
        else if (verticalSpeed >
                 _apexThreshold)
        {
            gravityMultiplier =
                _jumpGravityMultiplier;
        }
        else if (Mathf.Abs(verticalSpeed) <=
                 _apexThreshold)
        {
            gravityMultiplier =
                _apexGravityMultiplier;
        }
        else
        {
            gravityMultiplier =
                _fallGravityMultiplier;
        }

        float baseGravity =
            30f;

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
            _letJumpGo =
                true;
        }

        if (_grounded &&
            _letJumpGo)
        {
            StartCoroutine(
                ResetJumpCooldown()
            );

            _letJumpGo =
                false;
        }
    }


    private IEnumerator ResetJumpCooldown()
    {
        yield return new WaitForSeconds(
            _jumpCooldown
        );

        _readyToJump =
            true;
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
            direction *
            distance
        );
    }
}