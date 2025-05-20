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
    private CanvasSceneTransition _canvasSceneTransition;

    private GameObject _mainCamera;
    private GameObject _factoryCamera;
    private Transform _playerTransform;

    void Awake()
    {
        _mainCamera = GameObject.Find("Main Camera");
        _factoryCamera = GameObject.Find("FactoryCamera");
        _canvasSceneTransition = GameObject.Find("Canvas_SceneTransition").GetComponent<CanvasSceneTransition>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    private void Start()
    {
        _factoryCamera.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        _canvasSceneTransition.FadeAction += ChangeCamera;
        _canvasSceneTransition.CanvasFadeInAndOut(2f);
    }

    private void ChangeCamera()
    {
        _playerTransform.localPosition = _teleportLocation.position;
        if (_changeCameraWhenTeleport)
        {
            _mainCamera.SetActive(!_mainCamera.activeInHierarchy);
            _factoryCamera.SetActive(!_factoryCamera.activeInHierarchy);
        }
    }
}
