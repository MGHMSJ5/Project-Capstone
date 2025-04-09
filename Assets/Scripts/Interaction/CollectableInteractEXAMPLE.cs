using UnityEngine;
// Toolbox EXAMPLE
public class CollectableInteractEXAMPLE : BaseInteract
{
    [SerializeField]
    [Tooltip("Set the amount of resources the player gets")]
    private CollectAmount _resourceAmount;

    private enum CollectAmount
    {
        two,
        four,
        six
    }

    private string _popupText;
    private Animator _animator;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _popupText = "Nice!\r\nYou found a <color=yellow>Screw Pair</color>. That's <color=yellow>" + _resourceAmount + "</color> Screws to your total!";
    }
    protected override void InteractFunction()
    {
        base.InteractFunction();
        _animator.SetTrigger("Open");
        _UICanvas.ToolBoxPopUp();
        _UICanvas.PopUpText.text = _popupText;
    }
}
