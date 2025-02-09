﻿using System.Collections;
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

    public PhysicalSpeaker currentSpeaker;

    public DialogueRunner dialogueRunner;
    public DialogueUI dialogueUI;

    public GameObject text;
    public GameObject textParent;

    public Transform optionsPoint;
    public Transform options;

    public bool canContinue;

    Dictionary<string, SpeakerData> speakerDatabase = new Dictionary<string, SpeakerData>();

    public List<SpeakerData> speakers = new List<SpeakerData>();

    bool autoAdvance = false;
    int advancesLeft = 0;
    bool noType;

    [Header("Tags")]
    public ObjectShake textShake;
    public float autoAdvanceTime = 1.5f;

    private Coroutine stopTalk;

    float defaultCameraSize;
    Vector3 defaultCameraPosition;


    private void Awake()
    {
        base.Awake();
        foreach(SpeakerData s in speakers)
        {
            speakerDatabase.Add(s.speakerName, s);
        }
        dialogueRunner.AddCommandHandler("SetSpeaker", SetSpeakerInfo);
        dialogueRunner.AddCommandHandler("SetPortrait", SetSpeakerPortrait);
        //dialogueRunner.AddCommandHandler("SetTags", SetDialogueTags);
        dialogueRunner.AddCommandHandler("AutoAdvance", SetAutoAdvance);
        dialogueRunner.AddCommandHandler("TargetZoom", TargetZoom);
    }

    private void Start()
    {
        defaultCameraSize = Camera.main.orthographicSize;
        defaultCameraPosition = Camera.main.transform.position;

        canContinue = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canContinue && !autoAdvance)
            {
                dialogueUI.MarkLineComplete();
            }
        }

        textParent.transform.position = Camera.main.WorldToScreenPoint(currentSpeaker.speaker.GetComponent<Character>().textPoint.position);
        options.transform.position = Camera.main.WorldToScreenPoint(optionsPoint.position);
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
        foreach(PhysicalSpeaker p in physicalSpeakers)
        {
            if (!noType)
            {
                p.speaker.GetComponent<Animator>().SetBool("talking", false);
            }
        }
    }

    //
    public void TargetZoom(string[] info)
    {
        if(!info[0].Equals("Reset"))
        {
            string target = info[0];
            int zoom = Int32.Parse(info[1]);

            PhysicalSpeaker targetSpeaker = new PhysicalSpeaker();

            if (speakerDatabase.TryGetValue(target, out SpeakerData data))
            {
                foreach (PhysicalSpeaker p in physicalSpeakers)
                {
                    if (p.speakerData == data)
                    {
                        targetSpeaker = p;
                    }
                }
            }

            SetFocus(zoom, targetSpeaker.speaker.GetComponent<Character>().focusPoint.position);
        }
        else
        {
            SetFocus(defaultCameraSize, defaultCameraPosition);
            //virtualCamera.transform.position = new Vector3(0, 0, virtualCamera.transform.position.z);
        }
    }

    Coroutine zoomSize;
    float desiredSize;

    Vector3 cameraDesiredPosition;
    public void SetFocus(float size, Vector3 position)
    {
        float preSize = defaultCameraSize;
        desiredSize = size;

        Vector3 prePos = transform.position;
        cameraDesiredPosition = position;

        zoomSize = StartCoroutine(zoomS(0.35f));
    }
    IEnumerator zoomS(float time)
    {
        float elapsedTime = 0;
        float startSize = Camera.main.orthographicSize;
        Vector3 startPos = Camera.main.transform.position;

        while(Camera.main.orthographicSize != desiredSize && Camera.main.transform.position != cameraDesiredPosition)
        {
            Camera.main.transform.position = Vector3.Lerp(startPos, new Vector3( cameraDesiredPosition.x , cameraDesiredPosition.y, startPos.z), elapsedTime / time);
            Camera.main.orthographicSize = Mathf.Lerp(startSize, desiredSize, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

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
                }
            }
            physicalSpeaker.speaker.GetComponent<SpriteRenderer>().sprite = data.GetEmotionSprite(emotion);
        }
    }

    public void SetSpeakerInfo(string[] info)
    {
        string speaker = info[0];
        string emotion = info[1];

        currentSpeaker = new PhysicalSpeaker();

        if (speakerDatabase.TryGetValue(speaker, out SpeakerData data))
        {
            foreach (PhysicalSpeaker p in physicalSpeakers)
            {
                if (p.speakerData == data)
                {
                    currentSpeaker = p;
                }
            }
            currentSpeaker.speaker.GetComponent<SpriteRenderer>().sprite = data.GetEmotionSprite(emotion);
            currentSpeaker.speaker.GetComponent<Animator>().SetBool("talking", true);
            textParent.transform.position = Camera.main.WorldToScreenPoint(currentSpeaker.speaker.GetComponent<Character>().textPoint.position);

            if(currentSpeaker.speaker.GetComponent<Patient>())
            {
                if ((currentSpeaker.speaker.GetComponent<Patient>().currentAnger < 25) && currentSpeaker.speaker.GetComponent<Patient>().currentSadness < 25)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Game/Speech/Positive");
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Game/Speech/Negative");
                }
            }
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Game/Speech/Neutral");
            }
        }

        int index = 0;
        foreach (string s in info)
        {
            if (index > 1)
            {
                switch (s)
                {
                    case "Shake":
                        textShake.Shake();
                        break;
                    case "TempShake":
                        textShake.TempShake();
                        break;
                    case "NoType":
                        noType = true;
                        dialogueUI.textSpeed = 0;
                        break;
                    case "Red":
                        text.GetComponent<TextMeshProUGUI>().color = new Color32(214, 32, 15, 255);
                        break;
                    case "Bold":
                        text.GetComponent<Animator>().SetBool("bold", true);
                        break;
                    case "Small":
                        textParent.transform.localScale = new Vector3(.7f, .7f, 1f);
                        break;
                    case "Big":
                        textParent.transform.localScale = new Vector3(1.4f, 1.4f, 1f);
                        break;
                    case "Huge":
                        textParent.transform.localScale = new Vector3(1.8f, 1.8f, 1f);
                        break;

                }
            }
            index++;
        }
        if (noType)
        {
            stopTalk = StartCoroutine(StopNoTypeTalking(currentSpeaker));
        }
    }

    IEnumerator StopNoTypeTalking(PhysicalSpeaker p)
    {
        yield return new WaitForSeconds(1.5f);
        p.speaker.GetComponent<Animator>().SetBool("talking", false);
    }

    /*public void SetDialogueTags(string[] info)
    {
        foreach(string s in info)
        {
            switch (s)
            {
                case "Shake":
                    textShake.Shake();
                    break;
                case "TempShake":
                    textShake.TempShake();
                    break;
                case "NoType":
                    noType = true;
                    dialogueUI.textSpeed = 0;
                    break;
                case "Red":
                    text.GetComponent<TextMeshProUGUI>().color = new Color32(214, 32, 15, 255);
                    break;
                case "Bold":
                    text.GetComponent<Animator>().SetBool("bold", true);
                    break;
                case "Small":
                    textParent.transform.localScale = new Vector3(.7f, .7f, 1f);
                    break;
                case "Big":
                    textParent.transform.localScale = new Vector3(1.4f, 1.4f, 1f);
                    break;
                case "Huge":
                    textParent.transform.localScale = new Vector3(1.8f, 1.8f, 1f);
                    break;

            }
        }
    }*/

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
        if(stopTalk != null) { StopCoroutine(stopTalk); }
        noType = false;
        textShake.StopShake();
        dialogueUI.textSpeed = .025f;
        text.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        text.GetComponent<Animator>().SetBool("bold", false);
        textParent.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

    }

    #region Choice Management
    public void InitiateChoice()
    {
        canContinue = false;
        foreach (PhysicalSpeaker p in physicalSpeakers)
        {
            p.speaker.GetComponent<Animator>().SetBool("talking", false);
        }
    }
    public void StopInitiateChoice()
    {
        canContinue = true;
    }
    #endregion
}
