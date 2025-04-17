using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteract : BaseInteract
{
    [SerializeField]
    private TextAsset inkJSON;

    private bool _dialogueHasInteracted = false;

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
            SetInteract(true);
        }

    }
    protected override void InteractFunction()
    {
        base.InteractFunction();
        print("Talking");
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        
        //Player has interacted with the dialogue
        _dialogueHasInteracted = true;
    }
}
