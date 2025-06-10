using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SoundType
{
    //All soundtypes can be found here. New soundtypes that are added in the future, should also be added here.
    WALK,
    FALL,
    PULSE,
    HOVER,
    REPAIR,
    UNREPAIRABLE,
    SPACESHIPLAND,
    SPACESHIPTAKEOFF,
    ENGINE,
    TOOLBOX,
    UI,
    MAINMENU,
    INTROSCENE,
    MAINSCENE,
    CREDITS
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] 
    private SoundList[] soundlist;

    private AudioSource audioSource;
    public bool isMusicPlaying = false;

    private void Start()
    {
        //sound manager always has an audio source on it
        audioSource = GetComponent<AudioSource>();
    }

    //This function can be used in other scripts, which randomly plays on of the sounds found in that specific soundtype.
    public static void PlaySound(SoundType sound, float volume)
    {
        AudioClip[] clips = Instance.soundlist[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        Instance.audioSource.PlayOneShot(randomClip, volume);
    }

    public static void PlaySoundOnLoop(SoundType sound, float volume)
    {
        //Plays a sound on loop
        AudioClip[] clips = Instance.soundlist[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        Instance.audioSource.clip = randomClip;
        Instance.audioSource.loop = true;
        Instance.audioSource.volume = volume;
        Instance.audioSource.Play();
    }

    public static void StopSound()
    {
        //Stops the sound that is currently playing
        Instance.audioSource.Stop();
    }
}

//This makes organization possible. This makes it possible for us to name the assets in the list that can be found in the spectator. 
[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }

    [SerializeField]
    private string name;
    [SerializeField]
    private AudioClip[] sounds;
}
