using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// IMPRTANT: The toaster doesnt use the MinorRepair or MajorRepair script. It will be 'repaired' through dialogue
public class Toaster : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _crumbParticles;
    private Animator _animator;
    private NPCInteract _npcInteract;
    private QuestPoint _questPoint;

    [SerializeField]
    private GameObject _bread1;
    [SerializeField]
    private GameObject _bread2;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _npcInteract = GetComponent<NPCInteract>();
        _questPoint = GetComponent<QuestPoint>();
    }

    private void OnEnable()
    {
        _npcInteract.onSubmitPressed += StartTalkingAnimation;
        _npcInteract.DoneTalkingEvent += EndTalkingAnimation;
    }

    private void OnDisable()
    {
        _npcInteract.onSubmitPressed -= StartTalkingAnimation;
        _npcInteract.DoneTalkingEvent -= EndTalkingAnimation;
    }

    private void StartTalkingAnimation()
    {
        _animator.SetTrigger("IsTalking");
    }

    private void EndTalkingAnimation()
    {
        if (_questPoint.currentQuestState == QuestState.IN_PROGRESS)
        {
            _animator.Play("ToasterRepairing");
        }
        else
        {
            _animator.SetTrigger("DoneTalking");
        }
    }

    public void PlayCrumbParticles()
    {
        _crumbParticles.Play();
    }

    public void EnableBread()
    {
        _bread1.SetActive(true);
        _bread2.SetActive(true);
    }
}
