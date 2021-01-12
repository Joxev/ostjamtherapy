using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] string yarnStartNode = "Start";
    [SerializeField] YarnProgram yarnDialog;

    private void Start()
    {
        StartDialogue();
    }

    public void StartDialogue()
    {
        BubbleDialogueUI.instance.dialogueRunner.Add(yarnDialog);
        BubbleDialogueUI.instance.dialogueRunner.StartDialogue(yarnStartNode);
    }
}
