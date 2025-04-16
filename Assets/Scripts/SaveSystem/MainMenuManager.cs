using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject confirmNewGamePanel;
    public GameObject noSaveFoundPanel;
    public GameObject loadSavePanel;
    public GameObject confirmLoadPanel;

    [Header("First Selected Buttons")]
    public GameObject firstMainMenuButton;
    public GameObject firstLoadSaveMenuButton;
    public GameObject firstConfirmLoadButton;
    public GameObject firstConfirmNewGameButton;

    public TMP_Text manualSaveInfoText;
    public TMP_Text autoSaveInfoText;

    private bool isAutoSaveSelected;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstMainMenuButton);
    }

    public void OnNewGamePressed()
    {
        if (SaveSystem.SaveFileExists(false))
        {
            confirmNewGamePanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstConfirmNewGameButton);
        }
        else
        {
            StartNewGame();
        }
    }

    public void ConfirmStartNewGame()
    {
        SaveSystem.DeleteSave(false);
        StartNewGame();
    }

    public void OnCancelNewGamePressed()
    {
        confirmNewGamePanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstMainMenuButton);
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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstLoadSaveMenuButton);

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

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstConfirmLoadButton);
        }
    }

    public void OnAutoSaveSelected()
    {
        if (SaveSystem.SaveFileExists(true))
        {
            isAutoSaveSelected = true;
            SaveLoadContext.LoadAutoSave = true;
            confirmLoadPanel.SetActive(true);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstConfirmLoadButton);
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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstLoadSaveMenuButton);
    }

    public void OnBackFromLoadSave()
    {
        loadSavePanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstMainMenuButton);
    }
}
