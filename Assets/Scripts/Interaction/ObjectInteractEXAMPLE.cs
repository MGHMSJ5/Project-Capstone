
public class ObjectInteractEXAMPLE : BaseInteract
{   // Add variables here if necessary
    public override void InteractFunction()
    {
        base.InteractFunction();
        print("Do action (when interacting with object)");
        // To interact again while still in the collider:
        //SetInteract(true);
    }
}
