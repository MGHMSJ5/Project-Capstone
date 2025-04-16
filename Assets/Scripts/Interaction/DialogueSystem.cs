using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : BaseInteract
{
    protected override void InteractFunction()
    {
        base.InteractFunction();
        print("Talking");
        // To interact again while still in the collider:
        // Will work differntly with the dialogue
        //SetInteract(true);
    }
}
