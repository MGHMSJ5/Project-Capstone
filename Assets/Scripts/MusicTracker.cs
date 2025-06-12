using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicTracker : MonoBehaviour
{
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "TitleScreen")
        {
            SoundManager.PlaySoundOnLoop(SoundType.MAINMENU, 0.2f);
        }
        else if (currentScene.name == "IntroScene")
        {

        }
        else if (currentScene.name == "Kettler22T")
        {
            SoundManager.PlaySoundOnLoop(SoundType.MAINSCENE, 0.2f);
        }
        else if (currentScene.name == "Credits")
        {
            SoundManager.PlaySoundOnLoop(SoundType.CREDITS, 0.2f);
        }
    }
}
