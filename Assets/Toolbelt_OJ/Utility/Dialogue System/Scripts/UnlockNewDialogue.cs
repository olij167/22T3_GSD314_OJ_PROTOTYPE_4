using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add method to enable this script in other object
public class UnlockNewDialogue : MonoBehaviour
{
    private PlayerDialogue playerDialogue;
    [SerializeField] private PlayerDialogueOption newDialogueOption;
    [SerializeField] private bool forSpecificNPC;
    [SerializeField] private List<NPCInfo> specificNPCList;

    private void OnEnable()
    {
        playerDialogue = FindObjectOfType<PlayerDialogue>();

        if (forSpecificNPC)
        {
            foreach (NPCInfo npc in specificNPCList)
            {
                playerDialogue.AddQuestionForSpecificNPC(newDialogueOption, npc);
            }
        }
        else
        {
            playerDialogue.AddQuestionForAllNPCs(newDialogueOption);
        }
    }
}
