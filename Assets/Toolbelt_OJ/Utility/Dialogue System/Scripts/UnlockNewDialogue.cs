using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add method to enable this script in other object
public class UnlockNewDialogue : MonoBehaviour
{
    private PlayerDialogue playerDialogue = FindObjectOfType<PlayerDialogue>();

    public void UnlockDialogueForAll(PlayerDialogueOption dialogueOption)
    {
        playerDialogue.AddQuestionForAllNPCs(dialogueOption);
        playerDialogue.AddDialogueOptions();
    }

    public void UnlockDialogueForSpecificNPC(NPCInfo npc, PlayerDialogueOption dialogueOption)
    {
        playerDialogue.AddQuestionForSpecificNPC(dialogueOption, npc);
        playerDialogue.AddDialogueOptions();
    }
}
