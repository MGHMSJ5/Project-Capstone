using System;
using UnityEngine;
// IMPORTANT! Do not change this script unless it's necessary for all interact versions!
[RequireComponent(typeof(Collider))]
public class BaseInteract : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Set to true when interacting only needs to happen once")]
    private bool _interactOnce;
    [SerializeField]
    [Tooltip("Select the right interaction. Interact = interacting with environment & NPC's (E & B). Collect = Collect collectibles & pick up objects (F & Y).")]
    private InteractType _interactType;
    [SerializeField]
    [Tooltip("Select if you want the object to interrupt the carrying")]
    private DropObject _interruptCarrying;
    
    private enum InteractType
    {
        Interact,
        Collect
    }

    private enum DropObject
    {
        False,
        True
    }
    private Transform _carryPoint;
    private bool _dropObjectBool;

    protected bool _hasInteracted = false;
    protected bool _canInteract = false;
    protected string _interactButton;
    protected UICanvas _UICanvas;
    private GameObject _button;

    public event Action onSubmitPressed;

    protected virtual void Start()
    {
        // Make sure that the collider is a trigger (this is the area that the player needs to be in in order to interact
        transform.GetComponent<Collider>().isTrigger = true;
        _interactButton = _interactType.ToString();
        _UICanvas = GameObject.Find("Canvas").GetComponent<UICanvas>(); // Change 'Canvas' if the name of the canvas changes
        // if the set interact type is 'Interact', then get the InteractButton from UIInteract script. Otherwise get the CollectButton
        _button = _interactButton == "Interact" ? _UICanvas.InteractButton : _UICanvas.CollectButton;

        // If the interaction needs to interrupt the carrying, then find the object that is the carrypoint
        // (In the carrypoint, the carried object will be set as a child
        _dropObjectBool = _interruptCarrying.ToString() == "True" ? true : false;
        if (_dropObjectBool)
        {
            _carryPoint = GameObject.Find("CarryPoint").GetComponent<Transform>();
        }
    }

    protected virtual void Update()
    {   // If the player can interact, 
        if (_canInteract)
        {   // and presses the right button
            if (Input.GetButtonDown(_interactButton))
            {
                InteractFunction();
            }
        }
    }

    public void InvokeSubmitPressed()
    {
        onSubmitPressed?.Invoke();
    }

    // Override this function in other script with what needs to happen
    protected virtual void InteractFunction()
    {
        onSubmitPressed?.Invoke();
        // What happens when the player can interact/collect and presses the button
        SetInteract(false);
        // Is used to make sure the player can only interact with it once ↓
        if (_interactOnce) { _hasInteracted = true; };

        if (_carryPoint != null && _carryPoint.childCount > 0)
        {
            // Get the carryscript from the child of the child and run the Interrupt() function so that the player drops the carried object
            CarryObjectEXAMPLE carryObjectEXAMPLE = _carryPoint.GetChild(0).GetChild(0).GetComponent<CarryObjectEXAMPLE>();
            if (carryObjectEXAMPLE != null) { carryObjectEXAMPLE.Interrupt(); }
        }
    }

    // Call from other script in case the player can interact again (while still in the trigger)
    protected void SetInteract(bool canInteract)
    {   
        // Set's if the player can interact or not (+ making the button appear/disappear)
        _button.SetActive(canInteract);
        _canInteract = canInteract;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {   // When the player enters the trigger + make sure that the player can't interact again if ticked _interactionOnce
        if (other.gameObject.tag == "Player" && !_hasInteracted)
        {
            SetInteract(true);
        }
    }
    
    protected virtual void OnTriggerExit(Collider other)
    {   // When the player exits the trigger
        if (other.gameObject.tag == "Player")
        {
            SetInteract(false);
        }
    }
}
