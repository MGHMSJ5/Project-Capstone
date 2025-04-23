using UnityEngine;

public class CanvasSceneTransitionUsageEXAMPLE : MonoBehaviour
{
    [Tooltip("Set the name of the scene to go to. Make sure that the name is the same, and that the scene is in the Build settings.")]
    [SerializeField]
    private string _nextSceneName;
    private CanvasSceneTransition _canvasSceneTransition;

    // Just used for an example with the in-scene fading and action from CanvasSceneTransition
    [SerializeField]
    private GameObject _cube;
    [SerializeField]
    private float _pauseTime = 2f;

    private void Awake()
    {
        _canvasSceneTransition = GameObject.Find("Canvas_SceneTransition").GetComponent<CanvasSceneTransition>();
    }
    void Update()
    {
        // Transition from scene example with key-press
        if (Input.GetKeyDown(KeyCode.T))
        {
            _canvasSceneTransition.ChangeScene(_nextSceneName);
        }
        // Do an in-scene fade + usage of the FadeAction example with key-press
        // DO NOT SPAM PRESS, it'll mess it up
        // When this is used in game, make sure this function is only called once...
        if (Input.GetKeyDown(KeyCode.Y) && _cube != null)
        {
            _canvasSceneTransition.FadeAction += CubeAppear;
            _canvasSceneTransition.CanvasFadeInAndOut(_pauseTime);
        }
    }
    // Function that will be subscribed to FadeAtion
    private void CubeAppear()
    {
        _cube.SetActive(true);
    }
}
