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

    //This function can be used in other scripts, which randomly plays on of the sounds found in that specific soundtype.
    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundlist[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume);
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
