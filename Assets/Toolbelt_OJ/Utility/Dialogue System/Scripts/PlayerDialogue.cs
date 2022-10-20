using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerDialogue : MonoBehaviour
{
    public List<PlayerDialogueOption> greetingDialogue;

    public List<PlayerDialogueOption> goodbyeDialogue;

    public List<PlayerDialogueOption> changeTopicDialogue;

    public PlayerDialogueOption viewMoreDialogue;

    public PlayerDialogueOption continueDialogue;

    public NPCDialogueOption questions;

    [Serializable]
    public class PlayerQuestions
    {
        public NPCInfo npc;
        public List<PlayerDialogueOption> questionsForNPC;
    }


    // npc & respective dialogue options set at runtime
    public List<PlayerQuestions> playerQuestions;


    //Set the questions the player can ask the npc for specified NPC Dialogue Option
    public NPCDialogueOption SetPlayerDialogueBasedOnCurrentNPCAndDialogue(NPCInfo npc, NPCDialogueOption npcDialogue)
    {
        //create a new dialogue option to hold all the questions the player can ask the npc
        questions = ScriptableObject.CreateInstance<NPCDialogueOption>();

        questions.dialogue = npcDialogue.dialogue;

        //enable the player to choose a dialogue option
        //questions.requiresResponse = true;

        if (playerQuestions.Count > 0)
        {

            for (int i = 0; i < playerQuestions.Count; i++)
            {
                //Set appropriate dialogue options
                if (playerQuestions[i].npc == npc)
                {
                    questions.playerResponses = playerQuestions[i].questionsForNPC;
                }
            }
        }

        return questions;
    }

    //Add new dialogue option to playerQuestions for all npc's
    public void AddQuestionForAllNPCs(PlayerDialogueOption newDialogueOption)
    {
        foreach (PlayerQuestions question in playerQuestions)
        {
            question.questionsForNPC.Add(newDialogueOption);
        }
    }

    //Add new dialogue option to playerQuestions for specific npc
    public void AddQuestionForSpecificNPC(PlayerDialogueOption newDialogueOption, NPCInfo npc)
    {
        for (int i = 0; i < playerQuestions.Count; i++)
        {
            if (playerQuestions[i].npc == npc)
            {
                playerQuestions[i].questionsForNPC.Add(newDialogueOption);
            }
        }
    }

    public void AddDialogueOptions()
    {
        for (int i = 0; i < playerQuestions.Count; i++)
        {
            if (playerQuestions[i].npc == FindObjectOfType<NEWListDialogueSystem>().npc) // check that you are only affecting the current npc
            {
                playerQuestions[i].questionsForNPC = new List<PlayerDialogueOption>(); // reset their dialogue options

                for (int d = 0; d < playerQuestions[i].npc.npcDialogue.dialogueConnections.Count; d++) //for each npc dialogue object of each npc
                {
                    if (!playerQuestions[i].npc.npcDialogue.dialogueConnections[d].playerDialogueInput.isResponseToNPCDialogue && !playerQuestions[i].npc.npcDialogue.dialogueConnections[d].playerDialogueInput.isLocked) // check the dialogue object if the player is responding
                    {
                        playerQuestions[i].questionsForNPC.Add(playerQuestions[i].npc.npcDialogue.dialogueConnections[d].playerDialogueInput); //if not add it to the player's current dialogue selection
                    }
                    else
                    {
                        playerQuestions[i].questionsForNPC.Remove(playerQuestions[i].npc.npcDialogue.dialogueConnections[d].playerDialogueInput); //if they aren't remove it
                    }

                    //Debug.Log("Dialogue Option '" + playerQuestions[i].npc.npcDialogue.dialogueConnections[d].playerDialogueInput.name + "' has been added to questions for '" + playerQuestions[i].npc.name + "'");
                }
            }
        }
    }

    //public void AddWorldDialogueOptions()
    //{
    //    for (int i = 0; i < playerQuestions.Count; i++)
    //    {
    //        foreach (DialogueInWorld dialogue in FindObjectsOfType<DialogueInWorld>())
    //        {
    //            if (playerQuestions[i].npc == dialogue.narrator)
    //            {
    //                playerQuestions[i].questionsForNPC.Add(dialogue.dialogueFromWorld);
    //            }
    //        }
    //    }
    //}

    public void AddResponseOptions()
    {
        for (int i = 0; i < playerQuestions.Count; i++)
        {

            if (playerQuestions[i].npc == FindObjectOfType<NEWListDialogueSystem>().npc) // check that you are only affecting the current npc
            {
                playerQuestions[i].questionsForNPC = new List<PlayerDialogueOption>(); // reset their dialogue options

                for (int d = 0; d < playerQuestions[i].npc.npcDialogue.dialogueConnections.Count; d++) //for each npc dialogue object of each npc
                {
                    if (playerQuestions[i].npc.npcDialogue.dialogueConnections[d].playerDialogueInput.isResponseToNPCDialogue && !playerQuestions[i].npc.npcDialogue.dialogueConnections[d].playerDialogueInput.isLocked) // check the dialogue object if the player is responding
                    {
                        if (FindObjectOfType<NEWListDialogueSystem>().npcDialogue.playerResponses.Contains(playerQuestions[i].npc.npcDialogue.dialogueConnections[d].playerDialogueInput)) //if they are responding check what dialogue it is a response to
                        {
                            playerQuestions[i].questionsForNPC.Add(playerQuestions[i].npc.npcDialogue.dialogueConnections[d].playerDialogueInput); // if it's the current dialogue add it to the player's current dialogue selection
                        }
                        else
                        {
                            playerQuestions[i].questionsForNPC.Remove(playerQuestions[i].npc.npcDialogue.dialogueConnections[d].playerDialogueInput);
                        }
                    }

                    //Debug.Log("Dialogue Option '" + playerQuestions[i].npc.npcDialogue.dialogueConnections[d].playerDialogueInput.name + "' has been added to questions for '" + playerQuestions[i].npc.name + "'");
                }
            }
        }
    }

}
