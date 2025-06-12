using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicIntroScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.PlaySoundOnLoop(SoundType.INTROSCENE, 0.1f);
    }
}
