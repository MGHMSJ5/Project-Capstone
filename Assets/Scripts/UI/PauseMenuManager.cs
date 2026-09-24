using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Main UI")]
    public GameObject pauseMenuUI;
    public GameObject firstPauseMenuButton;
    public GameObject dialoguePanel;

    public static bool IsPaused { get; private set; } = false;

    private bool isPaused = false;
    private string lastInputMethod = "Controller";

    private CanvasSceneTransition _canvasSceneTransition;

    private void Awake()
    {
        _canvasSceneTransition =
            GameObject.Find("Canvas_SceneTransition")
            .GetComponent<CanvasSceneTransition>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (dialoguePanel != null && dialoguePanel.activeSelf)
                return;

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

        SoundManager.PlaySound(SoundType.UI, 1f);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        IsPaused = false;

        SoundManager.PlaySound(SoundType.UI, 1f);
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;

        SoundManager.PlaySound(SoundType.UI, 1f);
        _canvasSceneTransition.ChangeScene("TitleScreen");
    }
}