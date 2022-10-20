using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


public class ListDialogueSystem : MonoBehaviour
{
    public FirstPersonMovement playerMovement;
    public FirstPersonCam playerCam;

    [SerializeField] private GameObject playerDialoguePrefab;
    [SerializeField] private GameObject playerDialoguePanel;
    [SerializeField] private GameObject dialoguePanelColumn;

    public bool inDialogue;
    [SerializeField] private bool playerIsLeadingConversation;
    [SerializeField] private bool timerAutoStart;

    public NPCInfo npc;

    //Dialogue to display in npc text box
    public NPCDialogueOption npcDialogue;

    //Dialogue to display in the player text box
    public PlayerDialogueOption selectedDialogueOption;

    //Dialogue UI
    public TextMeshProUGUI npcNameText;
    [SerializeField] private TextMeshProUGUI npcDialogueText;
    public TextMeshProUGUI playerDialogueText;

    //Response Timer Variables
    private bool responseTimerActive;
    private float responseTimer = 5f;
    private float responseTimerReset;


    //[SerializeField] private int r = 0;
    //[SerializeField] private int elementsSpawned = 0;

    [SerializeField] private Slider responseTimerUI;

    //Default player dialogue
    [SerializeField] private PlayerDialogue playerDialogue;

    [SerializeField] private GameObject dialogueUI;

    //[SerializeField] private List<GameObject> listSelectionRows;
    //[SerializeField] GameObject listSelectionRow;

    private void Update()
    {
        if (inDialogue)
        {
            

            if (npcDialogue.requiresResponse)
            {
                if (responseTimerActive)
                {
                    if (timerAutoStart) //start the timer straight away
                    {
                        responseTimer -= Time.deltaTime;
                        responseTimerUI.value = responseTimer;

                        if (responseTimer <= 0f) // lock in random response after timer
                        {
                            selectedDialogueOption = npcDialogue.playerResponses[Random.Range(0, npcDialogue.playerResponses.Count)];

                            LockInResponse();
                        }
                    }
                }
            }
        }
        else
        {
            LeaveDialogue();
        }
    }

    //Lock in dialogue selection
    public void LockInResponse()
    {
        // Stop & reset response timer
        if (responseTimerActive)
        {
            responseTimer = responseTimerReset;
            responseTimerUI.value = responseTimer;
            responseTimerActive = false;
        }

        if (npcDialogue.endOfConversation)
        {
            inDialogue = false;
        }

        if (npcDialogue.changeOfTopic)
        {
            ChangeTopic();
        }

        if (!npcDialogue.requiresResponse)
        {
            selectedDialogueOption = playerDialogue.continueDialogue;
            npcDialogue = npcDialogue.continuedDialogue;
        }

        if (selectedDialogueOption != null)
        {
            ////Invoke player dialogue conditional event
            //if (selectedDialogueOption.conditionalEvents.Count > 0)
            //{
            //    foreach (ConditionalEvent conditional in selectedDialogueOption.conditionalEvents)
            //    {
            //        if (conditional.conditionalEvent != null)
            //        {
            //            conditional.SetNPC(npc);

            //            conditional.conditionalEvent.Invoke();
            //        }
            //    }
            //}

            playerDialogueText.text = selectedDialogueOption.dialogue;


            // Adjust npc mood values based on selected dialogues
            npc.npcEmotions.emotion = selectedDialogueOption.AffectEmotionValues(npc.npcEmotions.emotion);
            npc.npcEmotions.SetMood();

            // NPC select next option based on current mood
            npcDialogue = npc.RespondBasedOnMood(selectedDialogueOption);

        }


        SetNewDialogueText(npcDialogue);
    }

    // Update UI Dialogue Text
    public void SetNewDialogueText(NPCDialogueOption npcDialogueOption)
    {
        if (inDialogue)
        {
            npcDialogue = npcDialogueOption;
            npcDialogueText.text = npcDialogueOption.dialogue;

            if (npcDialogue.requiresResponse)
            {
                if (npcDialogue.playerResponses.Count <= 0)
                {
                    playerDialogue.AddDialogueOptions();
                    npcDialogue.playerResponses = playerDialogue.SetPlayerDialogueBasedOnCurrentNPCAndDialogue(npc, npcDialogue).playerResponses;
                }
                else
                {
                    playerDialogue.AddResponseOptions();
                    npcDialogue.playerResponses = playerDialogue.SetPlayerDialogueBasedOnCurrentNPCAndDialogue(npc, npcDialogue).playerResponses;
                }

                if (!npcDialogue == playerDialogue.questions)
                {
                    playerIsLeadingConversation = false;
                }
            }
            //else
            //{
            //    playerIsLeadingConversation = false;

            //    //playerDialogue.AddDialogueOptions();
            //    //npcDialogue.playerResponses = playerDialogue.SetPlayerDialogueBasedOnCurrentNPCAndDialogue(npc, npcDialogue).playerResponses;
            //}

            ListDialogueOptions(npcDialogue);

            //if (npcDialogue.conditionalEvents.Count > 0)
            //{

            //    //Invoke player dialogue conditional event
            //    foreach (ConditionalEvent conditional in npcDialogue.conditionalEvents)
            //    {
            //        if (conditional.conditionalEvent != null)
            //        {
            //            conditional.SetNPC(npc);
            //            conditional.conditionalEvent.Invoke();
            //        }
            //    }
            //}

            SetResponseTimer();
        }
        else
        {
            LeaveDialogue();
        }
    }

    public void ListDialogueOptions(NPCDialogueOption dialogueOption)
    {
        if (npcDialogue != null)
        {
            foreach (Transform child in dialoguePanelColumn.transform)
            {
                Destroy(child.gameObject);
            }

            if (npcDialogue.requiresResponse)
            {
                foreach (PlayerDialogueOption playerDialogueOption in dialogueOption.playerResponses)
                {
                    CreateDialogueOptionUI(playerDialogueOption);
                }
            }
            else
            {
                CreateContinueListOption();
            }

            if (npcDialogue.playerCanChangeTopic)
            {
                CreateRecursiveListOptions();
            }
        }
        else
        {
            Debug.Log("NPC Dialogue is null. Leaving Dialogue");
            inDialogue = false;
        }
    }

    private void CreateDialogueOptionUI(PlayerDialogueOption dialogueOption)
    {
        GameObject newDialogue = Instantiate(playerDialoguePrefab, playerDialoguePanel.transform.position, Quaternion.identity);
        newDialogue.GetComponentInChildren<TextMeshProUGUI>().text = dialogueOption.dialogue;
        newDialogue.GetComponent<DialogueListButton>().dialogueOption = dialogueOption;
        newDialogue.transform.SetParent(dialoguePanelColumn.transform);
    }

    public void CreateRecursiveListOptions()
    {
        //Debug.Log("Creating Leave (and maybe change topic) dialogue options");
        if (npcDialogue.playerCanChangeTopic)
        {
            if (!playerIsLeadingConversation)
            {
                CreateChangeTopicListOption();
            }

            CreateLeaveListOption();
        }
    }

    private void CreateContinueListOption()
    {
        for (int i = 0; i < playerDialogue.playerQuestions.Count; i++)
        {
            if (playerDialogue.playerQuestions[i].npc == npc)
            {
                for (int d = 0; d < playerDialogue.playerQuestions[i].questionsForNPC.Count; d++)
                {
                    if (playerDialogue.playerQuestions[i].questionsForNPC[d] == playerDialogue.continueDialogue)
                    {
                        return;
                    }
                    else if (d + 1 == playerDialogue.playerQuestions[i].questionsForNPC.Count)
                    {
                        GameObject continueDialogue = Instantiate(playerDialoguePrefab, playerDialoguePanel.transform.position, Quaternion.identity);
                        continueDialogue.transform.SetParent(dialoguePanelColumn.transform);
                        continueDialogue.GetComponentInChildren<TextMeshProUGUI>().text = playerDialogue.continueDialogue.dialogue;

                        npcDialogue.playerResponses.Add(playerDialogue.continueDialogue);

                        continueDialogue.transform.GetComponent<DialogueListButton>().dialogueOption = playerDialogue.continueDialogue;
                    }
                }
            }
        }
    }

    // Close Dialogue
    private void CreateLeaveListOption()
    {
        int rand = Random.Range(0, playerDialogue.goodbyeDialogue.Count);

        for (int i = 0; i < playerDialogue.playerQuestions.Count; i++)
        {
            if (playerDialogue.playerQuestions[i].npc == npc)
            {

                for (int d = 0; d < playerDialogue.playerQuestions[i].questionsForNPC.Count; d++)
                {
                    if (playerDialogue.playerQuestions[i].questionsForNPC[d].isGoodbyeOption)
                    {
                        playerDialogue.playerQuestions[i].questionsForNPC[d].dialogue = playerDialogue.goodbyeDialogue[rand].dialogue;
                    }
                    else if (d + 1 == playerDialogue.playerQuestions[i].questionsForNPC.Count)
                    {
                        GameObject leaveDialogue = Instantiate(playerDialoguePrefab, playerDialoguePanel.transform.position, Quaternion.identity);
                        leaveDialogue.GetComponentInChildren<TextMeshProUGUI>().text = playerDialogue.goodbyeDialogue[rand].dialogue;
                        leaveDialogue.transform.SetParent(dialoguePanelColumn.transform);

                        npcDialogue.playerResponses.Add(playerDialogue.goodbyeDialogue[rand]);

                        leaveDialogue.transform.GetComponent<DialogueListButton>().dialogueOption = playerDialogue.goodbyeDialogue[rand];
                    }
                }
            }

        } 
    }
    public void LeaveDialogue()
    {
        dialogueUI.SetActive(false);

        playerMovement.enabled = true;
        playerCam.enabled = true;

        foreach (NPCBrain npc in FindObjectsOfType<NPCBrain>())
        {
            npc.isSpeakingToPlayer = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach(Transform child in dialoguePanelColumn.transform)
        {
            Destroy(child.gameObject);
        }

        inDialogue = false;


        enabled = false;
    }

    private void CreateChangeTopicListOption()
    {
        int rand = Random.Range(0, playerDialogue.changeTopicDialogue.Count);

        for (int i = 0; i < playerDialogue.playerQuestions.Count; i++)
        {
            if (playerDialogue.playerQuestions[i].npc == npc)
            {
                for (int d = 0; d < playerDialogue.playerQuestions[i].questionsForNPC.Count; d++)
                {
                    if (playerDialogue.playerQuestions[i].questionsForNPC[d].isGoodbyeOption)
                    {
                        playerDialogue.playerQuestions[i].questionsForNPC[d].dialogue = playerDialogue.goodbyeDialogue[rand].dialogue;
                    }
                    else if (d + 1 == playerDialogue.playerQuestions[i].questionsForNPC.Count)
                    {
                        GameObject changeTopicDialogue = Instantiate(playerDialoguePrefab, playerDialoguePanel.transform.position, Quaternion.identity);
                        changeTopicDialogue.GetComponentInChildren<TextMeshProUGUI>().text = playerDialogue.changeTopicDialogue[rand].dialogue;
                        changeTopicDialogue.transform.SetParent(dialoguePanelColumn.transform);


                        npcDialogue.playerResponses.Add(playerDialogue.changeTopicDialogue[rand]);

                        changeTopicDialogue.transform.GetComponent<DialogueListButton>().dialogueOption = playerDialogue.changeTopicDialogue[rand];
                    }

                }
            }

        } 
    }

    // Return to initial dialogue options
    public void ChangeTopic()
    {
        selectedDialogueOption = playerDialogue.changeTopicDialogue[Random.Range(0, playerDialogue.changeTopicDialogue.Count)];
        npcDialogue = npc.npcDialogue.changeTopicDialogue[Random.Range(0, npc.npcDialogue.changeTopicDialogue.Count)];

        playerIsLeadingConversation = true;


        Debug.Log("Changing the topic");
    }

    // set response timer values & activate it
    private void SetResponseTimer()
    {
        if (npcDialogue.limitedTime)
        {
            responseTimer = npcDialogue.timeLimit;

            responseTimerReset = responseTimer;
            responseTimerUI.maxValue = responseTimerReset;
            responseTimerUI.value = responseTimer;

            responseTimerActive = true;

        }
        else
        {
            responseTimerReset = responseTimer;
            responseTimerUI.maxValue = responseTimerReset;
            responseTimerUI.value = responseTimer;

            responseTimerActive = false;

        }
    }

    //Initiate Dialogue
    public void BeginDialogue()
    {
        inDialogue = true;
        dialogueUI.SetActive(true);

        playerIsLeadingConversation = true;

    }
}