using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTeleportTrigger : MonoBehaviour
{
    [Header("Teleport")]
    [SerializeField]
    private Transform _teleportLocation;
    [Tooltip("Set to true if the NEXT and PREVIOUS teleport location is the Factory.")]
    [SerializeField]
    private bool _changeCameraWhenTeleport = false;
    [Header("Cave Light Changes")]
    [SerializeField]
    private GameObject _originalDirectionalLight;
    [SerializeField]
    private GameObject _caveDirectionalLight;
    [SerializeField]
    private bool _turnOriginalLightOff = false;

    // Made bool to check if the player has entered, because it was causing some issues withouts
    private bool _hasEntered = false;
    private CanvasSceneTransition _canvasSceneTransition;

    private GameObject _mainCamera;
    private GameObject _factoryCamera;
    private Transform _playerTransform;
    private PlayerController _playerController;
    private Transform _carryPoint;

    void Awake()
    {
        // Find and get all the objects/scripts/transforms
        _mainCamera = GameObject.Find("Main_Camera");
        _factoryCamera = GameObject.Find("FactoryCamera");
        _canvasSceneTransition = GameObject.Find("Canvas_SceneTransition").GetComponent<CanvasSceneTransition>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _playerController = _playerTransform.GetComponent<PlayerController>();
        _carryPoint = GameObject.Find("CarryPoint").GetComponent<Transform>();
    }
    private void Start()
    {
        // Deactivate the factory camera at the start
        _factoryCamera.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the player has entered, and has not entered yet
        if (other.CompareTag("Player") && !_hasEntered)
        {
            _hasEntered = true; // Set to true so that this code won't be called again (to prevent issue)
            Invoke("DisablePlayerMovement", 0.5f); // Invoke this 0.5 seconds later to stop the player movement
            _canvasSceneTransition.FadeAction += ChangeCamera; // Subscribe to FadeAction event
            _canvasSceneTransition.FadeAction += ChangeLight;
            InterruptCarrying(); // Make it so that the player drops the item they are carrying if they are
            _canvasSceneTransition.CanvasFadeInAndOut(2f); // Call the fading
        }
    }

    private void ChangeCamera()
    {   // Function that disables and enables the camera's if needed + changes the player's position & enables the player's movement
        if (_changeCameraWhenTeleport)
        {
            print(_mainCamera.activeSelf);
            _mainCamera.SetActive(!_mainCamera.activeSelf);
            _factoryCamera.SetActive(!_factoryCamera.activeSelf);
        }
        _playerTransform.localPosition = _teleportLocation.position;
        _playerController.enabled = true;
    }
    // Function that disables/enables the directional lights (from the main planet and the cave)
    private void ChangeLight()
    {
        if (_originalDirectionalLight != null)
        {
            _originalDirectionalLight.SetActive(_turnOriginalLightOff);
            _caveDirectionalLight.SetActive(!_turnOriginalLightOff);
        }
    }
    private void DisablePlayerMovement()
    {
        _playerController.enabled = false;
        _hasEntered = false;
    }

    private void InterruptCarrying()
    {
        if (_carryPoint != null && _carryPoint.childCount > 0)
        {
            CarryObjectEXAMPLE carryObjectEXAMPLE = _carryPoint.GetChild(0).GetChild(0).GetComponent<CarryObjectEXAMPLE>();
            if (carryObjectEXAMPLE != null)
            {
                carryObjectEXAMPLE.Interrupt();
            }
        }
    }
}
