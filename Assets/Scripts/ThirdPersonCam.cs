using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _playerObj;
    [SerializeField] private Rigidbody _rb;

    private float _rotationSpeed = 15;

    void Start()
    {   //Cursor invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (_player == null || _orientation == null) return;

        //Calculate view direction (horizontal direction to player)
        Vector3 viewDir = (_player.position - transform.position).normalized;

        //Compute the right vector (ensures stable side transitions)
        Vector3 right = Vector3.Cross(_player.up, viewDir).normalized;

        //If right vector gets too small (near top/bottom), use previous right direction
        if (right.sqrMagnitude < 0.001f)
        {
            right = _orientation.right;
        }

        //Compute forward direction (prevents flipping on the sides)
        Vector3 adjustedForward = Vector3.Cross(right, _player.up).normalized;

        //Smoothly adjust the up vector
        Vector3 safeUp = Vector3.Lerp(_orientation.up, _player.up, Time.deltaTime * 5f).normalized;

        //Apply final rotation smoothly
        Quaternion targetRotation = Quaternion.LookRotation(adjustedForward, safeUp);
        _orientation.rotation = Quaternion.Slerp(_orientation.rotation, targetRotation, Time.deltaTime * 5f);

        //Rotate the player object
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = _orientation.forward * verticalInput + _orientation.right * horizontalInput;

        //Check for player input
        if (inputDir != Vector3.zero)
        {
            //Ensure player rotates while staying aligned with the planet surface
            Quaternion playerTargetRotation = Quaternion.LookRotation(inputDir.normalized, _player.up);
            _playerObj.rotation = Quaternion.Slerp(_playerObj.rotation, playerTargetRotation, _rotationSpeed * Time.deltaTime);
        }

        //Debugging (visualize directions)
        Debug.DrawRay(_player.position, safeUp * 2, Color.green);  // Up direction
        Debug.DrawRay(_player.position, adjustedForward * 2, Color.blue);  // Forward direction
        Debug.DrawRay(_player.position, right * 2, Color.red);  // Right direction

        //Debug.Log("Orientation Rotation: " + _orientation.rotation.eulerAngles);
    }
}
