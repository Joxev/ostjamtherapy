using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Yarn;
using Yarn.Unity;
using UnityEngine.UI;

public class BubbleDialogueUI : Singleton<BubbleDialogueUI>
{
    [System.Serializable]
    public struct PhysicalSpeaker
    {
        public GameObject speaker;
        public SpeakerData speakerData;

        public PhysicalSpeaker(GameObject _speaker, SpeakerData _speakerData)
        {
            speaker = _speaker;
            speakerData = _speakerData;
        }
    }
    public List<PhysicalSpeaker> physicalSpeakers = new List<PhysicalSpeaker>();

    public DialogueRunner dialogueRunner;
    public DialogueUI dialogueUI;

    public bool canContinue;

    Dictionary<string, SpeakerData> speakerDatabase = new Dictionary<string, SpeakerData>();

    public List<SpeakerData> speakers = new List<SpeakerData>();

    private void Awake()
    {
        base.Awake();
        foreach(SpeakerData s in speakers)
        {
            speakerDatabase.Add(s.speakerName, s);
        }
        dialogueRunner.AddCommandHandler("SetSpeaker", SetSpeakerInfo);
    }

    private void Start()
    {
        canContinue = true;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(canContinue)
            {
                dialogueUI.MarkLineComplete();
            }
        }
    }

    public void SetSpeakerInfo(string[] info)
    {
        string speaker = info[0];
        string emotion = info[1];

        if(speakerDatabase.TryGetValue(speaker, out SpeakerData data))
        {
            PhysicalSpeaker physicalSpeaker = new PhysicalSpeaker();
            foreach(PhysicalSpeaker p in physicalSpeakers)
            {
                if(p.speakerData == data)
                {
                    physicalSpeaker = p;
                }
            }
            physicalSpeaker.speaker.GetComponent<SpriteRenderer>().sprite = data.GetEmotionSprite(emotion);
        }
    }

    #region Choice Management
    public void InitiateChoice()
    {
        canContinue = false;
    }
    public void StopInitiateChoice()
    {
        canContinue = true;
    }
    #endregion
}
