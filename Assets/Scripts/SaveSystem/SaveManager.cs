using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public Transform playerTransform;
    private QuestManager questManager;

    private void Start()
    {
        questManager = FindObjectOfType<QuestManager>();

        if (playerTransform == null)
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
