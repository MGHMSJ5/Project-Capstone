using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMainScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.PlaySoundOnLoop(SoundType.MAINSCENE, 0.1f);
    }
}
