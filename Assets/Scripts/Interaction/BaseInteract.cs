using UnityEngine;
// IMPORTANT! Do not change this script unless it's necessary for all interact versions!
[RequireComponent(typeof(Collider))]
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
    private UICanvas _UIInteract;
    private GameObject _button;

    void Start()
    {
        // Make sure that the collider is a trigger (this is the area that the player needs to be in in order to interact
        transform.GetComponent<Collider>().isTrigger = true;
        _interactButton = _interactType.ToString();
        _UIInteract = GameObject.Find("Canvas").GetComponent<UICanvas>(); // Change 'Canvas' if the name of the canvas changes
        // if the set interact type is 'Interact', then get the InteractButton from UIInteract script. Otherwise get the CollectButton
        _button = _interactButton == "Interact" ? _UIInteract.InteractButton : _UIInteract.CollectButton;
    }

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
    }

    // Call from other script in case the player can interact again
    public void SetInteract(bool canInteract)
    {   // Set's if the player can interact or not (+ making the button appear/disappear)
        _button.SetActive(canInteract);
        _canInteract = canInteract;
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
}
