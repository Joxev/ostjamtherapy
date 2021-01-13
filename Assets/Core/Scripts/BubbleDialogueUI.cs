using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Yarn;
using Yarn.Unity;
using UnityEngine.UI;
using TMPro;

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
    public GameObject text;

    public bool canContinue;

    Dictionary<string, SpeakerData> speakerDatabase = new Dictionary<string, SpeakerData>();

    public List<SpeakerData> speakers = new List<SpeakerData>();

    bool autoAdvance = false;
    int advancesLeft = 0;

    [Header("Tags")]
    public ObjectShake textShake;
    public float autoAdvanceTime = 1.5f;



    private void Awake()
    {
        base.Awake();
        foreach(SpeakerData s in speakers)
        {
            speakerDatabase.Add(s.speakerName, s);
        }
        dialogueRunner.AddCommandHandler("SetSpeaker", SetSpeakerInfo);
        dialogueRunner.AddCommandHandler("SetPortrait", SetSpeakerPortrait);
        dialogueRunner.AddCommandHandler("SetTags", SetDialogueTags);
        dialogueRunner.AddCommandHandler("AutoAdvance", SetAutoAdvance);
    }

    private void Start()
    {
        canContinue = true;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(canContinue && !autoAdvance)
            {
                dialogueUI.MarkLineComplete();
            }
        }
    }

    //
    public void OnLineFinishDisplaying()
    {
        if(autoAdvance && advancesLeft > 0)
        {
            StartCoroutine(autoAdvanceCo());
        }
        else if(autoAdvance)
        {
            autoAdvance = false;
        }
    }

    //

    public void SetSpeakerPortrait(string[] info)
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
                    print(physicalSpeaker.speakerData.name);
                }
            }
            physicalSpeaker.speaker.GetComponent<SpriteRenderer>().sprite = data.GetEmotionSprite(emotion);
        }
    }

    public void SetSpeakerInfo(string[] info)
    {
        string speaker = info[0];
        string emotion = info[1];

        if (speakerDatabase.TryGetValue(speaker, out SpeakerData data))
        {
            PhysicalSpeaker physicalSpeaker = new PhysicalSpeaker();
            foreach (PhysicalSpeaker p in physicalSpeakers)
            {
                if (p.speakerData == data)
                {
                    physicalSpeaker = p;
                    print(physicalSpeaker.speakerData.name);
                }
            }
            physicalSpeaker.speaker.GetComponent<SpriteRenderer>().sprite = data.GetEmotionSprite(emotion);
            text.transform.position = Camera.main.WorldToScreenPoint(physicalSpeaker.speaker.GetComponent<Character>().textPoint.position);
        }
    }

    public void SetDialogueTags(string[] info)
    {
        foreach(string s in info)
        {
            switch(s)
            {
                case "Shake":
                    textShake.Shake();
                    break;
                case "TempShake":
                    textShake.TempShake();
                    break;
                case "NoType":
                    dialogueUI.textSpeed = 0;
                    break;
                case "Red":
                    text.GetComponent<TextMeshProUGUI>().color = new Color32(214, 32, 15, 255);
                    print(text.GetComponent<TextMeshProUGUI>().color);
                    print("red");
                    break;
                case "Bold":
                    text.GetComponent<Animator>().SetBool("bold", true);
                    break;

            }
        }
    }

    public void SetAutoAdvance(string[] info)
    {
        int result;
        if(int.TryParse(info[0], out result))
        {
            autoAdvance = true;
            advancesLeft = result;
        }
    }

    private IEnumerator autoAdvanceCo()
    {
        yield return new WaitForSeconds(autoAdvanceTime);
        dialogueUI.MarkLineComplete();
        advancesLeft--;
    }

    public void StopDialogTags()
    {
        textShake.StopShake();
        dialogueUI.textSpeed = .025f;
        text.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        text.GetComponent<Animator>().SetBool("bold", false);
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
