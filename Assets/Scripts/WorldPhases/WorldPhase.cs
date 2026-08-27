using System.Collections.Generic;
using UnityEngine;
// Create new scriptable object 'WorldPhase 'for each world, and fill in the phases
[CreateAssetMenu(fileName = "WorldPhase", menuName = "ScriptableObjects/WorldPhase")]
public class WorldPhase : ScriptableObject
{
    public List<string> allWorldPhases = new List<string>();

    public string currentWorldPhase;
}
