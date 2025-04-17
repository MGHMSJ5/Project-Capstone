using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteract : BaseInteract
{
    [SerializeField]
    private TextAsset inkJSON;
    protected override void InteractFunction()
    {
        base.InteractFunction();
        print("Talking");
        print(inkJSON.text);
        // To interact again while still in the collider:
        // Will work differntly with the dialogue
        SetInteract(true);
    }
}
