using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn;
using System;

public class Patient : MonoBehaviour
{

    public string patientName;

    [Header("Dialogs")]
    [SerializeField] YarnProgram yarnDialog;

    [SerializeField] string yarnStartNode = "Start";
    [SerializeField] string angerStartNode;
    [SerializeField] string sadnessStartNode;
    [SerializeField] string suicideStartNode;
    [SerializeField] string playerDeathStartNode;

    public string pauseNode;


    [Header("Custom Values")]

    [Range(0, 100)]
    public int StartingAnger;
    [Range(0, 100)]
    public int StartingSadness;

    public Animator anim;

    int currentSadness;
    int currentAnger;

    public void setCurrentSadness(int value)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Sadness", value);
        currentSadness = value;
        updateAnimations();
        BubbleDialogueUI.instance.dialogueRunner.GetComponent<InMemoryVariableStorage>().SetValue("sadness", value);
    }
    public void setCurrentAnger(int value)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Anger", value);
        currentAnger = value;
        updateAnimations();
        BubbleDialogueUI.instance.dialogueRunner.GetComponent<InMemoryVariableStorage>().SetValue("anger", value);
    }

    bool enabledGate = false;
    bool breakingPointGate = false;

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        BubbleDialogueUI.instance.dialogueRunner.RemoveCommandHandler("CheckEmotionLevels");
        BubbleDialogueUI.instance.dialogueRunner.RemoveCommandHandler("ResumeDialog");
        BubbleDialogueUI.instance.dialogueRunner.RemoveCommandHandler("ChangeEmotionValues");
    }
    private void Start()
    {
        BubbleDialogueUI.instance.dialogueRunner.AddCommandHandler("CheckEmotionLevels", CheckEmotionLevels);
        BubbleDialogueUI.instance.dialogueRunner.AddCommandHandler("ResumeDialog", ResumeDialog);
        BubbleDialogueUI.instance.dialogueRunner.AddCommandHandler("ChangeEmotionValues", ChangeEmotionValues);
    }

    public void initalizePatient()
    {
        if (!enabledGate)
        {
            enabledGate = true;
            setCurrentAnger(StartingAnger);
            setCurrentSadness(StartingSadness);

            StartDialogue();
        }
    }

    public void ChangeEmotionValues(string[] info)
    {
        switch(info[0])
        {
            case "Anger":
                setCurrentAnger(currentAnger + Int32.Parse(info[1]));
                break;
            case "Sadness":
                setCurrentSadness(currentSadness + Int32.Parse(info[1]));
                break;
        }
    }

    public void StartDialogue()
    {
        BubbleDialogueUI.instance.dialogueRunner.Add(yarnDialog);
        BubbleDialogueUI.instance.dialogueRunner.StartDialogue(patientName + "." + yarnStartNode);
    }
    public void updateAnimations()
    {
        anim.SetBool("happy", false);
        anim.SetBool("neutral", false);
        anim.SetBool("sad", false);
        anim.SetBool("angry", false);

        if ((currentAnger < 50) && (currentSadness < 25)) // Happy
        {
            anim.SetBool("happy", true);
        }
        else if ((currentAnger < 50) && currentSadness < 50)
        {
            anim.SetBool("neutral", true);
        }
        else if ((currentAnger >= 50) || currentSadness >= 50)
        {
            if(currentSadness > currentAnger)
            {
                anim.SetBool("sad", true);
            }
            else
            {
                anim.SetBool("angry", true);
            }
        }
    }

    public void CheckEmotionLevels(string[] info)
    {
        if (currentAnger >= 100 || currentSadness >= 100)
        {
            if (!breakingPointGate)
            {
                breakingPointGate = true;
                pauseNode = BubbleDialogueUI.instance.dialogueRunner.CurrentNodeName;
                if (currentSadness > currentAnger)
                {
                    BubbleDialogueUI.instance.dialogueRunner.StartDialogue(patientName + "." + sadnessStartNode);
                }
                else
                {
                    BubbleDialogueUI.instance.dialogueRunner.StartDialogue(patientName + "." + angerStartNode);
                }

            }
            else
            {
                if (currentSadness > currentAnger)
                {
                    BubbleDialogueUI.instance.dialogueRunner.StartDialogue(patientName + "." + suicideStartNode);
                }
                else
                {
                    BubbleDialogueUI.instance.dialogueRunner.StartDialogue(patientName + "." + playerDeathStartNode);
                }
            }
        }
    }

    public void ResumeDialog(string[] info)
    {
        BubbleDialogueUI.instance.dialogueRunner.StartDialogue(pauseNode);
    }
}