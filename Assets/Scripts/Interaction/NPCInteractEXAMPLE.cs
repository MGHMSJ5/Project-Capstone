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
        // Check for if the dialogue disappeared after appearing
        if (!_dialogueObject.activeInHierarchy && _dialogueVisible)
        {
            _dialogueVisible = false;
            // To interact again while still in the collider:
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
        _playerController.enabled = false;
    }
}
