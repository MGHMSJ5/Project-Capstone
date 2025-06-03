using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCInteract : BaseInteract
{
    private TextAsset inkJSON;

    [Header("Input versions")]
    [Tooltip("The PC version of the dialogue is the default version. If there is also a controller version (when a button is mentioned) then the controller JSON file has to be applied.")]
    //[HideInInspector]
    public TextAsset inkJSON_PC_current;
    [HideInInspector]
    public TextAsset inkJSON_Controller_current;

    private bool _dialogueHasInteracted = false;
    private UIChangeSubject _UIChangeSubject;

    private bool _isInTrigger = false;

    public bool DialogueHasInteracted => _dialogueHasInteracted;

    private void Awake()
    {
        _UIChangeSubject = GameObject.Find("UIChangeManager").GetComponent<UIChangeSubject>();
    }
    private void OnEnable()
    {
        _UIChangeSubject.UISwitch += UIChange;
    }

    private void OnDisable()
    {
        _UIChangeSubject.UISwitch -= UIChange;
        SetInteract(false);
        _isInTrigger = false;
    }
    protected override void Update()
    {
        // If the player can interact, 
        if (_canInteract)
        {   // and presses the right button
            if (Input.GetButtonDown(_interactButton) && !DialogueManager.GetInstance().dialogueIsPlaying)
            {
                InteractFunction();
            }
        }
        //If the dialogue has already been interacted with by the player, and the dialogue panel is gone, the player will be allowed to interact with the dialogue again
        if (!DialogueManager.GetInstance().dialogueIsPlaying && _dialogueHasInteracted)
        {
            _dialogueHasInteracted = false;
            // Enable the dialoue after a delay
            StartCoroutine(DialogueInteractEnableDelay());
        }
    }
    protected override void InteractFunction()
    {
        //Player has interacted with the dialogue
        _dialogueHasInteracted = true;
        base.InteractFunction();
        //if the player interacts with the NPC, it will start the EnterDialogueMode fuction in the DialogueManager Script, using the inkJSON file connected to the NPC
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);       
    }

    public void SwitchDialogue()
    {
        if (inkJSON_Controller_current != null)
        {
            inkJSON = _UIChangeSubject.UsingPCControls ? inkJSON_PC_current : inkJSON_Controller_current;
        }
        else
        {
            inkJSON = inkJSON_PC_current;
        }
    }

    private void UIChange(bool isController)
    {
        // If both the PC and Controller versions are applied:
        if (inkJSON_Controller_current != null)
        {   // Set the JSON file depending on if the controller is used
            inkJSON = isController ? inkJSON_Controller_current : inkJSON_PC_current;
            
        }
        else
        {   // If the controller text is not set, then set the file to the PC version at default
            inkJSON = inkJSON_PC_current;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        // When the player enters the trigger + make sure that the player can't interact again if ticked _interactionOnce
        if (other.gameObject.tag == "Player" && !_hasInteracted)
        {
            SetInteract(true);
            _isInTrigger = true;
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        // When the player exits the trigger
        if (other.gameObject.tag == "Player")
        {
            SetInteract(false);
            _isInTrigger = false;
        }
    }

    private IEnumerator DialogueInteractEnableDelay()
    {
        yield return new WaitForSeconds(1f);
        // Check if the player is still in the trigger
        if (_isInTrigger)
        {
            SetInteract(true);
        }
    }
}
