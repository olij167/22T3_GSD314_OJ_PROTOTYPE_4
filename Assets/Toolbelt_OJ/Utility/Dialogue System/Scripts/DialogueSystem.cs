using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogueSystem : MonoBehaviour
{
    public FirstPersonMovement playerMovement;
    public FirstPersonCam playerCam;

    [SerializeField] private bool displayAsList;
    [SerializeField] private TextMeshProUGUI playerDialoguePrefab;
    [SerializeField] private GameObject listDialoguePanel;

    [SerializeField] private bool inDialogue;
    [SerializeField] private bool playerIsSpeaking;
    [SerializeField] private bool timerAutoStart;

    public NPCInfo npc;

    //Dialogue to display in npc text box
    [SerializeField] private NPCDialogueOption npcDialogue;

    //Dialogue to display in the player text box
    public PlayerDialogueOption selectedDialogueOption;

    //Dialogue UI
    public TextMeshProUGUI npcNameText;
    //public TextMeshProUGUI npcMoodText;
    [SerializeField] private TextMeshProUGUI npcDialogueText;
    public TextMeshProUGUI playerDialogueText;
    public Sprite defaultSprite, pressedSprite;
    public Sprite defaultSpaceSprite, pressedSpaceSprite;
    public UIAnimator pressSpaceAnimation;

    //player Dialogue Selection UI
    [SerializeField] private List<Image> selectedOptionImages;
    [SerializeField] private TextMeshProUGUI selectedOptionText;

    [SerializeField] private bool playerResponseLockedIn;

    //Response Timer Variables
    private bool responseTimerActive;
    private float responseTimer = 5f;
    private float responseTimerReset;

    [SerializeField] private Slider responseTimerUI;   // if true the timer will automatically start during a time-limited response and pick a random option if the player doesn't begin viewing the dialogue options
                                                       // if false the timer won't start until the player has begun viewing the dialogue options
    [SerializeField] private Slider happinessBar, stressBar, shockBar;

    //Default player dialogue
    [SerializeField] private PlayerDialogue playerDialogue;

    [SerializeField] private GameObject dialogueUI;

    private int WElementNum = 0, AElementNum = 1, DElementNum = 2, SElementNum = 3;

    private void Update()
    {
        if (inDialogue)
        {

            if (npcDialogue.requiresResponse)
            {
                if (!displayAsList)
                {
                    if (!playerResponseLockedIn)
                    {
                        selectedOptionImages[6].enabled = true;

                        //selectedOptionImages[6].sprite = defaultSpaceSprite;



                        ViewFullDialogueOption();

                        if (selectedDialogueOption != null)
                        {
                            selectedOptionImages[6].color = Color.white;
                            //selectedOptionImages[6].sprite = defaultSpaceSprite;
                            pressSpaceAnimation.enabled = true;

                        }
                        else
                        {
                            selectedOptionImages[6].color = Color.grey;

                            pressSpaceAnimation.enabled = false;

                            selectedOptionImages[6].sprite = defaultSpaceSprite;
                        }

                        if (responseTimerActive)
                        {
                            if (timerAutoStart) //start the timer straight away
                            {
                                responseTimer -= Time.deltaTime;
                                responseTimerUI.value = responseTimer;

                                if (responseTimer <= 0f)
                                {
                                    // select a random option if the player doesn't have anything selected
                                    if (selectedDialogueOption == null)
                                    {
                                        selectedDialogueOption = npcDialogue.playerResponses[Random.Range(0, npcDialogue.playerResponses.Count)];
                                        playerDialogueText.text = selectedDialogueOption.dialogue;
                                    }

                                    LockInResponse();
                                }
                            }
                            else if (selectedDialogueOption != null) // start the timer after the player has selected an option
                            {
                                responseTimer -= Time.deltaTime;
                                responseTimerUI.value = responseTimer;

                                if (responseTimer <= 0f)
                                {
                                    LockInResponse();
                                }
                            }
                        }

                        //Manually lock in player Answer
                        if (selectedDialogueOption != null)
                        {
                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                LockInResponse();
                            }
                        }
                    }
                    else if (selectedOptionText.text != "Q" && !npcDialogue.endOfConversation)
                    {
                        //Progress Conversation

                        if (selectedOptionText.text != "E")
                        {
                            // Check if dialogue requires the player to continue the conversation
                            if (!npcDialogue.requiresResponse)
                            {
                                //If it doesn't the npc dialogue will only read the first element from the current npc DialogueOption's response list
                                npcDialogue = npcDialogue.continuedDialogue;
                            }
                            else
                            {
                                // if the player responds the npc dialogue will only read the first element from the selected DialogueOption's response list
                                npcDialogue = npc.RespondBasedOnMood(selectedDialogueOption);
                                //selectedDialogueOption.npcResponse;
                            }
                        }

                        SetNewDialogueText(npcDialogue);

                    }

                }
                else
                {
                    ListDialogueOptions();
                }
            }
            else
            {
                selectedOptionImages[6].color = Color.white;
                //selectedOptionImages[6].sprite = defaultSpaceSprite;

                pressSpaceAnimation.enabled = true;


                if (Input.GetKeyDown(KeyCode.Space))
                {
                    selectedOptionImages[6].sprite = pressedSpaceSprite;

                    if (npcDialogue.endOfConversation)
                    {

                        LeaveDialogue();
                    }
                    else
                    {

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

    // Change Currently selected dialogue & update UI
    private void ViewFullDialogueOption()
    {
        //select W response
        if (npcDialogue.playerResponses.Count >= 1)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                playerDialogueText.text = npcDialogue.playerResponses[WElementNum].dialogue;
                selectedDialogueOption = npcDialogue.playerResponses[WElementNum];
                HighlightSelectedOption(0);
                selectedOptionText.text = "W";
                Debug.Log("Displaying W Option");
            }
        }

        //select A response
        if (npcDialogue.playerResponses.Count >= 2)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                playerDialogueText.text = npcDialogue.playerResponses[AElementNum].dialogue;
                selectedDialogueOption = npcDialogue.playerResponses[AElementNum];
                HighlightSelectedOption(1);
                selectedOptionText.text = "A";
                Debug.Log("Displaying A Option");

            }
        }

        //select D response
        if (npcDialogue.playerResponses.Count >= 3)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                playerDialogueText.text = npcDialogue.playerResponses[DElementNum].dialogue;
                selectedDialogueOption = npcDialogue.playerResponses[DElementNum];
                HighlightSelectedOption(2);
                selectedOptionText.text = "D";
                Debug.Log("Displaying D Option");


            }
        }

        //select S response
        if (npcDialogue.playerResponses.Count >= 4)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                playerDialogueText.text = npcDialogue.playerResponses[SElementNum].dialogue;
                selectedDialogueOption = npcDialogue.playerResponses[SElementNum];
                HighlightSelectedOption(3);
                selectedOptionText.text = "S";
                Debug.Log("Displaying S Option");


            }
        }

        //select Leave Conversation
        if (playerIsSpeaking && npcDialogue.canChangeTopic && !npcDialogue.limitedTime)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                int rand = Random.Range(0, playerDialogue.goodbyeDialogue.Count);
                playerDialogueText.text = playerDialogue.goodbyeDialogue[rand].dialogue;
                selectedDialogueOption = playerDialogue.goodbyeDialogue[rand];
                HighlightSelectedOption(4);
                selectedOptionText.text = "Q";
                Debug.Log("Displaying Q Option");
            }
        }

        //select Change Topic / View more responses
        if (playerIsSpeaking && npcDialogue.canChangeTopic && !npcDialogue.limitedTime)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (playerDialogue.questions.playerResponses.Count < 4)
                {
                    int rand = Random.Range(0, playerDialogue.changeTopicDialogue.Count);
                    playerDialogueText.text = playerDialogue.changeTopicDialogue[rand].dialogue;
                    selectedDialogueOption = playerDialogue.changeTopicDialogue[rand];
                }
                else
                {
                    playerDialogueText.text = playerDialogue.viewMoreDialogue.dialogue;
                    selectedDialogueOption = playerDialogue.viewMoreDialogue;
                }

                HighlightSelectedOption(5);
                selectedOptionText.text = "E";
                Debug.Log("Displaying E Option");
            }
        }

    }


    private void ListDialogueOptions()
    {

        foreach (Transform child in listDialoguePanel.transform)
        {
            if (child != listDialoguePanel.transform.GetChild(0))
            {
                Destroy(child);
            }
        }

        foreach (PlayerDialogueOption dialogue in npcDialogue.playerResponses)
        {
            TextMeshProUGUI newDialogue = Instantiate(playerDialoguePrefab, listDialoguePanel.transform.position, Quaternion.identity);
            newDialogue.text = dialogue.dialogue;
            newDialogue.GetComponent<DialogueListButton>().dialogueOption = dialogue;
        }

        //Create leave conversation / change topic buttons
    }

    // Update UI Dialogue Text
    public void SetNewDialogueText(NPCDialogueOption npcDialogueOption)
    {
        npcDialogueText.text = npcDialogueOption.dialogue;
        npcDialogue = npcDialogueOption;

        playerResponseLockedIn = false;
        ResetHighlightedOption();

        if (npcDialogue.requiresResponse)
        {
            if (npcDialogue.playerResponses.Count <= 0)
            {
                npcDialogue.playerResponses = playerDialogue.SetPlayerQuestionsForNPC(npc, npcDialogue).playerResponses;
            }

            if (!npcDialogue == playerDialogue.questions)
            {
                playerIsSpeaking = false;
            }

            CheckValidDialougeOptions();
        }
        else
        {
            foreach (Image image in selectedOptionImages)
            {
                image.color = Color.grey;
            }
        }

        SetResponseTimer();

    }

    //Check whether to block any options from selection
    private void CheckValidDialougeOptions()
    {
        switch (npcDialogue.playerResponses.Count)
        {
            case 0:
                BlockDialogueOption(0);
                BlockDialogueOption(1);
                BlockDialogueOption(2);
                BlockDialogueOption(3);
                break;
            case 1:
                BlockDialogueOption(1);
                BlockDialogueOption(2);
                BlockDialogueOption(3);
                break;
            case 2:
                BlockDialogueOption(2);
                BlockDialogueOption(3);
                break;
            case 3:
                BlockDialogueOption(3);
                break;
            default:
                break;
        }
    }

    //Change unselectable options to grey
    private void BlockDialogueOption(int i)
    {
        selectedOptionImages[i].color = Color.grey;
    }

    //set selected option to yellow
    private void HighlightSelectedOption(int i)
    {
        selectedOptionImages[i].color = Color.yellow;

        foreach (Image optionUI in selectedOptionImages)
        {
            if (optionUI != selectedOptionImages[i] && optionUI.color != Color.grey)
            {
                optionUI.color = Color.white;
            }
        }

        SetNewSprite(i);
    }

    //Set all options to white
    private void ResetHighlightedOption()
    {
        selectedDialogueOption = null;
        selectedOptionText.text = "?";

        foreach (Image optionUI in selectedOptionImages)
        {
            optionUI.color = Color.white;
        }

        ResetSprite();

        playerDialogueText.text = "...";
    }

    // Change to selected sprite for relevant option
    private void SetNewSprite(int i)
    {
        selectedOptionImages[i].sprite = pressedSprite;

        foreach (Image optionUI in selectedOptionImages)
        {
            if (optionUI != selectedOptionImages[i] && optionUI != selectedOptionImages[6])
            {
                optionUI.sprite = defaultSprite;
                //optionUI.transform.GetChild(0).transform.position = new Vector3(optionUI.transform.GetChild(0).transform.position.x, optionUI.transform.GetChild(0).transform.position.y - 5f, optionUI.transform.GetChild(0).transform.position.z);
            }
        }
    }

    //Reset sprites to unselected
    private void ResetSprite()
    {
        foreach (Image optionUI in selectedOptionImages)
        {
            if (optionUI != selectedOptionImages[6])
            {
                optionUI.sprite = defaultSprite;
                //optionUI.transform.GetChild(0).transform.position = new Vector3(optionUI.transform.GetChild(0).transform.position.x, optionUI.transform.GetChild(0).transform.position.y + 5f, optionUI.transform.GetChild(0).transform.position.z);
            }
        }
    }

    //Lock in dialogue selection
    public void LockInResponse()
    {
        playerResponseLockedIn = true;
        responseTimer = responseTimerReset;
        responseTimerUI.value = responseTimer;
        responseTimerActive = false;
        selectedOptionImages[6].enabled = false;

        pressSpaceAnimation.enabled = false;

        npc.npcEmotions.emotion = selectedDialogueOption.AffectEmotionValues(npc.npcEmotions.emotion);
        npc.npcEmotions.SetMood();

        happinessBar.value = npc.npcEmotions.emotion.happiness;
        stressBar.value = npc.npcEmotions.emotion.stress;
        shockBar.value = npc.npcEmotions.emotion.shock;


        if (selectedOptionText.text == "E") // TO DO: grey out 'E' ui if the player isnt responding & doesnt have surplus dialog topics
        {
            if (!playerIsSpeaking)
            {
                ChangeTopic();
            }
            else if (playerDialogue.questions.playerResponses.Count > 4)
            {
                ViewOtherTopics();
            }
            //else BlockDialogueOption(5);
        }

        if (selectedOptionText.text == "Q" || npcDialogue.endOfConversation)
        {
            LeaveDialogue();
        }
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

        happinessBar.maxValue = npc.npcEmotions.personality.happyMinThreshold + 10;
        happinessBar.minValue = npc.npcEmotions.personality.sadMinThreshold - 10;
        happinessBar.value = npc.npcEmotions.emotion.happiness;

        stressBar.maxValue = npc.npcEmotions.personality.angryMinThreshold + 10;
        stressBar.minValue = npc.npcEmotions.personality.sadMinThreshold - 10;
        stressBar.value = npc.npcEmotions.emotion.stress;

        shockBar.maxValue = npc.npcEmotions.personality.surprisedMinThreshold + 10;
        shockBar.minValue = npc.npcEmotions.personality.scaredMinThreshold - 10;
        shockBar.value = npc.npcEmotions.emotion.shock;

        playerIsSpeaking = true;

    }

    // Close Dialogue
    private void LeaveDialogue()
    {
        dialogueUI.SetActive(false);
        inDialogue = false;

        playerMovement.enabled = true;
        playerCam.enabled = true;

        foreach (NPCBrain npc in FindObjectsOfType<NPCBrain>())
        {
            npc.isSpeakingToPlayer = false;
        }

        //Cursor.lockState = CursorLockMode.Locked;

        //Activate Player Controller
        //Unlock Camera

        enabled = false;
    }

    // Return to initial dialogue options
    private void ChangeTopic()
    {
        // get stored inquiries depending on NPC
        npcDialogue = playerDialogue.SetPlayerQuestionsForNPC(npc, npc.npcDialogue.changeTopicDialogue[Random.Range(0, npc.npcDialogue.changeTopicDialogue.Count)]);


        //npcDialogue = playerDialogue.questions;
        //playerDialogue.questions.dialogue = npcDialogue.dialogue;

        playerIsSpeaking = true;

        Debug.Log("Changing the topic");
    }

    // scroll up and down through dialogue options
    private void ViewOtherTopics()
    {
        int numOfInquiries = playerDialogue.questions.playerResponses.Count - 1;

        if (WElementNum + 4 <= numOfInquiries)
        {
            WElementNum += 4;
        }
        else if (WElementNum - 4 >= 0)
        {
            WElementNum -= 4;
        }

        if (AElementNum + 4 <= numOfInquiries)
        {
            AElementNum += 4;
        }
        else if (AElementNum - 4 >= 1)
        {
            AElementNum -= 4;
        }

        if (DElementNum + 4 <= numOfInquiries)
        {
            DElementNum += 4;
        }
        else if (DElementNum - 4 >= 2)
        {
            DElementNum -= 4;
        }

        if (SElementNum + 4 <= numOfInquiries)
        {
            SElementNum += 4;
        }
        else if (SElementNum - 4 >= 3)
        {
            SElementNum -= 4;
        }

        Debug.Log("Viewing Other Topics");
    }
}