using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "DialogueSystem/ConditionalEvent")]
public class ConditionalEvent : ScriptableObject
{
    public UnityEvent conditionalEvent;

[Header("Variables for 'AddDialogue events'")]
    public PlayerDialogueOption dialogue;
    public NPCInfo npc;



    //Add conditional events as methods

    public void ConditionalEventTest()
    {
        Debug.Log("Conditional Event Works!");
    }

    public void ApplyPlayerStatEffect()
    {
        //get player stats
        //apply stat effects

    }

    public void CheckPlayerStatsForHighest()
    {
        //compare current player stats and return whichever has the highest value
    } 
    
    public void CheckPlayerStatsForLowest()
    {
        //compare current player stats and return whichever has the lowest value
    }
    
    public void CheckSpecificPlayerStat()
    {
        //check a specified stats value
    }

    public void AddDialogueForAllNPCsBasedOnStats()
    {
        FindObjectOfType<PlayerDialogue>().AddQuestionForAllNPCs(dialogue);
    }

    public void AddDialogueForSpecificNPCBasedOnStats()
    {
        //DialogueSystem dialogueSystem = FindObjectOfType<DialogueSystem>();

        PlayerDialogue playerDialogue = FindObjectOfType<PlayerDialogue>();

        playerDialogue.AddQuestionForSpecificNPC(dialogue, npc);
    }

    public void MultipleNPCConversation()
    {
        //track multiple npc's in a conversation
            //determine speaking order
            //combine player dialogue options for all npc's
            //apply emotion effects to all npc's
    }
}

