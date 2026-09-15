using UnityEngine;
using UnityEngine.Rendering;

public class DayNightSystem : MonoBehaviour
{
    [Header("Time")]
    [Range(0f, 24f)]
    [SerializeField] private float timeOfDay = 12f;

    [SerializeField] private float fullDayLength = 300f;

    // ============================================================
    // SUN
    // ============================================================

    [Header("Sun")]
    [SerializeField] private Light sunLight;

    // The Sun does NOT rotate with time.
    [SerializeField]
    private Vector3 sunRotation =
        new Vector3(45f, -30f, 0f);

    [Header("Sun Intensity")]
    [SerializeField] private float daySunIntensity = 2f;

    [SerializeField] private float sunsetSunIntensity = 10f;

    [SerializeField] private float nightSunIntensity = 10f;

    [Header("Sun Colors")]
    [SerializeField]
    private Color daySunColor =
        new Color(1f, 0.95f, 0.85f);

    [SerializeField]
    private Color sunsetSunColor =
        new Color(1f, 0.35f, 0.08f);

    [SerializeField]
    private Color nightSunColor =
        new Color(0.70f, 0.78f, 1f);

    // ============================================================
    // AMBIENT
    // ============================================================

    [Header("Ambient")]
    [SerializeField] private float ambientIntensity = 1f;

    [Header("Ambient Colors")]
    [SerializeField]
    private Color dayAmbientColor =
        new Color(0.55f, 0.68f, 0.90f);

    [SerializeField]
    private Color sunsetAmbientColor =
        new Color(0.75f, 0.25f, 0.30f);

    [SerializeField]
    private Color nightAmbientColor =
        new Color(0.28f, 0.38f, 0.80f);

    // ============================================================
    // SKY
    // ============================================================

    [Header("Sky")]
    [SerializeField] private Material skyMaterial;

    [Header("Sky Brightness")]
    [SerializeField] private float daySkyBrightness = 1.05f;

    [SerializeField] private float sunsetSkyBrightness = 1.00f;

    [SerializeField] private float nightSkyBrightness = 1.25f;

    [Header("Sunset Sky Tint")]
    [SerializeField]
    private Color sunsetSkyTint =
        new Color(1f, 0.35f, 0.12f);

    // ============================================================
    // TRANSITION
    // ============================================================

    [Header("Day / Sunset / Night Transition")]
    [SerializeField] private float sunsetStart = 0.20f;

    [SerializeField] private float nightStart = -0.10f;

    private void Start()
    {
        RenderSettings.ambientMode =
            AmbientMode.Flat;

        if (sunLight != null)
        {
            // Sun never moves.
            sunLight.transform.rotation =
                Quaternion.Euler(sunRotation);
        }
    }

    private void Update()
    {
        UpdateTime();
        UpdateLighting();
        UpdateSky();
    }

    // ============================================================
    // TIME
    // ============================================================

    private void UpdateTime()
    {
        if (fullDayLength <= 0f)
            return;

        timeOfDay +=
            (24f / fullDayLength) *
            Time.deltaTime;

        if (timeOfDay >= 24f)
            timeOfDay -= 24f;
    }

    // ============================================================
    // SUN HEIGHT
    // ============================================================

    private float GetSunHeight()
    {
        return Mathf.Sin(
            (timeOfDay - 6f) /
            12f *
            Mathf.PI
        );
    }

    // ============================================================
    // LIGHTING
    // ============================================================

    private void UpdateLighting()
    {
        if (sunLight == null)
            return;

        float sunHeight =
            GetSunHeight();

        // --------------------------------------------------------
        // DAY
        // --------------------------------------------------------

        if (sunHeight >= sunsetStart)
        {
            sunLight.color =
                daySunColor;

            sunLight.intensity =
                daySunIntensity;

            RenderSettings.ambientLight =
                dayAmbientColor *
                ambientIntensity;

            return;
        }

        // --------------------------------------------------------
        // NIGHT
        // --------------------------------------------------------

        if (sunHeight <= nightStart)
        {
            sunLight.color =
                nightSunColor;

            sunLight.intensity =
                nightSunIntensity;

            RenderSettings.ambientLight =
                nightAmbientColor *
                ambientIntensity;

            return;
        }

        // --------------------------------------------------------
        // SUNSET / SUNRISE
        // --------------------------------------------------------

        float t =
            Mathf.InverseLerp(
                nightStart,
                sunsetStart,
                sunHeight
            );

        // Color transitions smoothly.
        sunLight.color =
            Color.Lerp(
                nightSunColor,
                sunsetSunColor,
                t
            );

        // IMPORTANT:
        // Sunset/Sunrise always use the strong light.
        //
        // No intensity interpolation.
        // No dark dip.
        sunLight.intensity =
            sunsetSunIntensity;

        RenderSettings.ambientLight =
            Color.Lerp(
                nightAmbientColor,
                sunsetAmbientColor,
                t
            ) * ambientIntensity;
    }

    // ============================================================
    // SKY
    // ============================================================

    private void UpdateSky()
    {
        if (skyMaterial == null)
            return;

        float sunHeight =
            GetSunHeight();

        float nightBlend;
        float brightness;
        Color skyTint;

        // --------------------------------------------------------
        // DAY
        // --------------------------------------------------------

        if (sunHeight >= sunsetStart)
        {
            nightBlend = 0f;

            brightness =
                daySkyBrightness;

            skyTint =
                Color.white;
        }

        // --------------------------------------------------------
        // NIGHT
        // --------------------------------------------------------

        else if (sunHeight <= nightStart)
        {
            nightBlend = 1f;

            brightness =
                nightSkyBrightness;

            // Do NOT tint the night texture.
            skyTint =
                Color.white;
        }

        // --------------------------------------------------------
        // SUNSET / SUNRISE
        // --------------------------------------------------------

        else
        {
            float t =
                Mathf.InverseLerp(
                    nightStart,
                    sunsetStart,
                    sunHeight
                );

            // Day -> Night texture transition.
            nightBlend =
                1f - t;

            brightness =
                Mathf.Lerp(
                    nightSkyBrightness,
                    sunsetSkyBrightness,
                    t
                );

            // Orange/pink tint peaks around sunset.
            float sunsetAmount =
                1f -
                Mathf.Abs(
                    t * 2f - 1f
                );

            skyTint =
                Color.Lerp(
                    Color.white,
                    sunsetSkyTint,
                    sunsetAmount
                );
        }

        skyMaterial.SetFloat(
            "_NightBlend",
            nightBlend
        );

        skyMaterial.SetFloat(
            "_Brightness",
            brightness
        );

        skyMaterial.SetColor(
            "_SkyColor",
            skyTint
        );
    }
}