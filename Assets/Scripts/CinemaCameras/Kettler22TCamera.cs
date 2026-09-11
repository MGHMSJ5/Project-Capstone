using UnityEngine;

public class Kettler22TCamera : MonoBehaviour
{
    private EventsSceneManager eventsSceneManager;
    private Animator animator;
    private GameObject childCamera;
    private GameObject mainCamera;

    private void Awake()
    {
        eventsSceneManager = GameObject.Find("EventsSceneManager").GetComponent<EventsSceneManager>();
        animator = GetComponent<Animator>();
        childCamera = gameObject.transform.GetChild(0).gameObject;
        mainCamera = GameObject.Find("Main_Camera");
    }

    public void StartWarmPhaseCinematic()
    {
        childCamera.SetActive(true);
        mainCamera.SetActive(false);
        animator.Play("KettleActivate");
    }

    public void ChangePhaseToWarm()
    {
        eventsSceneManager.WorldPhaseWarm();
    }

    public void DisableCamera()
    {
        mainCamera.SetActive(true);
        childCamera.SetActive(false);
    }
}
