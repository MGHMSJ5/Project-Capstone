using UnityEngine;

public class AutoSaveManager : MonoBehaviour
{
    public Transform playerTransform;
    public float AutosaveInterval = 5f; //Value can be changed. How often autosave kicks in.

    private float timer;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        timer = AutosaveInterval * 60f;
    }

    private void Update()
{
    if (playerTransform == null || PauseMenuManager.IsPaused) return;

    timer -= Time.deltaTime;

    if (timer <= 0f)
    {
        TryAutoSave();
        timer = AutosaveInterval * 60f;
    }
}


    private void TryAutoSave()
    {
        SaveSystem.AutoSaveGame(playerTransform.position);
        Debug.Log($"AutoSave triggered at {Time.time} seconds.");
    }
}
