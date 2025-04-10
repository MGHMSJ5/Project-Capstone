using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    private static string saveFilePath => Path.Combine(Application.persistentDataPath, "savefile.json");

   public static void SaveGame(Vector3 playerPosition)
{
    SaveData data = new SaveData
    {
        playerX = playerPosition.x,
        playerY = playerPosition.y,
        playerZ = playerPosition.z,
        sceneName = SceneManager.GetActiveScene().name // Saves current scene the player is in.
    };

    string json = JsonUtility.ToJson(data, true);
    File.WriteAllText(saveFilePath, json);
    Debug.Log("Game Saved!");
}

    public static SaveData LoadGame()
    {
        if (!File.Exists(saveFilePath)) return null;

        string json = File.ReadAllText(saveFilePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        return data;
    }

    public static bool SaveFileExists()
    {
        return File.Exists(saveFilePath);
    }

    public static void DeleteSave()
    {
        if (SaveFileExists())
            File.Delete(saveFilePath);
    }
}