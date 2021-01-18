using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public string GameplayEvent;
    public FMOD.Studio.EventInstance GameplaySound;

    private void Start()
    {
        GameplaySound = FMODUnity.RuntimeManager.CreateInstance(GameplayEvent);
        GameplaySound.start();
    }
}
