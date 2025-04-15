using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject confirmNewGamePanel;
    public GameObject noSaveFoundPanel;
    public GameObject loadSavePanel;
    public GameObject confirmLoadPanel;

    public TMP_Text manualSaveInfoText;
    public TMP_Text autoSaveInfoText;

    private bool isAutoSaveSelected;

    public void OnNewGamePressed()
    {
        if (SaveSystem.SaveFileExists(false))
            confirmNewGamePanel.SetActive(true);
        else
            StartNewGame();
    }

    public void ConfirmStartNewGame()
    {
        SaveSystem.DeleteSave(false);
        StartNewGame();
    }

    private void StartNewGame()
{
    int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
    if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
    {
        SceneManager.LoadScene(nextSceneIndex);
    }
    else
    {
        Debug.LogWarning("No scene found in build settings!");
    }
}

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

        if (manualExists)
        {
            var data = SaveSystem.LoadGame(false);
            manualSaveInfoText.text = $"{data.sceneName}\nTime: {data.saveTime}";
        }
        else
        {
            manualSaveInfoText.text = "No manual save found.";
        }

        if (autoExists)
        {
            var data = SaveSystem.LoadGame(true);
            autoSaveInfoText.text = $"{data.sceneName}\nTime: {data.saveTime}";
        }
        else
        {
            autoSaveInfoText.text = "No autosave found.";
        }
    }

    public void OnManualSaveSelected()
    {
        if (SaveSystem.SaveFileExists(false))
        {
            isAutoSaveSelected = false;
            SaveLoadContext.LoadAutoSave = false;
            confirmLoadPanel.SetActive(true);
        }
    }

    public void OnAutoSaveSelected()
    {
        if (SaveSystem.SaveFileExists(true))
        {
            isAutoSaveSelected = true;
            SaveLoadContext.LoadAutoSave = true;
            confirmLoadPanel.SetActive(true);
        }
    }

    public void OnConfirmLoadPressed()
    {
        SaveData data = SaveSystem.LoadGame(isAutoSaveSelected);
        if (data != null)
            SceneManager.LoadScene(data.sceneName);
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
