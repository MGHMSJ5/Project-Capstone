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
    {
        if (_canInteract)
        {
            if (Input.GetButtonDown(_interactButton))
            {
                InteractFunction();
            }
        }
    }

    public virtual void InteractFunction()
    {   // What happens when the player can interact/collect and presses the button
        print("Interacting");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _button.SetActive(true);
            _canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _button.SetActive(false);
            _canInteract = false;
        }
    }
}
