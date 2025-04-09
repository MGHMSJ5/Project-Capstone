using UnityEngine;

public class NPCInteractEXAMPLE : BaseInteract
{   // Add variables here if necessary
    [SerializeField]
    private GameObject _dialogueObject;
    private PlayerController _playerController;
    
    private bool _dialogueVisible;

    protected override void Start()
    {
        base.Start();
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    protected override void Update()
    {
        base.Update();
        // Check if the dialogue disappeared after appearing
        if (!_dialogueObject.activeInHierarchy && _dialogueVisible) //Nick: How does this even happen at the same time? Isn't the first statement false and the second true? How does that not lead to an error? Maybe just me not understanding the code.
        {
            _dialogueVisible = false;
            // To interact again while still in the collider
            SetInteract(true);
            // Enable the player movement
            _playerController.enabled = true;
        }
    }

    protected override void InteractFunction()
    {
        base.InteractFunction();
        print("NPC interaction (repeatable)");
        // Enable dialogue object
        _dialogueObject.SetActive(true);
        _dialogueVisible = true;
        // Disable the player movement
        _playerController.enabled = false; //Nick: I don't think this is necessarily needed. I think it would be nice to let the player just move away if they wanted to and the dialogue then closes if they move outside a specific range. Especially if we do the dialogue with text bubbles. If we do dialogue with textboxes like in animal crossing then this is handy. Something I will need to design and then we discuss with the team.
    }
}
