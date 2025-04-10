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

        // Hide cursor when resuming the game
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;  // Optionally lock the cursor when not in the pause menu
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Show cursor when the pause menu is active
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Make sure cursor is unlocked while in the menu
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

        Time.timeScale = 1f; // Reset time scale before quitting
        SceneManager.LoadScene("TitleScreen");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreen");
    }

    // New method to handle the "Save" button functionality
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
