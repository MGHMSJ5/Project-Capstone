
public class ObjectInteractEXAMPLE : BaseInteract
{   // Add variables here if necessary
    protected override void InteractFunction()
    {
        base.InteractFunction();
        print("Object interact (once)");
        // To interact again while still in the collider:
        //SetInteract(true);
    }
}
