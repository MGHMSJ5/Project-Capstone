using UnityEngine;
using UnityEngine.UI;

public class RollingCredits : MonoBehaviour
{
    public float scrollSpeed = 20f;
    public RectTransform creditsTransform;
    public GameObject exitButton;

    public float endYPosition = 1000f; // Where the scrolling ends
    public float extraGap = 200f;      // Extra gap after credits before showing button

    private bool finished = false;

    void Start()
    {
        exitButton.SetActive(false);
    }

    void Update()
    {
        if (!finished)
        {
            creditsTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            if (creditsTransform.anchoredPosition.y >= endYPosition + extraGap)
            {
                finished = true;
                exitButton.SetActive(true);
            }
        }
    }
}