using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueListButton : MonoBehaviour
{
    public ListDialogueSystem dialogueSystem;
    public PlayerDialogueOption dialogueOption;

    private void Awake()
    {
        dialogueSystem = FindObjectOfType<ListDialogueSystem>();
    }

    public void OnClickSelectDialogue()
    {
        dialogueSystem.selectedDialogueOption = dialogueOption;

        //Debug.Log("Selected Dialogue = " + dialogueOption.dialogue);

        if (dialogueSystem.npcDialogue.requiresResponse)
        {
            //Debug.Log("It's a goodbye right? = " + dialogueOption.isGoodbyeOption);
            //Debug.Log("It's a change topic right? = " + dialogueOption.isChangeTopicOption);

            if (dialogueOption.isGoodbyeOption)
            {
                dialogueSystem.LeaveDialogue();
            }
            else if (dialogueOption.isChangeTopicOption)
            {
                dialogueSystem.ChangeTopic();
            }
            else
            {
                dialogueSystem.LockInResponse();
            }
        }
        else
        {
            dialogueSystem.LockInResponse();
        }
    }
}
