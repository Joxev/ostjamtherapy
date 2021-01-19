using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;
using FMOD.Studio;

public class SoundManager : Singleton<SoundManager>
{
    public string GameplayEvent;
    public FMOD.Studio.EventInstance GameplaySound;

    public float volume = 1.0f;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        GameplaySound = FMODUnity.RuntimeManager.CreateInstance(GameplayEvent);
        GameplaySound.start();
    }
}
