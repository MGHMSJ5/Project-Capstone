using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIChangeSubject : MonoBehaviour
{
    [Tooltip("Action that needs to be subscribed to with a function if the UI needs to be changed.")]
    public event Action<bool> UISwitch;

    private Coroutine _connectCheckCoroutine;

    public bool UsingPCControls = true;

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
        // Check for if a controller is aleady connected
        if (_connectCheckCoroutine != null) StopCoroutine(_connectCheckCoroutine);
        {
            _connectCheckCoroutine = StartCoroutine(ConfigmControllerConnection(true));
        }
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void InputChanged(bool inputController)
    {
        UsingPCControls = !inputController;
        UISwitch?.Invoke(inputController);
    }

    // Function that is used to check if a controller has been conntected or disconnected
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        // Check if the device is a GamePad
        if (device is Gamepad)
        {   // Handle different types of device changes
            switch (change)
            {
                case InputDeviceChange.Added:
                    if (_connectCheckCoroutine != null) StopCoroutine(_connectCheckCoroutine);
                    {
                        _connectCheckCoroutine = StartCoroutine(ConfigmControllerConnection(true));
                    }
                    break;
                case InputDeviceChange.Removed:
                    if (_connectCheckCoroutine != null) StopCoroutine(_connectCheckCoroutine);
                    {
                        _connectCheckCoroutine = StartCoroutine(ConfigmControllerConnection(false));
                    }
                    break;
            }
        }
    }

    private IEnumerator ConfigmControllerConnection(bool connecting)
    {
        yield return new WaitForSeconds(1f);

        // Check for controller connecting, and if there is a Gamepad active
        if (connecting && Gamepad.current != null)
        {
            // Change the input and break out of the Coroutine
            InputChanged(true);
            yield break;
        }
        // Otherwise the controller is not connected, so change the UI back to 'keyboard'
        InputChanged(false);
    }
}
