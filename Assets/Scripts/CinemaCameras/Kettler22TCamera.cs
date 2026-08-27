using UnityEngine;

public class Kettler22TCamera : MonoBehaviour
{
    private EventsSceneManager eventsSceneManager;
    private Animator animator;
    private GameObject childCamera;

    private void Awake()
    {
        eventsSceneManager = GameObject.Find("EventsSceneManager").GetComponent<EventsSceneManager>();
        animator = GetComponent<Animator>();
        childCamera = gameObject.transform.GetChild(0).gameObject;
    }

    public void StartWarmPhaseChange()
    {
        childCamera.SetActive(true);
        animator.Play("KettleActivate");
    }

    public void ChangePhaseToWarm()
    {
        eventsSceneManager.WorldPhaseWarm();
    }

    public void DisableCamera()
    {
        childCamera.SetActive(false);
    }
}
