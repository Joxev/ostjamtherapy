using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialog/Speaker")]
public class SpeakerData : ScriptableObject
{
    public string speakerName;

    [System.Serializable]
    public struct EmotionState
    {
        public string name;
        public Sprite icon;
    }

    public List<EmotionState> emotions = new List<EmotionState>();

    public Sprite GetEmotionSprite(string emotion)
    {
        foreach(EmotionState e in emotions)
        {
            if(e.name == emotion)
            {
                return e.icon;
            }
        }
        return null;
    }
}
