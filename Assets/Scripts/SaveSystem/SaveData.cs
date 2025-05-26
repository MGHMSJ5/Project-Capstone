using System.Collections.Generic;

[System.Serializable]
public class QuestSaveData
{
    public string questId;
    public QuestState state;
    public int currentStepIndex;
}

[System.Serializable]
public class SaveData
{
    public float playerX;
    public float playerY;
    public float playerZ;

    public string sceneName; //stores saved scene
    public string saveTime; // saves real life time when save made.
    public int screwCount; // saves screw amount

    public List<string> collectedToolboxIDs = new List<string>(); // saves toolbox state
    public List<QuestSaveData> savedQuests = new List<QuestSaveData>(); //saves quest state

    // We'll need to put all other data here too (like abilities received etc.)
}