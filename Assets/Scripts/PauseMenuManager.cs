using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Main UI")]
    public GameObject pauseMenuUI;
    public Transform playerTransform;

    [Header("Load Save UI")]
    public GameObject loadSavePanel;
    public GameObject confirmLoadPanel;

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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SaveAndQuit()
    {
        if (playerTransform != null)
        {
            SaveSystem.SaveGame(playerTransform.position);
        }
        else
        {
            Debug.LogWarning("Player Transform not set in PauseMenuManager!");
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreen");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
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

    // ---------- New Load Save Logic Below ----------

    public void OnLoadSavePressed()
    {
        loadSavePanel.SetActive(true);
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
        else
        {
            Debug.Log("No manual save found.");
        }
    }

    public void OnAutoSaveSelected()
    {
        if (SaveSystem.SaveFileExists(true))
        {
            isAutoSaveSelected = true;
            confirmLoadPanel.SetActive(true);
        }
        else
        {
            Debug.Log("No autosave found.");
        }
    }

    public void OnConfirmLoadPressed()
    {
        Time.timeScale = 1f; // Unpause
        confirmLoadPanel.SetActive(false);
        SaveData data = SaveSystem.LoadGame(isAutoSaveSelected);
        if (data != null)
        {
            SceneManager.LoadScene(data.sceneName);
        }
        else
        {
            Debug.LogWarning("Save data was null.");
        }
    }

    public void OnCancelLoadPressed()
    {
        confirmLoadPanel.SetActive(false);
    }
}
