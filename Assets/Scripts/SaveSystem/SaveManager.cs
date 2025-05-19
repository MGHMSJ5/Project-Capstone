using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public Transform playerTransform;

   private void Start()
{
    if (playerTransform != null)
    {
        bool loadAuto = SaveLoadContext.LoadAutoSave;

        if (SaveSystem.SaveFileExists(loadAuto))
        {
            SaveData data = SaveSystem.LoadGame(loadAuto);
            if (data != null)
            {
                string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

                if (data.sceneName == currentScene)
                {
                    playerTransform.position = new Vector3(data.playerX, data.playerY, data.playerZ);
                    Debug.Log($"Loaded {(loadAuto ? "autosave" : "manual save")} at position {playerTransform.position} in scene '{currentScene}'");
                }
                else
                {
                    Debug.LogWarning($"Save data is from scene '{data.sceneName}', but current scene is '{currentScene}'. Skipping position load.");
                }
            }
        }
    }
    else
    {
        Debug.LogWarning("SaveManager: Player Transform not assigned!");
    }
}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (playerTransform != null)
            {
                SaveSystem.SaveGame(playerTransform.position);
            }
            else
            {
                Debug.LogWarning("Player Transform needs to be assigned in SaveManager!");
            }
        }
    }
}
