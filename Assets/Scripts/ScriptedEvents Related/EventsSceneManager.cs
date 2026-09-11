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

    public void WorldPhaseCold()
    {
        ScriptedEvents.Instance.ChangeWorldPhase("Cold");
    }

    public void WorldPhaseWarm()
    {
        ScriptedEvents.Instance.ChangeWorldPhase("Warm");
    }
}
