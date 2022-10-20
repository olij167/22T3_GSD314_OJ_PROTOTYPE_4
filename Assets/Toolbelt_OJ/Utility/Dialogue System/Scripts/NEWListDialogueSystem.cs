using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class NEWListDialogueSystem : MonoBehaviour
{
    public FirstPersonMovement playerMovement;
    public FirstPersonCam playerCam;

    [SerializeField] private GameObject playerDialoguePrefab;
    [SerializeField] private GameObject listDialoguePanel;

    public bool inDialogue;
    public bool playerIsSpeaking;

    public NPCInfo npc;

    //Dialogue to display in npc text box
    public NPCDialogueOption npcDialogue;

    //Dialogue to display in the player text box
    public PlayerDialogueOption selectedDialogueOption;

    //Dialogue UI
    public TextMeshProUGUI npcNameText;
    //public TextMeshProUGUI npcMoodText;
    [SerializeField] private TextMeshProUGUI npcDialogueText;
    public TextMeshProUGUI playerDialogueText;

    //public bool playerResponseLockedIn;



    //Response Timer Variables
    private bool responseTimerActive;
    private float responseTimer = 5f;
    private float responseTimerReset;

    [SerializeField] private Slider responseTimerUI;   // if true the timer will automatically start during a time-limited response and pick a random option if the player doesn't begin viewing the dialogue options
                                                       // if false the timer won't start until the player has begun viewing the dialogue options
                                                       //Default player dialogue
    [SerializeField] private PlayerDialogue playerDialogue;

    [SerializeField] private GameObject dialogueUI;

    private int elementNum = 0;

    private void Update()
    {
        if (inDialogue)
        {
            if (npcDialogue.requiresResponse)
            {

                if (responseTimerActive)
                {
                    responseTimer -= Time.deltaTime;
                    responseTimerUI.value = responseTimer;

                    if (responseTimer <= 0f)
                    {
                        // select a random option if the player hasn't selected in time
                        selectedDialogueOption = npcDialogue.playerResponses[Random.Range(0, npcDialogue.playerResponses.Count)];
                        playerDialogueText.text = selectedDialogueOption.dialogue;

                        LockInResponse();
                    }
                }
            }
            
        }
        else
        {
            LeaveDialogue();
        }
    }


    // Update UI Dialogue Text
    public void SetNewDialogueText(NPCDialogueOption npcDialogueOption)
    {
        if (inDialogue)
        {
            npcDialogueText.text = npcDialogueOption.dialogue;
            npcDialogue = npcDialogueOption;

            DestroyOldDialogueOptions();

            if (npcDialogue.playerResponses.Count <= 0)
            {
                npcDialogue.playerResponses = playerDialogue.SetPlayerDialogueBasedOnCurrentNPCAndDialogue(npc, npcDialogue).playerResponses;
            }

            if (!npcDialogue == playerDialogue.questions)
            {
                playerIsSpeaking = false;
            }

            SetResponseTimer();

            CreateDialogueOptions(npcDialogue);
        }
    }

    public void DestroyOldDialogueOptions()
    {
        for (int i = 0; i < listDialoguePanel.transform.childCount; i++)
        {
            Destroy(listDialoguePanel.transform.GetChild(i).gameObject);
        }
    }

    public void CreateDialogueOptions(NPCDialogueOption npcDialogueOption)
    {
        if (npcDialogue.requiresResponse)
        {
            foreach (PlayerDialogueOption dialogueOption in npcDialogueOption.playerResponses)
            {
                if (dialogueOption == playerDialogue.continueDialogue)
                {
                    return;
                }

                GameObject newDialogue = Instantiate(playerDialoguePrefab, listDialoguePanel.transform.position, Quaternion.identity);
                newDialogue.GetComponentInChildren<TextMeshProUGUI>().text = dialogueOption.dialogue;
                newDialogue.GetComponent<DialogueListButton>().dialogueOption = dialogueOption;
                newDialogue.transform.SetParent(listDialoguePanel.transform);
            }
        }
        else
        {
            CreateContinueListOption();
        }

        if (npcDialogueOption.playerCanChangeTopic)
        {
            CreateLeaveListOption();

            if (!playerIsSpeaking)
            {
                CreateChangeTopicListOption();
            }
        }
    }

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
                        GameObject leaveDialogue = Instantiate(playerDialoguePrefab, listDialoguePanel.transform.position, Quaternion.identity);
                        leaveDialogue.GetComponentInChildren<TextMeshProUGUI>().text = playerDialogue.goodbyeDialogue[rand].dialogue;
                        leaveDialogue.transform.SetParent(listDialoguePanel.transform);

                        npcDialogue.playerResponses.Add(playerDialogue.goodbyeDialogue[rand]);

                        leaveDialogue.transform.GetComponent<DialogueListButton>().dialogueOption = playerDialogue.goodbyeDialogue[rand];
                    }
                }
            }

        }
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
                        GameObject changeTopicDialogue = Instantiate(playerDialoguePrefab, listDialoguePanel.transform.position, Quaternion.identity);
                        changeTopicDialogue.GetComponentInChildren<TextMeshProUGUI>().text = playerDialogue.changeTopicDialogue[rand].dialogue;
                        changeTopicDialogue.transform.SetParent(listDialoguePanel.transform);


                        npcDialogue.playerResponses.Add(playerDialogue.changeTopicDialogue[rand]);

                        changeTopicDialogue.transform.GetComponent<DialogueListButton>().dialogueOption = playerDialogue.changeTopicDialogue[rand];
                    }

                }
            }

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
                        GameObject continueDialogue = Instantiate(playerDialoguePrefab, listDialoguePanel.transform.position, Quaternion.identity);
                        continueDialogue.transform.SetParent(listDialoguePanel.transform);
                        continueDialogue.GetComponentInChildren<TextMeshProUGUI>().text = playerDialogue.continueDialogue.dialogue;

                        npcDialogue.playerResponses.Add(playerDialogue.continueDialogue);

                        continueDialogue.transform.GetComponent<DialogueListButton>().dialogueOption = playerDialogue.continueDialogue;
                    }
                }
            }
        }
    }

    //Lock in dialogue selection
    public void LockInResponse()
    {
        playerDialogueText.text = selectedDialogueOption.dialogue;

        responseTimer = responseTimerReset;
        responseTimerUI.value = responseTimer;
        responseTimerActive = false;

        npc.npcEmotions.emotion = selectedDialogueOption.AffectEmotionValues(npc.npcEmotions.emotion);
        npc.npcEmotions.SetMood();


        if (playerDialogue.changeTopicDialogue.Contains(selectedDialogueOption))
        {
            if (!playerIsSpeaking)
            {
                ChangeTopic();
            }
        }

        if (selectedDialogueOption.isGoodbyeOption || npcDialogue.endOfConversation)
        {
            LeaveDialogue();
        }

        if (!npcDialogue.requiresResponse)
        {
            npcDialogue = npcDialogue.continuedDialogue;
        }
        else
        {
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

    // Close Dialogue
    public void LeaveDialogue()
    {

        DestroyOldDialogueOptions();
        dialogueUI.SetActive(false);
        inDialogue = false;

        playerMovement.enabled = true;
        playerCam.enabled = true;

        foreach (NPCBrain npc in FindObjectsOfType<NPCBrain>())
        {
            npc.isSpeakingToPlayer = false;
        }

        Cursor.lockState = CursorLockMode.Locked;

        enabled = false;
    }

    // Return to initial dialogue options
    public void ChangeTopic()
    {
        // get stored inquiries depending on NPC
        npcDialogue = playerDialogue.SetPlayerDialogueBasedOnCurrentNPCAndDialogue(npc, npc.npcDialogue.changeTopicDialogue[Random.Range(0, npc.npcDialogue.changeTopicDialogue.Count)]);


        //npcDialogue = playerDialogue.questions;
        //playerDialogue.questions.dialogue = npcDialogue.dialogue;

        playerIsSpeaking = true;

        Debug.Log("Changing the topic");
    }
}