using System;
using System.Collections;
using UnityEngine;

public class UIChangeSubject : MonoBehaviour
{
    private bool connected = false;
    public event Action SwitchUI;

    void Awake()
    {
        StartCoroutine(CheckForControllers());
    }
    private void InputHasSwitched()
    {
        SwitchUI?.Invoke();
    }


    IEnumerator CheckForControllers()
    {
        while (true)
        {
            var controllers = Input.GetJoystickNames();

            if (!connected && controllers.Length > 0)
            {
                connected = true;
                Debug.Log("Connected");

            }
            else if (connected && controllers.Length == 0)
            {
                connected = false;
                Debug.Log("Disconnected");
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
