using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
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
    private string lastInputMethod = "Controller";

    private void Start()
    {
        ResetAllPanels();
        mainMenuPanel.SetActive(true);
        SetSelected(firstMainMenuButton);
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            lastInputMethod = "Mouse";
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (lastInputMethod != "Controller")
            {
                RestoreControllerFocus();
                lastInputMethod = "Controller";
            }
        }
    }

    private void RestoreControllerFocus()
    {
        if (confirmNewGamePanel.activeSelf)
            SetSelected(firstConfirmNewGameButton);
        else if (confirmLoadPanel.activeSelf)
            SetSelected(firstConfirmLoadButton);
        else if (loadSavePanel.activeSelf)
            SetSelected(firstLoadSaveMenuButton);
        else
            SetSelected(firstMainMenuButton);
    }

    private void SetSelected(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(obj);
    }

    private void ResetAllPanels()
    {
        mainMenuPanel.SetActive(false);
        confirmNewGamePanel.SetActive(false);
        noSaveFoundPanel.SetActive(false);
        loadSavePanel.SetActive(false);
        confirmLoadPanel.SetActive(false);
    }

    public void OnNewGamePressed()
    {
        ResetAllPanels();

        if (SaveSystem.SaveFileExists(false))
        {
            confirmNewGamePanel.SetActive(true);
            SetSelected(firstConfirmNewGameButton);
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
        ResetAllPanels();
        mainMenuPanel.SetActive(true);
        SetSelected(firstMainMenuButton);
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
        ResetAllPanels();

        bool manualExists = SaveSystem.SaveFileExists(false);
        bool autoExists = SaveSystem.SaveFileExists(true);

        if (!manualExists && !autoExists)
        {
            noSaveFoundPanel.SetActive(true);
            return;
        }

        loadSavePanel.SetActive(true);
        SetSelected(firstLoadSaveMenuButton);

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

        loadSavePanel.SetActive(false);
        confirmLoadPanel.SetActive(true);
        SetSelected(firstConfirmLoadButton);
    }
}

public void OnAutoSaveSelected()
{
    if (SaveSystem.SaveFileExists(true))
    {
        isAutoSaveSelected = true;
        SaveLoadContext.LoadAutoSave = true;

        loadSavePanel.SetActive(false);
        confirmLoadPanel.SetActive(true);
        SetSelected(firstConfirmLoadButton);
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
        loadSavePanel.SetActive(true);
        SetSelected(firstLoadSaveMenuButton);
    }

    public void OnBackFromLoadSave()
    {
        ResetAllPanels();
        mainMenuPanel.SetActive(true);
        SetSelected(firstMainMenuButton);
    }
}
