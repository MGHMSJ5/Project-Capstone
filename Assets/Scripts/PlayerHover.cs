using System.Collections;
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

    [Header("Ground Check")]
    private bool _grounded;

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!_hoverAbilityGranted) return; // Checks if the ability is enabled.

        // Checks to see if player is in mid-air/jumping. Works the same like the one in PlayerController:

        _grounded = Physics.Raycast(transform.position, Vector3.down, _playerController.PlayerHeight * 0.5f + 0.2f, _playerController.GroundMask);

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
        if (rb.velocity.y < 0f) // Note: works only if the player is falling.
        {
            rb.AddForce(Vector3.up * _hoverForce, ForceMode.Acceleration);
        }

        // This allows for horizontal movement during hovering.

        Vector3 moveDirection = _playerController.Orientation.forward * _playerController.VerticalInput +
                                _playerController.Orientation.right * _playerController.HorizontalInput;
        rb.velocity = new Vector3(moveDirection.x * _playerController.Speed, rb.velocity.y, moveDirection.z * _playerController.Speed);
    }

    private void StopHover()
    {
        _isHovering = false;

        // If hover time exceeds max hover time, ability stops working until player lands.

        if (_currentHoverTime >= _maxHoverTime)
        {
            _currentHoverTime = _maxHoverTime;
        }
    }
}
