using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasSceneTransition : MonoBehaviour
{
    public Action FadeAction;
    [Header("Fade variables")]
    [SerializeField]
    private float _duration = 1f;

    private GameObject _background;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _background = transform.GetChild(0).gameObject;
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut(_duration));
    }
    // Public function that can be used to change from scenes
    public void ChangeScene(string sceneName)
    {
        // Stop other coroutine to not mess up fading
        // This WILL stop CanvasFadeInAndOut from happening. So make sure that it is not possible to do these two at the same time
        StopAllCoroutines();
        StartCoroutine(TransitionToNextScene(sceneName));
    }
    // Public function that can be used to fade in and out
    // Also plays the action FadeAction, in case anything needs to happen during the fading
    // Make sure to subscribe to that action!
    public void CanvasFadeInAndOut(float pauseTime)
    {   // Pause time is the wais time between the in and out fading (how long the screen will be black) //Thomas - "wais" to wait
        StartCoroutine(FadeInAndOut(pauseTime));
    }

    // Scene transition. Load the next scene and transition, plus unload the current scene
    // !!For Mihai, this part would also need something related to Save/Load (to save/load certain things when transitioning from scenes)!!
    // Also keep in mind that there is a LoadingSceneController script with a 'load' scene
    private IEnumerator TransitionToNextScene(string sceneName)
    {
        yield return StartCoroutine(FadeIn(_duration));
        yield return new WaitForSeconds(0.4f); // Add a bit of delay before scene switching

        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;

        // Wait until the scene is fully loaded
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                // Allow scene activation when loading is done
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
        // Set the name of the next scene in the LoadingSceneController script // Thomas - "LoadingSceneControlle" to LoadingSceneController
        // This is able to happen because both scenes are loaded now
        LoadingSceneController loadingSceneController = GameObject.Find("LoadingCanvas").GetComponent<LoadingSceneController>();
        loadingSceneController.NextSceneName = sceneName;
        // Unload the current scene
        SceneManager.UnloadSceneAsync(currentScene);
    }

    // Used for the fading in and out without scene switching
    // Also has the option to play some functions from other script(s) that subscribe to the event
    private IEnumerator FadeInAndOut(float pauseTime)
    {
        yield return StartCoroutine(FadeIn(_duration));
        yield return new WaitForSeconds(pauseTime);

        // Play functions subscribed to this action, if there is an action
        FadeAction?.Invoke();
        // Clear all subscriptions
        FadeAction = null;

        yield return StartCoroutine(FadeOut(_duration));
    }

    private IEnumerator FadeIn(float duration)
    {
        BackgroundActivate(true);
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            SetAlpha(elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetAlpha(1f); // Ensure the final value is 1
    }

    private IEnumerator FadeOut(float duration)
    {   // Add tiny delay because the fading acts weird without it
        yield return new WaitForSeconds(0.5f);
        BackgroundActivate(true);
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            SetAlpha(1 - (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetAlpha(0.0f); // Ensure final value is 0
        BackgroundActivate(false); // Disable the background so that it doesn't 'cover' other UI
    }

    private void SetAlpha(float value)
    {
        _canvasGroup.alpha = Mathf.Clamp01(value);
    }

    private void BackgroundActivate(bool activate)
    {
        _background.SetActive(activate);
    }
}
