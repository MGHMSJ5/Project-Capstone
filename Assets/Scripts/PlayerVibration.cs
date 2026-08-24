using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVibration : MonoBehaviour
{
    // =========================================================
    // LANDING VIBRATION
    // =========================================================

    [Header("Landing Vibration")]

    [Tooltip("Minimum downward impact speed required to trigger landing vibration.")]
    [SerializeField]
    private float _minimumLandingImpactSpeed = 2f;

    [Tooltip("Controller vibration strength for landing impacts.")]
    [SerializeField]
    [Range(0f, 1f)]
    private float _landingVibrationStrength = 0.7f;

    [Tooltip("How long the landing vibration lasts.")]
    [SerializeField]
    private float _landingVibrationDuration = 0.12f;


    // =========================================================
    // HOVER VIBRATION
    // =========================================================

    [Header("Hover Vibration")]

    [Tooltip("Controller vibration strength while hovering.")]
    [SerializeField]
    [Range(0f, 1f)]
    private float _hoverVibrationStrength = 0.3f;


    // =========================================================
    // STATE
    // =========================================================

    private bool _hoverVibrationRequested = false;

    private bool _landingVibrationActive = false;

    private Coroutine _landingVibrationCoroutine;


    // =========================================================
    // PUBLIC PROPERTIES
    // =========================================================

    public float MinimumLandingImpactSpeed =>
        _minimumLandingImpactSpeed;


    // =========================================================
    // HOVER
    // =========================================================

    /// <summary>
    /// Requests that the hover vibration should be active.
    /// The vibration will not override a higher-priority landing vibration.
    /// </summary>
    public void StartHoverVibration()
    {
        _hoverVibrationRequested = true;

        UpdateVibration();
    }


    /// <summary>
    /// Requests that the hover vibration should stop.
    /// </summary>
    public void StopHoverVibration()
    {
        _hoverVibrationRequested = false;

        UpdateVibration();
    }


    // =========================================================
    // LANDING
    // =========================================================

    /// <summary>
    /// Plays a landing vibration.
    /// </summary>
    public void PlayLandingVibration()
    {
        if (_landingVibrationCoroutine != null)
        {
            StopCoroutine(_landingVibrationCoroutine);
        }

        _landingVibrationCoroutine =
            StartCoroutine(
                LandingVibrationRoutine()
            );
    }


    private IEnumerator LandingVibrationRoutine()
    {
        _landingVibrationActive = true;

        UpdateVibration();

        yield return new WaitForSeconds(
            _landingVibrationDuration
        );

        _landingVibrationActive = false;

        _landingVibrationCoroutine = null;

        UpdateVibration();
    }


    // =========================================================
    // VIBRATION OUTPUT
    // =========================================================

    private void UpdateVibration()
    {
        Gamepad gamepad =
            Gamepad.current;

        if (gamepad == null)
        {
            return;
        }


        // =====================================================
        // LANDING HAS PRIORITY
        // =====================================================

        if (_landingVibrationActive)
        {
            gamepad.SetMotorSpeeds(
                _landingVibrationStrength,
                _landingVibrationStrength
            );

            return;
        }


        // =====================================================
        // HOVER
        // =====================================================

        if (_hoverVibrationRequested)
        {
            gamepad.SetMotorSpeeds(
                _hoverVibrationStrength,
                _hoverVibrationStrength
            );

            return;
        }


        // =====================================================
        // NOTHING ACTIVE
        // =====================================================

        gamepad.SetMotorSpeeds(
            0f,
            0f
        );
    }


    // =========================================================
    // CLEANUP
    // =========================================================

    private void OnDisable()
    {
        _hoverVibrationRequested = false;
        _landingVibrationActive = false;

        if (_landingVibrationCoroutine != null)
        {
            StopCoroutine(
                _landingVibrationCoroutine
            );

            _landingVibrationCoroutine = null;
        }

        Gamepad gamepad =
            Gamepad.current;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(
                0f,
                0f
            );
        }
    }


    private void OnDestroy()
    {
        Gamepad gamepad =
            Gamepad.current;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(
                0f,
                0f
            );
        }
    }
}