using UnityEngine;

public class AutoSaveManager : MonoBehaviour
{
    private QuestManager questManager;
    public Transform playerTransform;
    public float AutosaveInterval = 5f; // Value can be changed. How often autosave kicks in.

    private float timer;

    private void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        timer = AutosaveInterval * 60f;
        questManager = FindObjectOfType<QuestManager>();
    }

    private void Update()
{
    if (playerTransform == null || PauseMenuManager.IsPaused) return; // Does not work when pause menu is open.

    timer -= Time.deltaTime;

    if (timer <= 0f)
    {
        TryAutoSave();
        timer = AutosaveInterval * 60f;
    }
}

    private void TryAutoSave()
    {
        if (questManager != null)
    {
        SaveSystem.AutoSaveGame(playerTransform.position, questManager);
        Debug.Log($"AutoSave triggered at {Time.time} seconds.");
    }
    }
}