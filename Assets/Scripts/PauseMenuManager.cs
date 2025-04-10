using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Transform playerTransform;

    private bool isPaused = false;

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

        // Hides the cursor when exiting pause menu
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Shows cursor when the pause menu is active:
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

        Time.timeScale = 1f; // Unpauses game before quitting.
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
}
