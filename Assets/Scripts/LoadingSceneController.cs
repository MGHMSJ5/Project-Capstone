using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
// Loading scene is used to avoid clash betwene two scenes leading //Thomas - "betwene" to between
public class LoadingSceneController : MonoBehaviour
{   // String is set from CanvasSceneTransition while loading
    public string NextSceneName
    {
        set { _nextSceneName = value; }
    }
    [SerializeField]
    private string _nextSceneName = null;

    private bool _isLoading = false;

    private void Update()
    {
        if (_nextSceneName != "0" && !_isLoading)
        {
            _isLoading = true;
            StartCoroutine(LoadNextScene());
        }
    }

    private IEnumerator LoadNextScene()
    {   // Like in CanvasSceneTransition, load the next scene, and unload this scene
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_nextSceneName, LoadSceneMode.Additive);
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

        // Unload this scene
        SceneManager.UnloadSceneAsync("LoadingScene");
    }
}
