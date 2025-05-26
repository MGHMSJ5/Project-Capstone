using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public Transform playerTransform;
    private QuestManager questManager;

    private void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
        if (playerTransform != null)
        {
            bool loadAuto = SaveLoadContext.LoadAutoSave;

            if (SaveSystem.SaveFileExists(loadAuto))
            {
                SaveData data = SaveSystem.LoadGame(questManager, loadAuto);
                if (data != null)
                {
                    playerTransform.position = new Vector3(data.playerX, data.playerY, data.playerZ);
                    Debug.Log($"Loaded {(loadAuto ? "autosave" : "manual save")} at position {playerTransform.position}");
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
                SaveSystem.SaveGame(playerTransform.position, questManager);
            }
            else
            {
                Debug.LogWarning("Player Transform needs to be assigned in SaveManager!");
            }
        }
    }
}