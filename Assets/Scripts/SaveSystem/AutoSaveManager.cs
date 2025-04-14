using UnityEngine;

public class AutoSaveManager : MonoBehaviour
{
    public Transform playerTransform;
    public float AutosaveInterval = 5f;

    private float timer;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        timer = AutosaveInterval * 60f;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        timer -= Time.unscaledDeltaTime; // Works even when timeScale = 0 (e.g. during pause)
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
