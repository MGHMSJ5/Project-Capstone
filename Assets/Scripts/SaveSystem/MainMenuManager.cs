using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;
    public GameObject quitConfirmPanel;

    [Header("First Selected Buttons")]
    public GameObject firstCreditsButton;
    public GameObject firstQuitConfirmButton;
    public GameObject firstMainMenuButton;

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
        if (creditsPanel.activeSelf)
        {
            SetSelected(firstCreditsButton);
        }
        else if (quitConfirmPanel.activeSelf)
        {
            SetSelected(firstQuitConfirmButton);
        }
        else
        {
            SetSelected(firstMainMenuButton);
        }
    }

    private void SetSelected(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(obj);
    }

    private void ResetAllPanels()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(false);
        quitConfirmPanel.SetActive(false);
    }

    // =========================
    // NEW GAME
    // =========================

    public void OnNewGamePressed()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        // Reset runtime gameplay resources for a fresh game.
        RepairResources.ResetRepairResources();

        SoundManager.PlaySound(SoundType.UI, 1f);

        int nextSceneIndex =
            SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            string scenePath =
                SceneUtility.GetScenePathByBuildIndex(nextSceneIndex);

            string sceneName =
                System.IO.Path.GetFileNameWithoutExtension(scenePath);

            _canvasSceneTransition.ChangeScene(sceneName);
        }
        else
        {
            Debug.LogWarning("No scene found in build settings!");
        }
    }

    // =========================
    // CREDITS
    // =========================

    public void OnCreditsPressed()
    {
        ResetAllPanels();

        SoundManager.PlaySound(SoundType.UI, 1f);

        creditsPanel.SetActive(true);
        SetSelected(firstCreditsButton);
    }

    public void OnCloseCreditsPressed()
    {
        ResetAllPanels();

        SoundManager.PlaySound(SoundType.UI, 1f);

        mainMenuPanel.SetActive(true);
        SetSelected(firstMainMenuButton);
    }

    // =========================
    // QUIT
    // =========================

    public void OnQuitPressed()
    {
        ResetAllPanels();

        SoundManager.PlaySound(SoundType.UI, 1f);

        quitConfirmPanel.SetActive(true);
        SetSelected(firstQuitConfirmButton);
    }

    public void OnConfirmQuitPressed()
    {
        SoundManager.PlaySound(SoundType.UI, 1f);

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnCancelQuitPressed()
    {
        ResetAllPanels();

        SoundManager.PlaySound(SoundType.UI, 1f);

        mainMenuPanel.SetActive(true);
        SetSelected(firstMainMenuButton);
    }
}