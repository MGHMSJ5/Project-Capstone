using UnityEngine;

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

    private float _currentHoverTime = 0f;

    private bool _isHovering = false;

    // Player must release Space before hover can activate.
    private bool _jumpReleased = false;

    [Header("Movement")]

    private PlayerController _playerController;
    private GravityBody _gravityBody;

    [Header("Ground Check")]

    private bool _grounded;

    private Vector3 GravityDirection =>
        _gravityBody != null
            ? _gravityBody.GravityDirection
            : Vector3.down;

    public bool IsHovering =>
        _isHovering;

    private void Start()
    {
        _playerController =
            GetComponent<PlayerController>();

        _gravityBody =
            GetComponent<GravityBody>();
    }

    private void Update()
    {
        // Hover ability disabled.
        if (!_hoverAbilityGranted)
        {
            StopHover();

            return;
        }

        // Ground check.
        _grounded =
            Physics.Raycast(
                transform.position,
                GravityDirection,
                _playerController.PlayerHeight *
                0.5f +
                0.2f,
                _playerController.GroundMask
            );

        // Reset when grounded.
        if (_grounded)
        {
            _currentHoverTime = 0f;
            _isHovering = false;
            _jumpReleased = false;

            return;
        }

        // Player has released Space.
        if (!Input.GetButton("Jump"))
        {
            _jumpReleased = true;
        }

        // Hover requires:
        //
        // 1. Airborne
        // 2. Space has been released
        // 3. Space is pressed again
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

    private void Hover()
    {
        _isHovering = true;

        _currentHoverTime +=
            Time.deltaTime;

        Rigidbody rb =
            _playerController.RB;

        Vector3 localUp =
            -GravityDirection;

        // Preserve vertical velocity.
        Vector3 verticalVelocity =
            Vector3.Project(
                rb.velocity,
                localUp
            );

        // Determine whether player is falling.
        float verticalSpeed =
            Vector3.Dot(
                rb.velocity,
                localUp
            );

        // Apply upward force while falling.
        if (verticalSpeed <= 0f)
        {
            rb.AddForce(
                localUp *
                _hoverForce,
                ForceMode.Acceleration
            );
        }

        // Movement input.
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

        // Hover movement is slower than normal movement.
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

        // Clamp hover speed.
        if (smoothedVelocity.magnitude >
            _maxHoverSpeed)
        {
            smoothedVelocity =
                smoothedVelocity.normalized *
                _maxHoverSpeed;
        }

        // Apply horizontal + vertical velocity.
        rb.velocity =
            smoothedVelocity +
            verticalVelocity;
    }

    private void StopHover()
    {
        _isHovering = false;

        if (_currentHoverTime >
            _maxHoverTime)
        {
            _currentHoverTime =
                _maxHoverTime;
        }
    }
}