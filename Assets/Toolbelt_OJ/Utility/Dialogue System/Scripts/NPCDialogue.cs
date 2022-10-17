using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCDialogue
{
    public List<NPCDialogueOption> greetingDialogue;

    public List<NPCDialogueOption> goodbyeDialogue;

    public List<NPCDialogueOption> changeTopicDialogue;

    [System.Serializable]
    public struct DialogueConnections
    {
        public PlayerDialogueOption playerDialogueInput;
        public List<Responses> npcResponses;
    }

    [System.Serializable]
    public struct Responses
    {
        public NPCEmotions.Mood npcMood;
        public NPCDialogueOption response;
    }

    public List<DialogueConnections> dialogueConnections; // create a different element for each potential player option

    
}
