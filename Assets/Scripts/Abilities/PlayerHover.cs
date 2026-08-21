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
    // STATE
    // =========================================================

    private float _currentHoverTime = 0f;

    private bool _isHovering = false;

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
        // RESET WHEN GROUNDED
        // =====================================================

        if (_grounded)
        {
            _currentHoverTime = 0f;

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
        // 4. Hover time remains

        if (!_grounded &&
            _jumpReleased &&
            Input.GetButton("Jump") &&
            _currentHoverTime <
            _maxHoverTime)
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


        _currentHoverTime +=
            Time.deltaTime;


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

        if (_currentHoverTime >
            _maxHoverTime)
        {
            _currentHoverTime =
                _maxHoverTime;
        }
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