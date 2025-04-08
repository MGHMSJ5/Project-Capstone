using UnityEngine;

public class BaseInteract : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Select the right interaction. Interact = interacting with environment & NPC's (E & B). Collect = Collect collectibles & pick up objects (F & Y).")]
    private InteractType _interactType;
    private enum InteractType
    {
        Interact,
        Collect
    }

    private bool _canInteract = false;
    private string _interactButton;
    private UIInteract _UIInteract;
    private GameObject _button;

    // Start is called before the first frame update
    void Start()
    {
        _interactButton = _interactType.ToString();
        _UIInteract = GameObject.Find("Canvas").GetComponent<UIInteract>(); // Change 'Canvas' if the name of the canvas changes
        // if the set interact type is 'Interact', then get the InteractButton from UIInteract script. Otherwise get the CollectButton
        _button = _interactButton == "Interact" ? _UIInteract.InteractButton : _UIInteract.ColectButton;
    }

    // Update is called once per frame
    void Update()
    {   // If the player can interact, 
        if (_canInteract)
        {   // and presses the right button
            if (Input.GetButtonDown(_interactButton))
            {
                InteractFunction();
            }
        }
    }
    // Override this function in other script with what needs to happen
    public virtual void InteractFunction()
    {   // What happens when the player can interact/collect and presses the button
        SetInteract(false);
        print("Interacting");
    }

    private void OnTriggerEnter(Collider other)
    {   // When the player enters the trigger
        if (other.gameObject.tag == "Player")
        {
            SetInteract(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {   // When the player exits the trigger
        if (other.gameObject.tag == "Player")
        {
            SetInteract(false);
        }
    }

    // Call from other script in case the player can interact again
    public void SetInteract(bool canInteract)
    {   // Set's if the player can interact or not (+ making the button appear/disappear)
        _button.SetActive(canInteract);
        _canInteract = canInteract;
    }
}
