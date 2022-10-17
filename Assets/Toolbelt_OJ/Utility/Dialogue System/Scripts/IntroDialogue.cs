using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroDialogue : MonoBehaviour
{
    public StartDialogue startDialogue;
    public NPCInfo tutorialVoice;
    public NPCDialogueOption introDialogue;

    public void Start()
    {
        startDialogue.NPCInitiatedDialogue(tutorialVoice, introDialogue);
    }
}
