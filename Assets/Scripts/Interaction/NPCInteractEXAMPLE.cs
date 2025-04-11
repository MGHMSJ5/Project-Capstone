using UnityEngine;

public class NPCInteractEXAMPLE : BaseInteract
{   // Add variables here if necessary
    [Header("Own variables")]
    [SerializeField]
    private GameObject _dialogueObject;
    [SerializeField]
    [Tooltip("Distance from NPC to player when the dialogue disappears")]
    private float _disableDistance = 4f;

    [SerializeField]
    [Tooltip("Select the right dialogue type. 1: player's movement freezes. 2: player needs to walk away for the dialogue to disappear. This will NOT update in-game. Set before starting")]
    private DialogueType _dialogueType;

    private enum DialogueType
    {
        Option1,
        Option2
    }
    private string _interactTypeString;
    private GameObject _player;
    private PlayerController _playerController;
    
    private bool _dialogueVisible;

    protected override void Start()
    {
        base.Start();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _interactTypeString = _dialogueType.ToString();
    }

    protected override void Update()
    {
        base.Update();
        // Based on the dialogueType set, the type of dialogue disappearing will change
        // Option 1: Player movement is disabled, remove the dialogue box after pressing space. Player can interact with dialogue again without having to exit and enter the trigger
        // Option 2: Player movement is not disabled. The dialogue box will be removed when the player is past a certain distance from the NPC
        if (_interactTypeString == "Option1")
        {
            DialogueOption1();
        }
        else
        {
            DialogueOption2();
        }
    }

    protected override void InteractFunction()
    {
        base.InteractFunction();
        print("NPC interaction");
        // Enable dialogue object
        _dialogueObject.SetActive(true);

        if (_interactTypeString == "Option1")
        {
            _dialogueVisible = true;
            // Disable the player movement
            //Nick: I don't think this is necessarily needed. I think it would be nice to let the player just move away if they wanted to and the dialogue then closes if they move outside a specific range. Especially if we do the dialogue with text bubbles. If we do dialogue with textboxes like in animal crossing then this is handy. Something I will need to design and then we discuss with the team.
            _playerController.enabled = false;
        }     
    }

    private void DialogueOption1()
    {
        // Dialogue disappears when pressing the Space bar
        if (_dialogueObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            _dialogueObject.SetActive(false);
        }
        // Check if the dialogue disappeared after appearing
        //Nick: How does this even happen at the same time? Isn't the first statement false and the second true?
        //Nick: How does that not lead to an error? Maybe just me not understanding the code.
        // Elise: This part is to make it so that when the dialogue has disappeared after interacting with it, the player can interact with it again (without having to exit and enter the trigger)
        // Elise: To do that, in the InteractFunction, it sets the _dialogueVisible bool to true, so that it knows that the dialogue has appeared/is active.
        // Elise: Then in this if statement below, it checks for if the dialogue has disappeared 'after' it was active. 
        if (!_dialogueObject.activeInHierarchy && _dialogueVisible)
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

    private void DialogueOption2()
    {
        // Dialogue will disappear when the player is outside of a specific range (_disableDistance)
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance > _disableDistance)
        {
            _dialogueObject.SetActive(false);
        }
    }
}
