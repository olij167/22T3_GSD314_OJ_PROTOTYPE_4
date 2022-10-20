using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "DialogueSystem/ConditionalEvent")]
public class ConditionalEvents : MonoBehaviour
{
    public UnityEvent conditionalEvent;

[Header("Variables for Specific Events")]
    //public PlayerDialogueOption dialogue;
    [HideInInspector] public NPCInfo npc;
    

    public NPCInfo SetNPC(NPCInfo npcToSet)
    {
        return npc = npcToSet;
    }

    public void ConditionalEventTest()
    {
        Debug.Log("Conditional Event Works!");
    }

    public void AddDialogueForAllNPCs(PlayerDialogueOption dialogue)
    {
        PlayerDialogue playerDialogue = FindObjectOfType<PlayerDialogue>();

        playerDialogue.AddQuestionForAllNPCs(dialogue);

        if (!dialogue.isResponseToNPCDialogue)
        {
            playerDialogue.AddDialogueOptions();
        }
        else
        {
            playerDialogue.AddResponseOptions();
        }

    }

    public void AddDialogueForSpecificNPC(PlayerDialogueOption dialogue)
    {
        PlayerDialogue playerDialogue = FindObjectOfType<PlayerDialogue>();

        playerDialogue.AddQuestionForSpecificNPC(dialogue, npc);

        if (dialogue.isLocked)
        {
            dialogue.isLocked = false;
        }

        if (!dialogue.isResponseToNPCDialogue)
        {
            playerDialogue.AddDialogueOptions();
        }
        else
        {
            playerDialogue.AddResponseOptions();
        }

    }

    



    // To Make:
    private void MultipleNPCConversation()
    {
        //track multiple npc's in a conversation
            //determine speaking order
            //combine player dialogue options for all npc's
            //apply emotion effects to all npc's
    }

    private void ApplyPlayerStatEffect()
    {
        //get player stats
        //apply stat effects

    }

    private void CheckPlayerStatsForHighest()
    {
        //compare current player stats and return whichever has the highest value
    }

    private void CheckPlayerStatsForLowest()
    {
        //compare current player stats and return whichever has the lowest value
    }

    private void CheckSpecificPlayerStat()
    {
        //check a specified stats value
    }

    private void ChangeCamera()
    {
        //Transition to another position / camera
    }

}

