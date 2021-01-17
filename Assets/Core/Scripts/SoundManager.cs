using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public string Event;
    public FMOD.Studio.EventInstance GameplaySound;

    private void Start()
    {
        GameplaySound = FMODUnity.RuntimeManager.CreateInstance(Event);
        GameplaySound.start();
    }

}
