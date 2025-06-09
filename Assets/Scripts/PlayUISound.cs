using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayUISound : MonoBehaviour
{
    public void playUISound()
    {
        SoundManager.PlaySound(SoundType.UI, 1f);
    }

    public void PlayTakeOff()
    {
        SoundManager.PlaySound(SoundType.SPACESHIPTAKEOFF, 0.2f);
    }
}
