using UnityEngine;
// Toolbox EXAMPLE
public class CollectableInteractEXAMPLE : BaseInteract
{
    [Header("Own variables")]
    [SerializeField]
    [Tooltip("Set the amount of resources the player gets")]
    private CollectAmount _resourceAmount;

    private enum CollectAmount
    {
        two = 2,
        four = 4,
        six = 6
    }

    private string _popupText;
    private string _screwUpdateText;
    private Animator _animator;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        // Set the resource pop up text (_resourceAmount is in text)
        _popupText = "Nice!\r\nYou found a <color=yellow>Screw Pair</color>. That's <color=yellow>" + _resourceAmount + "</color> Screws to your total!";
        // Set he updatetext (in int. E.g. "+2")
        _screwUpdateText = "+ " + (int)_resourceAmount;
    }
    protected override void InteractFunction()
    {
        base.InteractFunction();
        // Play the open animation
        _animator.SetTrigger("Open");
        // Set the text on the canvas
        _UICanvas.PopUpText.text = _popupText;
        _UICanvas.ScrewAddedText.text = _screwUpdateText;
        // Run the function in the canvas script that causes the pop ups to appear
        _UICanvas.ToolBoxPopUp();
        
    }
}
