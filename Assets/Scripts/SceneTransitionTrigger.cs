using UnityEngine;

[RequireComponent (typeof(Collider))]
public class SceneTransitionTrigger : MonoBehaviour
{
    [Tooltip("Put in the name of the scene it needs to transition to. Make sure that the scene is in the build.")]
    [SerializeField]
    private string _nextSceneName;

    [Tooltip("In the scene that is NOT the main scene (planet) set the type of location of the scene.")]
    [SerializeField]
    private SpawnLocationsType _location;

    private CanvasSceneTransition _canvasSceneTransition;
    private SceneSpawnManager _spawnManager;
    private void Awake()
    {
        _canvasSceneTransition = GameObject.Find("Canvas_SceneTransition").GetComponent<CanvasSceneTransition>();
        _spawnManager = GameObject.Find("SceneSpawnManager").GetComponent<SceneSpawnManager>();
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // First check if the next scene name has been set
        if (_nextSceneName == "")
        {
            Debug.LogError("Can not transition to next scene because _nextSceneName has not been set.");
            return;
        }
        if (other.gameObject.tag == "Player")
        {
            if (_location != SpawnLocationsType.Default)
            {
                _spawnManager.Location = _location;
            }

            _canvasSceneTransition.ChangeScene(_nextSceneName);
        }
    }
}
