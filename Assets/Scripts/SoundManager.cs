using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

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
    private AudioSource backgroundMusic; // Added new audiosource for the background music so that they dont overlap
    public bool isMusicPlaying = false;

    private void Start()
    {
        //sound manager always has an audio source on it
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // Get the background music when a new scene has loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }
        backgroundMusic = null;

        if (scene.name != "LoadingScene")
        {
            backgroundMusic = GameObject.Find("Music").GetComponent<AudioSource>();
        }
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
        Instance.backgroundMusic.clip = randomClip;
        Instance.backgroundMusic.loop = true;
        Instance.backgroundMusic.volume = volume;
        Instance.backgroundMusic.Play();
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
