using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Main UI")]
    public GameObject pauseMenuUI;
    public GameObject firstPauseMenuButton;

    [Header("Load Save UI")]
    public GameObject loadSavePanel;
    public GameObject confirmLoadPanel;
    public GameObject firstLoadSaveButton;
    public GameObject firstConfirmLoadButton;
    public TMP_Text manualSaveInfoText;
    public TMP_Text autoSaveInfoText;

    public Transform playerTransform;
    public static bool IsPaused { get; private set; } = false;

    private QuestManager questManager;
    private bool isPaused = false;
    private bool isAutoSaveSelected;
    private string lastInputMethod = "Controller";

    private CanvasSceneTransition _canvasSceneTransition;
    private void Awake()
    {
        _canvasSceneTransition = GameObject.Find("Canvas_SceneTransition").GetComponent<CanvasSceneTransition>();
    }
    void Start()
    {   
        questManager = FindObjectOfType<QuestManager>();
        //Cursor invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }

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
        if (confirmLoadPanel.activeSelf)
            SetSelected(firstConfirmLoadButton);
        else if (loadSavePanel.activeSelf)
            SetSelected(firstLoadSaveButton);
        else
            SetSelected(firstPauseMenuButton);
    }

    private void SetSelected(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(obj);
    }

    private void ResetAllPanels()
    {
        pauseMenuUI.SetActive(false);
        loadSavePanel.SetActive(false);
        confirmLoadPanel.SetActive(false);
    }

    public void Pause()
    {
        ResetAllPanels();
        pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
        IsPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SetSelected(firstPauseMenuButton);
    }

    public void Resume()
    {
        ResetAllPanels();

        Time.timeScale = 1f;
        isPaused = false;
        IsPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SaveAndQuit()
    {
        if (playerTransform != null)
            SaveSystem.SaveGame(playerTransform.position, questManager);

        Time.timeScale = 1f;
        IsPaused = false;
        _canvasSceneTransition.ChangeScene("TitleScreen");
        //SceneManager.LoadScene("TitleScreen");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        _canvasSceneTransition.ChangeScene("TitleScreen");
        //SceneManager.LoadScene("TitleScreen");
    }

    public void SaveGame()
    {
        if (playerTransform != null)
        {
            SaveSystem.SaveGame(playerTransform.position, questManager);
            Debug.Log("Game Saved!");
        }
        else
        {
            Debug.LogWarning("Player Transform not set in PauseMenuManager!");
        }
    }

    public void OnLoadSavePressed()
{
    ResetAllPanels();
    loadSavePanel.SetActive(true);
    SetSelected(firstLoadSaveButton);

    if (SaveSystem.SaveFileExists(false))
    {
        var data = SaveSystem.LoadGame(questManager, false);
        manualSaveInfoText.text = $"{data.sceneName}\nTime: {data.saveTime}";
    }
    else
    {
        manualSaveInfoText.text = "No manual save found.";
    }

    if (SaveSystem.SaveFileExists(true))
    {
        var data = SaveSystem.LoadGame(questManager, true);
        autoSaveInfoText.text = $"{data.sceneName}\nTime: {data.saveTime}";
    }
    else
    {
        autoSaveInfoText.text = "No autosave found.";
    }
}

    public void OnBackFromLoadSave()
    {
        ResetAllPanels();
        pauseMenuUI.SetActive(true);
        SetSelected(firstPauseMenuButton);
    }

    public void OnManualSaveSelected()
    {
        if (SaveSystem.SaveFileExists(false))
        {
            isAutoSaveSelected = false;
            SaveLoadContext.LoadAutoSave = false;
            ResetAllPanels();
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
            ResetAllPanels();
            confirmLoadPanel.SetActive(true);
            SetSelected(firstConfirmLoadButton);
        }
    }

    public void OnConfirmLoadPressed()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        ResetAllPanels();

        SaveData data = SaveSystem.LoadGame(questManager, false);
        if (data != null)
            _canvasSceneTransition.ChangeScene(data.sceneName);
            //SceneManager.LoadScene(data.sceneName);
    }

    public void OnCancelLoadPressed()
    {
        ResetAllPanels();
        loadSavePanel.SetActive(true);
        SetSelected(firstLoadSaveButton);
    }
}