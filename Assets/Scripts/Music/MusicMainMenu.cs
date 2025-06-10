using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.PlaySoundOnLoop(SoundType.MAINMENU, 0.1f);
    }
}
