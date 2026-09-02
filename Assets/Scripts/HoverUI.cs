using UnityEngine;
using UnityEngine.UI;

public class HoverUI : MonoBehaviour
{
    [Header("References")]

    [Tooltip("The radial Image used to display hover stamina.")]
    [SerializeField] private Image _radialImage;

    [Tooltip("The player's hover component.")]
    [SerializeField] private PlayerHover _playerHover;


    // =========================================================
    // VISIBILITY
    // =========================================================

    [Header("Visibility")]

    [Tooltip("Hide the UI when the player has not unlocked hover.")]
    [SerializeField] private bool _hideWhenUnavailable = true;

    [Tooltip("How long the UI stays visible after the meter becomes full.")]
    [SerializeField] private float _fullDisplayTime = 1f;

    [Tooltip("How quickly the UI fades out.")]
    [SerializeField] private float _fadeOutSpeed = 5f;


    // =========================================================
    // ANIMATION
    // =========================================================

    [Header("Animation")]

    [Tooltip("How quickly the radial meter catches up when the actual stamina changes suddenly.")]
    [SerializeField] private float _fillCatchupSpeed = 5f;


    // =========================================================
    // STATE
    // =========================================================

    private float _displayedFill = 1f;

    private float _fullTimer = 0f;

    private bool _isVisible = false;


    // =========================================================
    // UNITY
    // =========================================================

    private void Awake()
    {
        if (_radialImage != null)
        {
            Color color =
                _radialImage.color;

            color.a = 0f;

            _radialImage.color =
                color;

            _radialImage.enabled = false;
        }
    }


    private void Start()
    {
        if (_playerHover != null)
        {
            _displayedFill =
                _playerHover.HoverPercent;

            if (_radialImage != null)
            {
                _radialImage.fillAmount =
                    _displayedFill;
            }
        }
    }


    private void Update()
    {
        if (_playerHover == null ||
            _radialImage == null)
        {
            return;
        }


        // =====================================================
        // ABILITY CHECK
        // =====================================================

        if (_hideWhenUnavailable &&
            !_playerHover._hoverAbilityGranted)
        {
            HideImmediately();

            return;
        }


        // =====================================================
        // HOVERING
        // =====================================================

        if (_playerHover.IsHovering)
        {
            _fullTimer = 0f;

            Show();
        }


        // =====================================================
        // REFILLING
        // =====================================================

        else if (_playerHover.IsRefillingHover)
        {
            bool isFull =
                _playerHover.HoverPercent >= 0.999f;


            if (!isFull)
            {
                _fullTimer = 0f;

                Show();
            }
            else
            {
                _fullTimer +=
                    Time.deltaTime;


                if (_fullTimer <
                    _fullDisplayTime)
                {
                    Show();
                }
                else
                {
                    FadeOut();
                }
            }
        }


        // =====================================================
        // NOTHING HAPPENING
        // =====================================================

        else
        {
            _fullTimer = 0f;

            FadeOut();
        }


        // =====================================================
        // UPDATE FILL
        // =====================================================

        UpdateFill();
    }


    // =========================================================
    // UPDATE FILL
    // =========================================================

    private void UpdateFill()
    {
        float targetFill =
            _playerHover.HoverPercent;


        // While refilling, directly follow the actual
        // stamina value so the animation is accurate.
        if (_playerHover.IsRefillingHover)
        {
            _displayedFill =
                targetFill;
        }
        else
        {
            // Smooth sudden changes such as stamina being
            // consumed while hovering.
            _displayedFill =
                Mathf.MoveTowards(
                    _displayedFill,
                    targetFill,
                    _fillCatchupSpeed *
                    Time.deltaTime
                );
        }


        _radialImage.fillAmount =
            _displayedFill;
    }


    // =========================================================
    // SHOW
    // =========================================================

    private void Show()
    {
        _isVisible = true;

        _radialImage.enabled = true;


        Color color =
            _radialImage.color;


        color.a =
            Mathf.MoveTowards(
                color.a,
                1f,
                _fadeOutSpeed *
                Time.deltaTime
            );


        _radialImage.color =
            color;
    }


    // =========================================================
    // FADE OUT
    // =========================================================

    private void FadeOut()
    {
        if (!_isVisible)
        {
            _radialImage.enabled = false;

            return;
        }


        Color color =
            _radialImage.color;


        color.a =
            Mathf.MoveTowards(
                color.a,
                0f,
                _fadeOutSpeed *
                Time.deltaTime
            );


        _radialImage.color =
            color;


        if (color.a <= 0f)
        {
            _radialImage.enabled = false;

            _isVisible = false;
        }
    }


    // =========================================================
    // HIDE IMMEDIATELY
    // =========================================================

    private void HideImmediately()
    {
        _isVisible = false;

        _fullTimer = 0f;


        if (_radialImage != null)
        {
            Color color =
                _radialImage.color;

            color.a = 0f;

            _radialImage.color =
                color;

            _radialImage.enabled = false;
        }
    }
}