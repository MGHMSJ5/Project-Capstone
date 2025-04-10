using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject confirmNewGamePanel;
    public GameObject noSaveFoundPanel;

    public void OnNewGamePressed()
    {
        if (SaveSystem.SaveFileExists())
        {
            confirmNewGamePanel.SetActive(true); // Shows "Are you sure?" confirmation
        }
        else
        {
            StartNewGame();
        }
    }

    public void ConfirmStartNewGame()
    {
        SaveSystem.DeleteSave(); // Deletes old save if chosen (and present!).
        StartNewGame();
    }

    private void StartNewGame()
    {
        SceneManager.LoadScene("Gravity");
    }

    public void OnLoadGamePressed()
{
    if (SaveSystem.SaveFileExists())
    {
        SaveData data = SaveSystem.LoadGame();
        if (data != null)
        {
            SceneManager.LoadScene(data.sceneName);
        }
    }
    else
    {
        noSaveFoundPanel.SetActive(true);
    }
}
}