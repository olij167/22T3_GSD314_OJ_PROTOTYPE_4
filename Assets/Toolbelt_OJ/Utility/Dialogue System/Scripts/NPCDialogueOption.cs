using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("DialogueSystem/NPCDialogue"))]
public class NPCDialogueOption : ScriptableObject
{
    [TextArea(3, 10)]
    public string dialogue; // set dialogue text

    public bool requiresResponse = true;   // Set to false to disable player dialogue selection

    public NPCDialogueOption continuedDialogue; //Set another dialogue option to follow this
                                          //**only if requiresResponse is false
    
    public List<PlayerDialogueOption> playerResponses; //Set player dialogue options to respond with

    public bool limitedTime; // If true player will need to respond within a time limit, when the time runs out the currently selected option will be said
    public float timeLimit;

    public bool canChangeTopic = true; //if true the player will be able to change the topic or leave when responding
                                       //**unless there is a time limit

    public bool endOfConversation;

}
