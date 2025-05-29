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
    public GameObject creditsPanel;
    public GameObject quitConfirmPanel;

    [Header("First Selected Buttons")]
    public GameObject firstCreditsButton;
    public GameObject firstQuitConfirmButton;
    public GameObject firstMainMenuButton;
    public GameObject firstLoadSaveMenuButton;
    public GameObject firstConfirmLoadButton;
    public GameObject firstConfirmNewGameButton;

    public TMP_Text manualSaveInfoText;
    public TMP_Text autoSaveInfoText;

    private QuestManager questManager;
    private bool isAutoSaveSelected;
    private string lastInputMethod = "Controller";

    private CanvasSceneTransition _canvasSceneTransition;
    private void Awake()
    {
        _canvasSceneTransition = GameObject.Find("Canvas_SceneTransition").GetComponent<CanvasSceneTransition>();
    }

    private void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
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
        creditsPanel.SetActive(false);
        quitConfirmPanel.SetActive(false);
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
            // Get the name of the next scene
            string scenePath = SceneUtility.GetScenePathByBuildIndex(nextSceneIndex);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            // Go to next scene using the fading canvas
            _canvasSceneTransition.ChangeScene(sceneName);
            //SceneManager.LoadScene(nextSceneIndex);
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
            var data = SaveSystem.LoadGame(questManager, false);
            manualSaveInfoText.text = $"{data.sceneName}\nTime: {data.saveTime}";
        }
        else
        {
            manualSaveInfoText.text = "No manual save found.";
        }

        if (autoExists)
        {
            var data = SaveSystem.LoadGame(questManager, true);
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
        SaveData data = SaveSystem.LoadGame(questManager, isAutoSaveSelected);
        if (data != null)
        // Go to next scene using the fading canvas
        _canvasSceneTransition.ChangeScene(data.sceneName);

        //SceneManager.LoadScene(data.sceneName);
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

    public void OnCreditsPressed()
    {
    ResetAllPanels();
    creditsPanel.SetActive(true);
    SetSelected(firstCreditsButton);
    }

    public void OnCloseCreditsPressed()
    {
    ResetAllPanels();
    mainMenuPanel.SetActive(true);
    SetSelected(firstMainMenuButton);
    }

    public void OnQuitPressed()
    {
    ResetAllPanels();
    quitConfirmPanel.SetActive(true);
    SetSelected(firstQuitConfirmButton);
    }

    public void OnConfirmQuitPressed()
    {
    Application.Quit();
    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false; // For helping us test before making a build within unity
    #endif
    }

    public void OnCancelQuitPressed()
    {
    ResetAllPanels();
    mainMenuPanel.SetActive(true);
    SetSelected(firstMainMenuButton);
    }

    public void OnBackFromNoSaveFound()
    {
    ResetAllPanels();
    mainMenuPanel.SetActive(true);
    SetSelected(firstMainMenuButton);
    }
}