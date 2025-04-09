using UnityEngine;
// Toolbox EXAMPLE
public class CollectableInteractEXAMPLE : BaseInteract
{
    [SerializeField]
    [Tooltip("Set the amount of resources the player gets")]
    private CollectAmount _resourceAmount;

    private enum CollectAmount
    {
        Two = 2,
        Four = 4,
        Six = 6
    }

    private Animator _animator;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }
    protected override void InteractFunction()
    {
        base.InteractFunction();
        _animator.SetTrigger("Open");
        _UICanvas.ToolBoxPopUp();
    }
}
