using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    private static string manualSaveFilePath => Path.Combine(Application.persistentDataPath, "manual_save.json");
    private static string autoSaveFilePath => Path.Combine(Application.persistentDataPath, "autosave.json");

 public static void SaveGame(Vector3 playerPosition)
{
    SaveData data = new SaveData
    {
        playerX = playerPosition.x,
        playerY = playerPosition.y,
        playerZ = playerPosition.z,
        sceneName = SceneManager.GetActiveScene().name,
        saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"), // date format for when save is made.
        screwCount = RepairResources.GetResourceAmount(RepairTypesOptions.Screws) //screws
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
        screwCount = RepairResources.GetResourceAmount(RepairTypesOptions.Screws)
    };

    string json = JsonUtility.ToJson(data, true);
    File.WriteAllText(autoSaveFilePath, json);
    Debug.Log("AutoSave Complete at " + data.saveTime);
}

    private static SaveData CreateSaveData(Vector3 playerPosition)
    {
        return new SaveData
        {
            playerX = playerPosition.x,
            playerY = playerPosition.y,
            playerZ = playerPosition.z,
            sceneName = SceneManager.GetActiveScene().name
        };
    }

    public static SaveData LoadGame(bool isAutoSave = false)
    {
        string path = isAutoSave ? autoSaveFilePath : manualSaveFilePath;
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
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
