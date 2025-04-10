using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public Transform playerTransform;

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

    private void Start()
    {
        if (SaveSystem.SaveFileExists() && playerTransform != null)
        {
            SaveData data = SaveSystem.LoadGame();
            if (data != null)
            {
                playerTransform.position = new Vector3(data.playerX, data.playerY, data.playerZ);
            }
        }
    }
}