using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Streetlight : MonoBehaviour
{
    public DayNightSystem dayNightSystem;
    private Renderer renderer;
    private Material[] mats;

    [SerializeField]
    private Material LightOn;
    [SerializeField]
    private Material LightOff;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        mats = renderer.materials;

        dayNightSystem = GameObject.Find("DayNightSystem").GetComponent<DayNightSystem>();
    }

    private void Start()
    {
        if (dayNightSystem.isCurrentlyDay)
        {
            ChangeLightDay();
        }
        else
        {
            ChangeLightNight();
        }
    }

    private void OnEnable()
    {
        dayNightSystem.SetDay += ChangeLightDay;
        dayNightSystem.SetNight += ChangeLightNight;
    }

    private void OnDisable()
    {
        dayNightSystem.SetDay -= ChangeLightDay;
        dayNightSystem.SetNight -= ChangeLightNight;
    }

    private void ChangeLightDay()
    {
        mats[1] = LightOff;
        renderer.materials = mats;
    }

    private void ChangeLightNight()
    {
        mats[1] = LightOn;
        renderer.materials = mats;
    }
}
