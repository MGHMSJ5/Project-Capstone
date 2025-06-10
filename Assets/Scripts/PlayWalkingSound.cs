using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWalkingSound : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlaySound()
    {
        SoundManager.PlaySound(SoundType.WALK, 1f);
    }
}
