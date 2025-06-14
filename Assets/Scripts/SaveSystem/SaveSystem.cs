using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class SaveSystem
{
    private static string manualSaveFilePath => Path.Combine(Application.persistentDataPath, "manual_save.json");
    private static string autoSaveFilePath => Path.Combine(Application.persistentDataPath, "autosave.json");

    // For simplicity, expose a static list here to store toolbox states during runtime
    public static List<string> CurrentCollectedToolboxIDs = new List<string>();

    public static void SaveGame(Vector3 playerPosition, QuestManager questManager)
{
    SaveData data = new SaveData
    {
        playerX = playerPosition.x,
        playerY = playerPosition.y,
        playerZ = playerPosition.z,
        sceneName = SceneManager.GetActiveScene().name,
        saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
        screwCount = RepairResources.GetResourceAmount(RepairTypesOptions.Screws),
        collectedToolboxIDs = new List<string>(CurrentCollectedToolboxIDs),
        savedQuests = questManager.GetQuestSaveData() // use passed-in QuestManager
    };

    string json = JsonUtility.ToJson(data, true);
    File.WriteAllText(manualSaveFilePath, json);
    Debug.Log("Manual Save Complete at " + data.saveTime);
}

public static void AutoSaveGame(Vector3 playerPosition, QuestManager questManager)
{
    SaveData data = new SaveData
    {
        playerX = playerPosition.x,
        playerY = playerPosition.y,
        playerZ = playerPosition.z,
        sceneName = SceneManager.GetActiveScene().name,
        saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
        screwCount = RepairResources.GetResourceAmount(RepairTypesOptions.Screws),
        collectedToolboxIDs = new List<string>(CurrentCollectedToolboxIDs),
        savedQuests = questManager.GetQuestSaveData()
    };

    string json = JsonUtility.ToJson(data, true);
    File.WriteAllText(autoSaveFilePath, json);
    Debug.Log("AutoSave Complete at " + data.saveTime);
}

    public static SaveData LoadGame(QuestManager questManager, bool isAutoSave = false)
{
    string path = isAutoSave ? autoSaveFilePath : manualSaveFilePath;
    if (!File.Exists(path)) return null;

    string json = File.ReadAllText(path);
    SaveData data = JsonUtility.FromJson<SaveData>(json);

    if (data != null)
    {
        CurrentCollectedToolboxIDs = new List<string>(data.collectedToolboxIDs);
        questManager.LoadQuestSaveData(data.savedQuests);
    }

    return data;
}

    public static bool SaveFileExists(bool isAutoSave = false)
    {
        return File.Exists(isAutoSave ? autoSaveFilePath : manualSaveFilePath);
    }

    public static void DeleteSave(bool isAutoSave = false)
    {
        string path = isAutoSave ? autoSaveFilePath : manualSaveFilePath;
        if (File.Exists(path)) File.Delete(path);
    }
}