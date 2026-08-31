using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerVibration))]
public class PlayerHover : MonoBehaviour
{
    [Header("Hovering")]

    [Tooltip("Whether the player has unlocked the hover ability.")]
    [SerializeField] public bool _hoverAbilityGranted = true;

    [Tooltip("Upward acceleration while hovering.")]
    [SerializeField] private float _hoverForce = 2f;

    [Tooltip("Maximum amount of time the player can hover.")]
    [SerializeField] private float _maxHoverTime = 3f;

    [Tooltip("Maximum horizontal speed while hovering.")]
    [SerializeField] private float _maxHoverSpeed = 5f;


    // =========================================================
    // HOVER STAMINA
    // =========================================================

    [Header("Hover Stamina")]

    [Tooltip("Whether hover stamina should refill when landing.")]
    [SerializeField] private bool _refillOnLanding = true;

    [Tooltip("How quickly hover stamina refills.")]
    [SerializeField] private float _hoverRefillSpeed = 3f;


    // =========================================================
    // STATE
    // =========================================================

    private float _currentHoverTime = 0f;

    private bool _isHovering = false;

    // Once empty, hover cannot be used again until landing.
    private bool _hoverDepleted = false;

    // Player must release Space before hover can activate.
    private bool _jumpReleased = false;


    // =========================================================
    // MOVEMENT
    // =========================================================

    [Header("Movement")]

    private PlayerController _playerController;
    private GravityBody _gravityBody;
    private PlayerVibration _playerVibration;


    // =========================================================
    // GROUND CHECK
    // =========================================================

    [Header("Ground Check")]

    private bool _grounded;


    // =========================================================
    // GRAVITY
    // =========================================================

    private Vector3 GravityDirection =>
        _gravityBody != null
            ? _gravityBody.GravityDirection
            : Vector3.down;


    // =========================================================
    // PUBLIC PROPERTIES
    // =========================================================

    public bool IsHovering =>
        _isHovering;

    public bool HoverDepleted =>
        _hoverDepleted;

    public bool IsGrounded =>
        _grounded;

    /// <summary>
    /// Whether the player is currently holding the hover/jump button.
    /// </summary>
    public bool HoverInputHeld =>
        Input.GetButton("Jump");

    /// <summary>
    /// Whether hover stamina is currently refilling.
    /// </summary>
    public bool IsRefillingHover =>
        _grounded &&
        _refillOnLanding &&
        _currentHoverTime < _maxHoverTime;

    /// <summary>
    /// Current hover stamina in seconds.
    /// </summary>
    public float CurrentHoverTime =>
        _currentHoverTime;

    /// <summary>
    /// Maximum hover stamina in seconds.
    /// </summary>
    public float MaxHoverTime =>
        _maxHoverTime;

    /// <summary>
    /// Normalized hover stamina from 0 to 1.
    /// Perfect for a UI radial fill.
    /// </summary>
    public float HoverPercent =>
        Mathf.Clamp01(
            _currentHoverTime /
            Mathf.Max(0.01f, _maxHoverTime)
        );


    // =========================================================
    // UNITY
    // =========================================================

    private void Start()
    {
        _playerController =
            GetComponent<PlayerController>();

        _gravityBody =
            GetComponent<GravityBody>();

        _playerVibration =
            GetComponent<PlayerVibration>();


        // Start with a full hover meter.
        _currentHoverTime =
            _maxHoverTime;
    }


    private void Update()
    {
        // =====================================================
        // HOVER ABILITY DISABLED
        // =====================================================

        if (!_hoverAbilityGranted)
        {
            StopHover();

            return;
        }


        // =====================================================
        // GROUND CHECK
        // =====================================================

        _grounded =
            Physics.Raycast(
                transform.position,
                GravityDirection,
                _playerController.PlayerHeight *
                0.5f +
                0.2f,
                _playerController.GroundMask
            );


        // =====================================================
        // RESET / REFILL WHEN GROUNDED
        // =====================================================

        if (_grounded)
        {
            if (_refillOnLanding)
            {
                RefillHover();
            }

            StopHover();

            _jumpReleased = false;

            return;
        }


        // =====================================================
        // PLAYER HAS RELEASED JUMP
        // =====================================================

        if (!Input.GetButton("Jump"))
        {
            _jumpReleased = true;
        }


        // =====================================================
        // HOVER CHECK
        // =====================================================

        // Hover requires:
        //
        // 1. Airborne
        // 2. Jump has been released
        // 3. Jump is pressed again
        // 4. Hover has not been depleted
        // 5. Hover time remains

        if (!_grounded &&
            _jumpReleased &&
            Input.GetButton("Jump") &&
            !_hoverDepleted &&
            _currentHoverTime > 0f)
        {
            Hover();
        }
        else
        {
            StopHover();
        }
    }


    // =========================================================
    // HOVER
    // =========================================================

    private void Hover()
    {
        _isHovering = true;


        // =====================================================
        // START HOVER VIBRATION
        // =====================================================

        if (_playerVibration != null)
        {
            _playerVibration.StartHoverVibration();
        }


        // =====================================================
        // DRAIN HOVER STAMINA
        // =====================================================

        _currentHoverTime -=
            Time.deltaTime;


        _currentHoverTime =
            Mathf.Max(
                0f,
                _currentHoverTime
            );


        // =====================================================
        // CHECK FOR DEPLETION
        // =====================================================

        if (_currentHoverTime <= 0f)
        {
            _currentHoverTime = 0f;

            _hoverDepleted = true;

            StopHover();

            return;
        }


        // =====================================================
        // GET RIGIDBODY
        // =====================================================

        Rigidbody rb =
            _playerController.RB;


        Vector3 localUp =
            -GravityDirection;


        // =====================================================
        // PRESERVE VERTICAL VELOCITY
        // =====================================================

        Vector3 verticalVelocity =
            Vector3.Project(
                rb.velocity,
                localUp
            );


        // =====================================================
        // DETERMINE WHETHER PLAYER IS FALLING
        // =====================================================

        float verticalSpeed =
            Vector3.Dot(
                rb.velocity,
                localUp
            );


        // =====================================================
        // APPLY UPWARD FORCE WHILE FALLING
        // =====================================================

        if (verticalSpeed <= 0f)
        {
            rb.AddForce(
                localUp *
                _hoverForce,
                ForceMode.Acceleration
            );
        }


        // =====================================================
        // MOVEMENT INPUT
        // =====================================================

        Vector3 moveInput =
            _playerController.Orientation.forward *
            _playerController.VerticalInput
            +
            _playerController.Orientation.right *
            _playerController.HorizontalInput;


        Vector3 localMove =
            Vector3.ProjectOnPlane(
                moveInput,
                localUp
            );


        if (localMove.sqrMagnitude >
            0.001f)
        {
            localMove.Normalize();
        }
        else
        {
            localMove =
                Vector3.zero;
        }


        // =====================================================
        // HOVER MOVEMENT
        // =====================================================

        Vector3 targetHorizontalVelocity =
            localMove *
            _playerController.Speed *
            0.5f;


        Vector3 currentHorizontalVelocity =
            Vector3.ProjectOnPlane(
                rb.velocity,
                localUp
            );


        Vector3 smoothedVelocity =
            Vector3.Lerp(
                currentHorizontalVelocity,
                targetHorizontalVelocity,
                Time.deltaTime * 5f
            );


        // =====================================================
        // CLAMP HOVER SPEED
        // =====================================================

        if (smoothedVelocity.magnitude >
            _maxHoverSpeed)
        {
            smoothedVelocity =
                smoothedVelocity.normalized *
                _maxHoverSpeed;
        }


        // =====================================================
        // APPLY VELOCITY
        // =====================================================

        rb.velocity =
            smoothedVelocity +
            verticalVelocity;
    }


    // =========================================================
    // REFILL HOVER
    // =========================================================

    private void RefillHover()
    {
        _currentHoverTime =
            Mathf.MoveTowards(
                _currentHoverTime,
                _maxHoverTime,
                _hoverRefillSpeed *
                Time.deltaTime
            );


        // =====================================================
        // FULLY REFILLED
        // =====================================================

        if (_currentHoverTime >= _maxHoverTime)
        {
            _currentHoverTime =
                _maxHoverTime;

            _hoverDepleted =
                false;
        }
    }


    // =========================================================
    // STOP HOVER
    // =========================================================

    private void StopHover()
    {
        _isHovering = false;


        // =====================================================
        // STOP HOVER VIBRATION
        // =====================================================

        if (_playerVibration != null)
        {
            _playerVibration.StopHoverVibration();
        }


        // =====================================================
        // CLAMP HOVER TIME
        // =====================================================

        _currentHoverTime =
            Mathf.Clamp(
                _currentHoverTime,
                0f,
                _maxHoverTime
            );
    }


    // =========================================================
    // CLEANUP
    // =========================================================

    private void OnDisable()
    {
        _isHovering = false;

        if (_playerVibration != null)
        {
            _playerVibration.StopHoverVibration();
        }
    }
}