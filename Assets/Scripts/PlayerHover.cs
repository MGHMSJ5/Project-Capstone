using UnityEngine;

public class PlayerHover : MonoBehaviour
{
    [Header("Hovering")]
    [SerializeField] private bool _hoverAbilityGranted = true; // Controls if the player has this ability unlocked (or not!)
    [SerializeField] private float _hoverForce = 2f; // Controls how fast the player falls.
    [SerializeField] private float _maxHoverTime = 3f; // How long the player can hover for.
    private float _currentHoverTime = 0f;
    private bool _isHovering = false;

    [Header("Movement")]
    private PlayerController _playerController;
    private GravityBody _gravityBody;

    [Header("Ground Check")]
    private bool _grounded;

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _gravityBody = GetComponent<GravityBody>();
    }

    void Update()
    {
        if (!_hoverAbilityGranted) return;

        // Use actual gravity direction (from GravityBody) for ground check
        _grounded = Physics.Raycast(
            transform.position,
            _gravityBody.GravityDirection,
            _playerController.PlayerHeight * 0.5f + 0.2f,
            _playerController.GroundMask
        );

        if (!_grounded && Input.GetButton("Jump") && _currentHoverTime < _maxHoverTime)
        {
            Hover();
        }
        else
        {
            StopHover();
        }

         // Resets the hover ability when the players lands:

        if (_grounded && !_isHovering)
        {
            _currentHoverTime = 0f;
        }
    }

    private void Hover()
    {
        _isHovering = true;
        _currentHoverTime += Time.deltaTime;

        Rigidbody rb = _playerController.RB;
        Vector3 localUp = -_gravityBody.GravityDirection;

        if (Vector3.Dot(rb.velocity, -localUp) > 0f)
        {
            rb.AddForce(localUp * _hoverForce, ForceMode.Acceleration);
        }

        Vector3 moveInput = _playerController.Orientation.forward * _playerController.VerticalInput +
                            _playerController.Orientation.right * _playerController.HorizontalInput;
        Vector3 localMove = Vector3.ProjectOnPlane(moveInput, localUp).normalized;

        rb.velocity = new Vector3(localMove.x * _playerController.Speed, rb.velocity.y, localMove.z * _playerController.Speed);
    }

    private void StopHover()
    {
        _isHovering = false;

        // If hover time exceeds max hover time, ability stops working until player lands:

        if (_currentHoverTime >= _maxHoverTime)
        {
            _currentHoverTime = _maxHoverTime;
        }
    }
}