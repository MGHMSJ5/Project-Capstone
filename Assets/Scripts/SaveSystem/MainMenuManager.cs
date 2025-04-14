using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject confirmNewGamePanel;
    public GameObject noSaveFoundPanel;
    public GameObject loadSavePanel;
    public GameObject confirmLoadPanel;

    private bool isAutoSaveSelected;

    public void OnNewGamePressed()
    {
        if (SaveSystem.SaveFileExists(false))
        {
            confirmNewGamePanel.SetActive(true);
        }
        else
        {
            StartNewGame();
        }
    }

    public void ConfirmStartNewGame()
    {
        SaveSystem.DeleteSave(false); // Manual save
        StartNewGame();
    }

    private void StartNewGame()
    {
        SceneManager.LoadScene("Gravity");
    }

    // Load Game Flow
    public void OnLoadGamePressed()
    {
        bool manualExists = SaveSystem.SaveFileExists(false);
        bool autoExists = SaveSystem.SaveFileExists(true);

        if (!manualExists && !autoExists)
        {
            noSaveFoundPanel.SetActive(true);
            return;
        }

        loadSavePanel.SetActive(true);
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
        SaveData data = SaveSystem.LoadGame(isAutoSaveSelected);
        if (data != null)
        {
            SceneManager.LoadScene(data.sceneName);
        }
    }

    public void OnCancelLoadPressed()
    {
        confirmLoadPanel.SetActive(false);
    }

    public void OnBackFromLoadSave()
    {
        loadSavePanel.SetActive(false);
    }
}
