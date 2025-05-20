using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    WALK,
    FALL,
    PULSE,
    HOVER,
    REPAIR,
    UNREPAIRABLE,
    REWARD,
    AMBIENT,
    TOOLBOX,
    UI
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] 
    private AudioClip[] soundlist;

    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        //sound manager always has an audio source on it
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundlist[(int)sound], volume);
    }
}
