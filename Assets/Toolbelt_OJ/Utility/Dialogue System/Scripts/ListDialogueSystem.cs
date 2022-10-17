using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ListDialogueSystem : MonoBehaviour
{
    public FirstPersonMovement playerMovement;
    public FirstPersonCam playerCam;

    [SerializeField] private GameObject playerDialoguePrefab;
    [SerializeField] private GameObject playerDialoguePanel;

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
    //public TextMeshProUGUI npcMoodText;
    [SerializeField] private TextMeshProUGUI npcDialogueText;
    public TextMeshProUGUI playerDialogueText;

    //Response Timer Variables
    private bool responseTimerActive;
    private float responseTimer = 5f;
    private float responseTimerReset;

    [SerializeField] private Slider responseTimerUI;   // if true the timer will automatically start during a time-limited response and pick a random option if the player doesn't begin viewing the dialogue options
                                                       // if false the timer won't start until the player has begun viewing the dialogue options
   // [SerializeField] private Slider happinessBar, stressBar, shockBar;

    //Default player dialogue
    [SerializeField] private PlayerDialogue playerDialogue;

    [SerializeField] private GameObject dialogueUI;

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
                            playerDialogueText.text = selectedDialogueOption.dialogue;

                            LockInResponse();
                        }
                    }
                }
            }
            else
            {
                CreateContinueListOption();
            }
        }
        else
        {
            LeaveDialogue();
        }
    }
    public void ListDialogueOptions()
    {
        if (npcDialogue != null)
        {
            if (playerDialoguePanel.transform.childCount > 2)
            {
                foreach (Transform child in playerDialoguePanel.transform)
                {
                    if (child != playerDialoguePanel.transform.GetChild(0) && child != playerDialoguePanel.transform.GetChild(playerDialoguePanel.transform.childCount))
                    {
                        Destroy(child);
                    }
                }
            }

            if (npcDialogue.playerResponses.Count > 0)
            {
                foreach (PlayerDialogueOption dialogue in npcDialogue.playerResponses)
                {
                    GameObject newDialogue = Instantiate(playerDialoguePrefab, playerDialoguePanel.transform.position, Quaternion.identity);
                    newDialogue.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.dialogue;
                    newDialogue.transform.SetParent(playerDialoguePanel.transform);
                    newDialogue.GetComponent<DialogueListButton>().dialogueOption = dialogue;
                }
            }
            else
            {
                CreateContinueListOption();
            }

            //Create leave conversation / change topic buttons

            if (npcDialogue.canChangeTopic)
            {
                CreateLeaveListOption();

                if (!playerIsSpeaking)
                {
                    CreateChangeTopicListOption();
                }
            }
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

        SetResponseTimer();

    }

    //Lock in dialogue selection
    public void LockInResponse()
    {
        // Stop & reset response timer
        responseTimerActive = false;
        responseTimer = responseTimerReset;
        responseTimerUI.value = responseTimer;

        // Adjust npc mood values based on selected dialogues
        npc.npcEmotions.emotion = selectedDialogueOption.AffectEmotionValues(npc.npcEmotions.emotion);
        npc.npcEmotions.SetMood();


        //// adjust UI emotion UI sliders
        //happinessBar.value = npc.npcEmotions.emotion.happiness;
        //stressBar.value = npc.npcEmotions.emotion.stress;
        //shockBar.value = npc.npcEmotions.emotion.shock;

        if (!npcDialogue.requiresResponse)
        {
            npcDialogue = npcDialogue.continuedDialogue;
        }
        else
        {
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

        //happinessBar.maxValue = npc.npcEmotions.personality.happyMinThreshold + 10;
        //happinessBar.minValue = npc.npcEmotions.personality.sadMinThreshold - 10;
        //happinessBar.value = npc.npcEmotions.emotion.happiness;

        //stressBar.maxValue = npc.npcEmotions.personality.angryMinThreshold + 10;
        //stressBar.minValue = npc.npcEmotions.personality.sadMinThreshold - 10;
        //stressBar.value = npc.npcEmotions.emotion.stress;

        //shockBar.maxValue = npc.npcEmotions.personality.surprisedMinThreshold + 10;
        //shockBar.minValue = npc.npcEmotions.personality.scaredMinThreshold - 10;
        //shockBar.value = npc.npcEmotions.emotion.shock;

        playerIsSpeaking = true;

    }

    private void CreateContinueListOption()
    {
        //int rand = Random.Range(0, playerDialogue.goodbyeDialogue.Count);

        GameObject continueDialogue = Instantiate(playerDialoguePrefab, playerDialoguePanel.transform.position, Quaternion.identity);
        continueDialogue.transform.SetParent(playerDialoguePanel.transform);

        continueDialogue.GetComponentInChildren<TextMeshProUGUI>().text = "...";
    }

    // Close Dialogue
    private void CreateLeaveListOption()
    {
        int rand = Random.Range(0, playerDialogue.goodbyeDialogue.Count);

        GameObject leaveDialogue = Instantiate(playerDialoguePrefab, playerDialoguePanel.transform.position, Quaternion.identity);
        leaveDialogue.GetComponentInChildren<TextMeshProUGUI>().text = playerDialogue.goodbyeDialogue[rand].dialogue;
        leaveDialogue.transform.SetParent(playerDialoguePanel.transform);

        npcDialogue.playerResponses.Add(playerDialogue.goodbyeDialogue[rand]);

        leaveDialogue.transform.GetComponent<DialogueListButton>().dialogueOption = playerDialogue.goodbyeDialogue[rand];
    }
    public void LeaveDialogue()
    {
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

        //Activate Player Controller
        //Unlock Camera

        enabled = false;
    }

    private void CreateChangeTopicListOption()
    {
        int rand = Random.Range(0, playerDialogue.changeTopicDialogue.Count);

        GameObject changeTopicDialogue = Instantiate(playerDialoguePrefab, playerDialoguePanel.transform.position, Quaternion.identity);
        changeTopicDialogue.GetComponentInChildren<TextMeshProUGUI>().text = playerDialogue.changeTopicDialogue[rand].dialogue;
        changeTopicDialogue.transform.SetParent(playerDialoguePanel.transform);


        npcDialogue.playerResponses.Add(playerDialogue.changeTopicDialogue[rand]);

        changeTopicDialogue.transform.GetComponent<DialogueListButton>().dialogueOption = playerDialogue.changeTopicDialogue[rand];
    }

    // Return to initial dialogue options
    public void ChangeTopic()
    {
        // get stored inquiries depending on NPC
        npcDialogue = playerDialogue.SetPlayerQuestionsForNPC(npc, npc.npcDialogue.changeTopicDialogue[Random.Range(0, npc.npcDialogue.changeTopicDialogue.Count)]);


        //npcDialogue = playerDialogue.questions;
        //playerDialogue.questions.dialogue = npcDialogue.dialogue;

        playerIsSpeaking = true;

        Debug.Log("Changing the topic");
    }
}