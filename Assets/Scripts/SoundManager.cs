using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] 
    private SoundList[] soundlist;

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
        AudioClip[] clips = instance.soundlist[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume);
    }
}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }

    [SerializeField]
    private string name;
    [SerializeField]
    private AudioClip[] sounds;
}
