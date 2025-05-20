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

    public static void SaveGame(Vector3 playerPosition)
    {
        SaveData data = new SaveData
        {
            playerX = playerPosition.x,
            playerY = playerPosition.y,
            playerZ = playerPosition.z,
            sceneName = SceneManager.GetActiveScene().name,
            saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
            screwCount = RepairResources.GetResourceAmount(RepairTypesOptions.Screws),
            collectedToolboxIDs = new List<string>(CurrentCollectedToolboxIDs) // Save the current list
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(manualSaveFilePath, json);
        Debug.Log("Manual Save Complete at " + data.saveTime);
    }

    public static void AutoSaveGame(Vector3 playerPosition)
    {
        SaveData data = new SaveData
        {
            playerX = playerPosition.x,
            playerY = playerPosition.y,
            playerZ = playerPosition.z,
            sceneName = SceneManager.GetActiveScene().name,
            saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
            screwCount = RepairResources.GetResourceAmount(RepairTypesOptions.Screws),
            collectedToolboxIDs = new List<string>(CurrentCollectedToolboxIDs)
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(autoSaveFilePath, json);
        Debug.Log("AutoSave Complete at " + data.saveTime);
    }

    public static SaveData LoadGame(bool isAutoSave = false)
    {
        string path = isAutoSave ? autoSaveFilePath : manualSaveFilePath;
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // When loading, update the static list so toolboxes can access it
        if (data != null)
        {
            CurrentCollectedToolboxIDs = new List<string>(data.collectedToolboxIDs);
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
