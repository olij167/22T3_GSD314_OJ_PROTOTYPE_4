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
    [SerializeField] private GameObject dialoguePanelRow;

    [SerializeField] private bool inDialogue;
    [SerializeField] private bool playerIsSpeaking;
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


    [SerializeField] private int r = 0;
    [SerializeField] private int elementsSpawned = 0;

    [SerializeField] private Slider responseTimerUI;

    //Default player dialogue
    [SerializeField] private PlayerDialogue playerDialogue;

    [SerializeField] private GameObject dialogueUI;

    [SerializeField] private List<GameObject> listSelectionRows;

    private void Update()
    {
        if (inDialogue)
        {
            if (npcDialogue.hasConditionalEvent)
            {
                //Invoke player dialogue conditional event
                foreach (ConditionalEvent conditional in npcDialogue.conditionalEvents)
                {
                    if (conditional.conditionalEvent != null)
                    {
                        conditional.conditionalEvent.Invoke();
                    }
                }
            }

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

    private void CreateListRow()
    {
        listSelectionRows = new List<GameObject>();

        GameObject newListRow = Instantiate(dialoguePanelRow, playerDialoguePanel.transform.position, Quaternion.identity);
        newListRow.transform.SetParent(playerDialoguePanel.transform);

        listSelectionRows.Add(newListRow);
    }

    private void DestroyListRows()
    {
        // destroy all current options
        if (playerDialoguePanel.transform.childCount >= 2)
        {
            foreach (Transform child in playerDialoguePanel.transform)
            {
                if (child != playerDialoguePanel.transform.GetChild(0) && child != playerDialoguePanel.transform.GetChild(1))
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }

    private void CreateDialogueUI()
    {
        foreach (PlayerDialogueOption playerDialogue in npcDialogue.playerResponses)
        {
            if (listSelectionRows[r].transform.childCount <= 4)
            {
                GameObject newDialogue = Instantiate(playerDialoguePrefab, playerDialoguePanel.transform.position, Quaternion.identity);
                newDialogue.GetComponentInChildren<TextMeshProUGUI>().text = playerDialogue.dialogue;
                newDialogue.GetComponent<DialogueListButton>().dialogueOption = playerDialogue;
                newDialogue.transform.SetParent(listSelectionRows[r].transform);
                elementsSpawned++;
            }
            else if (elementsSpawned < npcDialogue.playerResponses.Count)
            {
                GameObject newerListRow = Instantiate(dialoguePanelRow, playerDialoguePanel.transform.position, Quaternion.identity);
                newerListRow.transform.SetParent(playerDialoguePanel.transform);

                listSelectionRows.Add(newerListRow);
                r++;
            }
        }
    }    

    public void ListDialogueOptions()
    {
        if (npcDialogue != null)
        {
            DestroyListRows();

            if (npcDialogue.requiresResponse)
            {
                if (npcDialogue.playerResponses.Count >= 0)
                {
                    r = 0;
                    elementsSpawned = 0;
                    int numOfInquiries = playerDialogue.questions.playerResponses.Count - 1;

                    if (elementsSpawned < numOfInquiries)
                    {
                        CreateListRow();

                        CreateDialogueUI();

                        if (npcDialogue.canChangeTopic && listSelectionRows[r].transform.childCount <= 4)
                        {
                            CreateLeaveListOption(listSelectionRows[r]);

                            if (!playerIsSpeaking && listSelectionRows[r].transform.childCount <= 4)
                            {
                                CreateChangeTopicListOption(listSelectionRows[r]);
                            }
                            else if (!playerIsSpeaking && listSelectionRows[r].transform.childCount >= 4)
                            {
                                CreateListRow();
                                r++;

                                CreateChangeTopicListOption(listSelectionRows[r]);
                            }
                        }
                        else
                        {
                            CreateListRow();
                            r++;

                            CreateLeaveListOption(listSelectionRows[r]);

                            if (!playerIsSpeaking)
                            {
                                CreateChangeTopicListOption(listSelectionRows[r]);
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("NPC Dialogue needs a player response but has access to none. Leaving Dialogue");
                    LeaveDialogue();
                }
            }
            else
            {
                CreateContinueListOption();
            }           
        }
        else
        {
            Debug.Log("NPC Dialogue is null. Leaving Dialogue");
            LeaveDialogue();
        }
    }

    // Update UI Dialogue Text
    public void SetNewDialogueText(NPCDialogueOption npcDialogueOption)
    {
        npcDialogueText.text = npcDialogueOption.dialogue;
        npcDialogue = npcDialogueOption;

        if (npcDialogue.requiresResponse)
        {
            if (npcDialogue.playerResponses.Count <= 0)
            {
                npcDialogue.playerResponses = playerDialogue.SetPlayerQuestionsForNPC(npc, npcDialogue).playerResponses;

                if (playerDialoguePanel.transform.childCount < npcDialogue.playerResponses.Count + 1)
                {
                    ListDialogueOptions();
                }
            }

            if (!npcDialogue == playerDialogue.questions)
            {
                playerIsSpeaking = false;
            }
        }
        else
        {
            ListDialogueOptions();
        }

        SetResponseTimer();

    }

    //Lock in dialogue selection
    public void LockInResponse()
    {
        // Stop & reset response timer
        playerDialogueText.text = selectedDialogueOption.dialogue;

        responseTimerActive = false;
        responseTimer = responseTimerReset;
        responseTimerUI.value = responseTimer;

        // Adjust npc mood values based on selected dialogues
        npc.npcEmotions.emotion = selectedDialogueOption.AffectEmotionValues(npc.npcEmotions.emotion);
        npc.npcEmotions.SetMood();

        if (!npcDialogue.requiresResponse)
        {
            npcDialogue = npcDialogue.continuedDialogue;
        }
        else
        { 
            //Invoke player dialogue conditional event
            if (selectedDialogueOption.hasConditionalEvent)
            {
                foreach (ConditionalEvent conditional in selectedDialogueOption.conditionalEvents)
                {
                    if (conditional.conditionalEvent != null)
                    {
                        conditional.conditionalEvent.Invoke();
                    }
                }
            }

            // NPC select next option based on current mood
            npcDialogue = npc.RespondBasedOnMood(selectedDialogueOption);
        }

        SetNewDialogueText(npcDialogue);
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

        playerIsSpeaking = true;

    }

    private void CreateContinueListOption()
    {
        GameObject newListRow = Instantiate(dialoguePanelRow, playerDialoguePanel.transform.position, Quaternion.identity);
        newListRow.transform.SetParent(playerDialoguePanel.transform);
        listSelectionRows.Add(newListRow);

        GameObject continueDialogue = Instantiate(playerDialoguePrefab, playerDialoguePanel.transform.position, Quaternion.identity);
        continueDialogue.transform.SetParent(newListRow.transform);

        continueDialogue.GetComponentInChildren<TextMeshProUGUI>().text = "   .   .   .   ";
    }

    // Close Dialogue
    private void CreateLeaveListOption(GameObject parent)
    {
        for (int i = 0; i < playerDialogue.playerQuestions.Count; i++)
        {
            if (playerDialogue.playerQuestions[i].npc == npc)
            {
                foreach (PlayerDialogueOption dialogue in playerDialogue.playerQuestions[i].questionsForNPC)
                {
                    if (dialogue.isGoodbyeOption)
                    {
                        playerDialogue.playerQuestions[i].questionsForNPC.Remove(dialogue);
                    }
                }
            }

        }

        int rand = Random.Range(0, playerDialogue.goodbyeDialogue.Count);

        GameObject leaveDialogue = Instantiate(playerDialoguePrefab, playerDialoguePanel.transform.position, Quaternion.identity);
        leaveDialogue.GetComponentInChildren<TextMeshProUGUI>().text = playerDialogue.goodbyeDialogue[rand].dialogue;
        leaveDialogue.transform.SetParent(parent.transform);

        npcDialogue.playerResponses.Add(playerDialogue.goodbyeDialogue[rand]);

        leaveDialogue.transform.GetComponent<DialogueListButton>().dialogueOption = playerDialogue.goodbyeDialogue[rand];
    }
    public void LeaveDialogue()
    {
        if (playerDialoguePanel.transform.childCount >= 2)
        {
            foreach (Transform child in playerDialoguePanel.transform)
            {
                if (child != playerDialoguePanel.transform.GetChild(0) && child != playerDialoguePanel.transform.GetChild(1))
                {
                    Destroy(child.gameObject);
                }
            }
        }

        dialogueUI.SetActive(false);
        inDialogue = false;

        playerMovement.enabled = true;
        playerCam.enabled = true;

        foreach (NPCBrain npc in FindObjectsOfType<NPCBrain>())
        {
            npc.isSpeakingToPlayer = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        DestroyListRows();

        enabled = false;
    }

    private void CreateChangeTopicListOption(GameObject parent)
    {
        int rand = Random.Range(0, playerDialogue.changeTopicDialogue.Count);

        GameObject changeTopicDialogue = Instantiate(playerDialoguePrefab, playerDialoguePanel.transform.position, Quaternion.identity);
        changeTopicDialogue.GetComponentInChildren<TextMeshProUGUI>().text = playerDialogue.changeTopicDialogue[rand].dialogue;
        changeTopicDialogue.transform.SetParent(parent.transform);


        npcDialogue.playerResponses.Add(playerDialogue.changeTopicDialogue[rand]);

        changeTopicDialogue.transform.GetComponent<DialogueListButton>().dialogueOption = playerDialogue.changeTopicDialogue[rand];
    }

    // Return to initial dialogue options
    public void ChangeTopic()
    {
        // get stored inquiries depending on NPC
        npcDialogue = playerDialogue.SetPlayerQuestionsForNPC(npc, npc.npcDialogue.changeTopicDialogue[Random.Range(0, npc.npcDialogue.changeTopicDialogue.Count)]);

        playerIsSpeaking = true;

        Debug.Log("Changing the topic");
    } 
}