using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;
using UnityEngine.UI;

public class BubbleDialogueUI : Singleton<BubbleDialogueUI>
{
    public DialogueRunner dialogueRunner;
    public DialogueUI dialogueUI;

    public bool canContinue;

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
