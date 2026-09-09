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
    // COLORS
    // =========================================================

    [Header("Colors")]

    [Tooltip("Color when hover stamina is between 50% and 100%.")]
    [SerializeField] private Color _fullColor = Color.green;

    [Tooltip("Color when hover stamina is between 25% and 50%.")]
    [SerializeField] private Color _warningColor = Color.yellow;

    [Tooltip("Color when hover stamina is between 0% and 25%.")]
    [SerializeField] private Color _dangerColor = Color.red;


    // =========================================================
    // VISIBILITY
    // =========================================================

    [Header("Visibility")]

    [Tooltip("Hide the UI when the player has not unlocked hover.")]
    [SerializeField] private bool _hideWhenUnavailable = true;

    [Tooltip("How long the UI stays visible after the meter becomes full.")]
    [SerializeField] private float _fullDisplayTime = 1f;

    [Tooltip("How quickly the radial UI fades out.")]
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

                UpdateColor();
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


        // =====================================================
        // UPDATE COLOR
        // =====================================================

        UpdateColor();
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
    // UPDATE COLOR
    // =========================================================

    private void UpdateColor()
    {
        float percent =
            _playerHover.HoverPercent;


        Color targetColor;


        // =====================================================
        // 50% - 100% = GREEN
        // =====================================================

        if (percent > 0.50f)
        {
            targetColor =
                _fullColor;
        }


        // =====================================================
        // 25% - 50% = YELLOW
        // =====================================================

        else if (percent > 0.25f)
        {
            targetColor =
                _warningColor;
        }


        // =====================================================
        // 0% - 25% = RED
        // =====================================================

        else
        {
            targetColor =
                _dangerColor;
        }


        // Preserve the alpha used by the fade system.
        targetColor.a =
            _radialImage.color.a;


        // Apply the complete color directly.
        _radialImage.color =
            targetColor;
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


        // Instantly show the hover UI.
        // This prevents the player from missing the
        // beginning of the stamina meter when hovering starts.
        color.a = 1f;


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