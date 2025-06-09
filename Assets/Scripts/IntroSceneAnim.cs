using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

public class IntroSceneAnim : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private GameObject dialoguePanel;
    private Animator animator;

    [SerializeField] private bool goneThroughAnim = false;

    public UnityEvent endOfIntroEvent;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.PlaySoundOnLoop(SoundType.ENGINE, 0.2f);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (goneThroughAnim && dialoguePanel.activeInHierarchy == false)
        {
            animator.SetBool("AfterDialogue", true);
        }
    }

    public void CameraSwitchAnimation()
    {
        if (!DialogueManager.GetInstance().dialogueIsPlaying)
        {
            StartCoroutine(WaitForDialogue());
        }
        StartCoroutine(WaitForAnimation());
    }

    IEnumerator WaitForDialogue()
    {
        yield return new WaitForSeconds(1f);
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(3f);
        goneThroughAnim = true;
    }

    public void EndOfIntro()
    {
        SoundManager.StopSound();
        endOfIntroEvent?.Invoke();
    }
}
