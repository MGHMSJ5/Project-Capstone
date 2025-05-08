using UnityEngine;

public class PlayerHover : MonoBehaviour
{
    [Header("Hovering")]
    [SerializeField] private bool _hoverAbilityGranted = true;
    [SerializeField] private float _hoverForce = 2f; // Upward force
    [SerializeField] private float _maxHoverTime = 3f; // How long it lasts
    [SerializeField] private float _maxHoverSpeed = 5f; // Max speed when using this.
    private float _currentHoverTime = 0f;
    private bool _isHovering = false;
    private bool _jumpReleased = false; // Tracks if the player let go of jump button
    private bool _hoverInputHeld = false;

    [Header("Movement")]
    private PlayerController _playerController;
    private GravityBody _gravityBody;

    [Header("Ground Check")]
    private bool _grounded;

    private Vector3 GravityDirection => _gravityBody != null ? _gravityBody.GravityDirection : Vector3.down;

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _gravityBody = GetComponent<GravityBody>();
    }

    void Update()
    {
        if (!_hoverAbilityGranted) return;

        // Ground check
        _grounded = Physics.Raycast(
            transform.position,
            GravityDirection,
            _playerController.PlayerHeight * 0.5f + 0.2f,
            _playerController.GroundMask
        );

        // Resets on ground collision
        if (_grounded)
        {
            _currentHoverTime = 0f;
            _isHovering = false;
            _jumpReleased = false;
            return;
        }

        // Tracks if the jump button was released after jumping
        if (!Input.GetButton("Jump"))
        {
            _jumpReleased = true;
        }

        // Only allow hovers if: not grounded,jump was released at least once (so the user didn't just hold it from jump) and also jump is now being pressed again
        if (!_grounded && _jumpReleased && Input.GetButton("Jump") && _currentHoverTime < _maxHoverTime)
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
        _currentHoverTime += Time.deltaTime;

        Rigidbody rb = _playerController.RB;
        Vector3 localUp = -GravityDirection;

        // Applies upward force
        if (Vector3.Dot(rb.velocity, -localUp) > 0f)
        {
            rb.AddForce(localUp * _hoverForce, ForceMode.Acceleration);
        }

        Vector3 moveInput = _playerController.Orientation.forward * _playerController.VerticalInput +
                            _playerController.Orientation.right * _playerController.HorizontalInput;
        Vector3 localMove = Vector3.ProjectOnPlane(moveInput, localUp).normalized;

        // Adjusts horizontal velocity
        Vector3 currentHorizontalVel = Vector3.ProjectOnPlane(rb.velocity, localUp);
        Vector3 targetHorizontalVel = localMove * _playerController.Speed * 0.5f;
        Vector3 smoothedVel = Vector3.Lerp(currentHorizontalVel, targetHorizontalVel, Time.deltaTime * 5f);

        // Clamps the hover speed, avoids propulsion woo
        if (smoothedVel.magnitude > _maxHoverSpeed)
        {
            smoothedVel = smoothedVel.normalized * _maxHoverSpeed;
        }

        // Applys new velocity:
        rb.velocity = smoothedVel + Vector3.Project(rb.velocity, localUp);
    }

    private void StopHover()
    {
        _isHovering = false;

        if (_currentHoverTime >= _maxHoverTime)
        {
            _currentHoverTime = _maxHoverTime;
        }
    }
}
