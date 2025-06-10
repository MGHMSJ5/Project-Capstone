using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCredits : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.PlaySoundOnLoop(SoundType.CREDITS, 0.1f);
    }
}
