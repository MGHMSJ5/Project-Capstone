using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Main UI")]
    public GameObject pauseMenuUI;
    public Transform playerTransform;

    [Header("Load Save UI")]
    public GameObject loadSavePanel;
    public GameObject confirmLoadPanel;
    public TMP_Text manualSaveInfoText;
    public TMP_Text autoSaveInfoText;

    public static bool IsPaused { get; private set; } = false;

    private bool isPaused = false;
    private bool isAutoSaveSelected;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        IsPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        IsPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SaveAndQuit()
    {
        if (playerTransform != null)
            SaveSystem.SaveGame(playerTransform.position);

        Time.timeScale = 1f;
        IsPaused = false;
        SceneManager.LoadScene("TitleScreen");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        SceneManager.LoadScene("TitleScreen");
    }

    public void SaveGame()
    {
        if (playerTransform != null)
        {
            SaveSystem.SaveGame(playerTransform.position);
            Debug.Log("Game Saved!");
        }
        else
        {
            Debug.LogWarning("Player Transform not set in PauseMenuManager!");
        }
    }

    public void OnLoadSavePressed()
    {
        loadSavePanel.SetActive(true);

        if (SaveSystem.SaveFileExists(false))
        {
            var data = SaveSystem.LoadGame(false);
            manualSaveInfoText.text = $"Scene: {data.sceneName}\nTime: {data.saveTime}";
        }
        else
        {
            manualSaveInfoText.text = "No manual save found.";
        }

        if (SaveSystem.SaveFileExists(true))
        {
            var data = SaveSystem.LoadGame(true);
            autoSaveInfoText.text = $"Scene: {data.sceneName}\nTime: {data.saveTime}";
        }
        else
        {
            autoSaveInfoText.text = "No autosave found.";
        }
    }

    public void OnBackFromLoadSave()
    {
        loadSavePanel.SetActive(false);
    }

    public void OnManualSaveSelected()
    {
        if (SaveSystem.SaveFileExists(false))
        {
            isAutoSaveSelected = false;
            confirmLoadPanel.SetActive(true);
        }
    }

    public void OnAutoSaveSelected()
    {
        if (SaveSystem.SaveFileExists(true))
        {
            isAutoSaveSelected = true;
            confirmLoadPanel.SetActive(true);
        }
    }

    public void OnConfirmLoadPressed()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        confirmLoadPanel.SetActive(false);

        SaveData data = SaveSystem.LoadGame(isAutoSaveSelected);
        if (data != null)
            SceneManager.LoadScene(data.sceneName);
    }

    public void OnCancelLoadPressed()
    {
        confirmLoadPanel.SetActive(false);
    }
}
