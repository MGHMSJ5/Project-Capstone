using UnityEngine;

public class EventsSceneManager : MonoBehaviour
{
    // Tutorial
    public void MoveChobo()
    {
        ScriptedEvents.Instance.MoveChobo();
    }

    public void StopChobo()
    {
        ScriptedEvents.Instance.StopChobo();
    }

    // Factory
    public void PlugThePlug()
    {
        ScriptedEvents.Instance.PlugInThePlug();
    }

    // Workshop
    public void GetHover()
    {
        ScriptedEvents.Instance.GetHover();
    }
}
