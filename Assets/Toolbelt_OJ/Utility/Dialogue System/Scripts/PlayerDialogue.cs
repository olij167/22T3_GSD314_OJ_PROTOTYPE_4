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

    [HideInInspector] public NPCDialogueOption questions;

    [Serializable]
    public class PlayerQuestions
    {
        public NPCInfo npc;
        public List<PlayerDialogueOption> questionsForNPC;
    }

    public List<PlayerQuestions> playerQuestions;


    //Set the questions the player can ask the npc for specified NPC Dialogue Option
    public NPCDialogueOption SetPlayerQuestionsForNPC(NPCInfo npc, NPCDialogueOption npcDialogue)
    {
        //create a new dialogue option to hold all the questions the player can ask the npc
        questions = new NPCDialogueOption();

        questions.dialogue = npcDialogue.dialogue;

        //enable the player to choose a dialogue option
        questions.requiresResponse = true;

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
            playerQuestions[i].questionsForNPC = new List<PlayerDialogueOption>();

            for (int d = 0; d < playerQuestions[i].npc.npcDialogue.dialogueConnections.Count; d++)
            {
                playerQuestions[i].questionsForNPC.Add(playerQuestions[i].npc.npcDialogue.dialogueConnections[d].playerDialogueInput);
            }
        }
    }

}
