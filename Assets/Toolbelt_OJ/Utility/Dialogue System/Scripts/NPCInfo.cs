using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("DialogueSystem/NPCInfo"))]

[System.Serializable]
public class NPCInfo : ScriptableObject
{
    public string npcName;
    public string npcGender;

    public string npcProfileID;

    public NPCEmotions npcEmotions;

    public NPCDialogue npcDialogue;

    public NPCDialogueOption RespondBasedOnMood(PlayerDialogueOption playerDialogueInput)
    {
        for (int i = 0; i < npcDialogue.dialogueConnections.Count; i++) //cycle through dialogue options
        {
            if (npcDialogue.dialogueConnections[i].playerDialogueInput == playerDialogueInput) //find current player dialogue
            {
                for (int d = 0; d < npcDialogue.dialogueConnections[i].npcResponses.Count; d++) //scroll through current dialogue responses
                {
                    if (npcDialogue.dialogueConnections[i].npcResponses[d].npcMood == npcEmotions.GetStrongestEmotion())
                    {
                        return npcDialogue.dialogueConnections[i].npcResponses[d].response;
                    }
                }
            }
        }

        return RespondBasedOnClosestMood(playerDialogueInput);
    }

    public NPCDialogueOption RespondBasedOnClosestMood(PlayerDialogueOption playerDialogueInput)
    {

        for (int i = 0; i < npcDialogue.dialogueConnections.Count; i++) //cycle through dialogue options
        {
            if (npcDialogue.dialogueConnections[i].playerDialogueInput == playerDialogueInput) //find current player dialogue
            {
                for (int d = 0; d < npcDialogue.dialogueConnections[i].npcResponses.Count; d++) //scroll through current dialogue responses
                {
                    if (npcDialogue.dialogueConnections[i].npcResponses[d].npcMood == GetClosestEmotionWithDialogue(playerDialogueInput, npcEmotions.GetMood()))
                    {
                        return npcDialogue.dialogueConnections[i].npcResponses[d].response;
                    }
                }
            }
        }

        return default;
    }

    public NPCEmotions.Mood GetClosestEmotionWithDialogue(PlayerDialogueOption playerDialogueInput, NPCEmotions.Mood currentMood)
    {
        List<NPCEmotions.Mood> availableMoodDialogue = new List<NPCEmotions.Mood>();

        for (int i = 0; i < npcDialogue.dialogueConnections.Count; i++)
        {
            if (npcDialogue.dialogueConnections[i].playerDialogueInput == playerDialogueInput)
            {
                foreach (NPCDialogue.Responses responses in npcDialogue.dialogueConnections[i].npcResponses)
                {
                    if (!availableMoodDialogue.Contains(responses.npcMood))
                    {
                        availableMoodDialogue.Add(responses.npcMood);
                    }
                }
            }
        }
        
        switch(currentMood)
        {
            case NPCEmotions.Mood.alert:
                if (npcEmotions.emotion.happiness > npcEmotions.emotion.stress)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.happy))
                        return NPCEmotions.Mood.happy;
                    else break;
                }
                else if (npcEmotions.emotion.happiness < npcEmotions.emotion.stress)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.angry))
                        return NPCEmotions.Mood.angry;
                    else break;
                }
                else
                {
                    int rand = Random.Range(0, 1);
                    if (rand == 0)
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.happy))
                            return NPCEmotions.Mood.happy;
                        else break;
                    }
                    else
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.angry))
                            return NPCEmotions.Mood.angry;
                        else break;
                    }
                }

            case NPCEmotions.Mood.excited:
                if (npcEmotions.emotion.happiness > Mathf.Abs(npcEmotions.emotion.stress))
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.happy))
                        return NPCEmotions.Mood.happy;
                    else break;
                }
                else if (npcEmotions.emotion.happiness < Mathf.Abs(npcEmotions.emotion.stress))
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.nervous))
                        return NPCEmotions.Mood.nervous;
                    else break;
                }
                else
                {
                    int rand = Random.Range(0, 1);
                    if (rand == 0)
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.happy))
                            return NPCEmotions.Mood.happy;
                        else break;
                    }
                    else
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.nervous))
                            return NPCEmotions.Mood.nervous;
                        else break;
                    }
                }

            case NPCEmotions.Mood.delighted:
                if (npcEmotions.emotion.happiness > npcEmotions.emotion.shock)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.happy))
                        return NPCEmotions.Mood.happy;
                    else break;
                }
                else if (npcEmotions.emotion.happiness < npcEmotions.emotion.shock)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.surprised))
                        return NPCEmotions.Mood.surprised;
                    else break;
                }
                else
                {
                    int rand = Random.Range(0, 1);
                    if (rand == 0)
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.happy))
                            return NPCEmotions.Mood.happy;
                        else break;
                    }
                    else
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.surprised))
                            return NPCEmotions.Mood.surprised;
                        else break;
                    }
                }

            case NPCEmotions.Mood.brave:
                if (npcEmotions.emotion.happiness > Mathf.Abs(npcEmotions.emotion.shock))
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.happy))
                        return NPCEmotions.Mood.happy;
                    else break;
                }
                else if (npcEmotions.emotion.happiness < Mathf.Abs(npcEmotions.emotion.shock))
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.scared))
                        return NPCEmotions.Mood.scared;
                    else break;
                }
                else
                {
                    int rand = Random.Range(0, 1);
                    if (rand == 0)
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.happy))
                            return NPCEmotions.Mood.happy;
                        else break;
                    }
                    else
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.scared))
                            return NPCEmotions.Mood.scared;
                        else break;
                    }
                }

            case NPCEmotions.Mood.grumpy:
                if (Mathf.Abs(npcEmotions.emotion.happiness) > npcEmotions.emotion.stress)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.sad))
                        return NPCEmotions.Mood.sad;
                    else break;
                }
                else if (Mathf.Abs(npcEmotions.emotion.happiness) < npcEmotions.emotion.stress)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.angry))
                        return NPCEmotions.Mood.angry;
                    else break;
                }
                else
                {
                    int rand = Random.Range(0, 1);
                    if (rand == 0)
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.sad))
                            return NPCEmotions.Mood.sad;
                        else break;
                    }
                    else
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.angry))
                            return NPCEmotions.Mood.angry;
                        else break;
                    }
                }

            case NPCEmotions.Mood.worried:
                if (npcEmotions.emotion.happiness < npcEmotions.emotion.stress)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.sad))
                        return NPCEmotions.Mood.sad;
                    else break;
                }
                else if (npcEmotions.emotion.happiness > npcEmotions.emotion.stress)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.nervous))
                        return NPCEmotions.Mood.nervous;
                    else break;
                }
                else
                {
                    int rand = Random.Range(0, 1);
                    if (rand == 0)
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.sad))
                            return NPCEmotions.Mood.sad;
                        else break;
                    }
                    else
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.nervous))
                            return NPCEmotions.Mood.nervous;
                        else break;
                    }
                }

            case NPCEmotions.Mood.dismayed:
                if (Mathf.Abs(npcEmotions.emotion.happiness) > npcEmotions.emotion.shock)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.sad))
                        return NPCEmotions.Mood.sad;
                    else break;
                }
                else if (Mathf.Abs(npcEmotions.emotion.happiness) < npcEmotions.emotion.shock)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.surprised))
                        return NPCEmotions.Mood.surprised;
                    else break;
                }
                else
                {
                    int rand = Random.Range(0, 1);
                    if (rand == 0)
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.sad))
                            return NPCEmotions.Mood.sad;
                        else break;
                    }
                    else
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.surprised))
                            return NPCEmotions.Mood.surprised;
                        else break;
                    }
                }

            case NPCEmotions.Mood.fearful:
                if (npcEmotions.emotion.happiness < npcEmotions.emotion.shock)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.sad))
                        return NPCEmotions.Mood.sad;
                    else break;
                }
                else if (npcEmotions.emotion.happiness > npcEmotions.emotion.shock)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.scared))
                        return NPCEmotions.Mood.scared;
                    else break;
                }
                else
                {
                    int rand = Random.Range(0, 1);
                    if (rand == 0)
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.sad))
                            return NPCEmotions.Mood.sad;
                        else break;
                    }
                    else
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.scared))
                            return NPCEmotions.Mood.scared;
                        else break;
                    }
                }

            case NPCEmotions.Mood.startled:
                if (npcEmotions.emotion.stress > npcEmotions.emotion.shock)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.angry))
                        return NPCEmotions.Mood.angry;
                    else break;
                }
                else if (npcEmotions.emotion.stress < npcEmotions.emotion.shock)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.surprised))
                        return NPCEmotions.Mood.surprised;
                    else break;
                }
                else
                {
                    int rand = Random.Range(0, 1);
                    if (rand == 0)
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.angry))
                            return NPCEmotions.Mood.angry;
                        else break;
                    }
                    else
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.surprised))
                            return NPCEmotions.Mood.surprised;
                        else break;
                    }
                }

            case NPCEmotions.Mood.upset:
                if (npcEmotions.emotion.stress > Mathf.Abs(npcEmotions.emotion.shock))
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.angry))
                        return NPCEmotions.Mood.angry;
                    else break;
                }
                else if (npcEmotions.emotion.stress < Mathf.Abs(npcEmotions.emotion.shock))
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.scared))
                        return NPCEmotions.Mood.scared;
                    else break;
                }
                else
                {
                    int rand = Random.Range(0, 1);
                    if (rand == 0)
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.angry))
                            return NPCEmotions.Mood.angry;
                        else break;
                    }
                    else
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.scared))
                            return NPCEmotions.Mood.scared;
                        else break;
                    }
                }

            case NPCEmotions.Mood.alarmed:
                if (Mathf.Abs(npcEmotions.emotion.stress) > npcEmotions.emotion.shock)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.nervous))
                        return NPCEmotions.Mood.nervous;
                    else break;
                }
                else if (Mathf.Abs(npcEmotions.emotion.happiness) < npcEmotions.emotion.shock)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.surprised))
                        return NPCEmotions.Mood.surprised;
                    else break;
                }
                else
                {
                    int rand = Random.Range(0, 1);
                    if (rand == 0)
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.nervous))
                            return NPCEmotions.Mood.nervous;
                        else break;
                    }
                    else
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.surprised))
                            return NPCEmotions.Mood.surprised;
                        else break;
                    }
                }

            case NPCEmotions.Mood.horrified:
                if (npcEmotions.emotion.stress < npcEmotions.emotion.shock)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.nervous))
                        return NPCEmotions.Mood.nervous;
                    else break;
                }
                else if (npcEmotions.emotion.happiness > npcEmotions.emotion.shock)
                {
                    if (availableMoodDialogue.Contains(NPCEmotions.Mood.scared))
                        return NPCEmotions.Mood.scared;
                    else break;
                }
                else
                {
                    int rand = Random.Range(0, 1);
                    if (rand == 0)
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.nervous))
                            return NPCEmotions.Mood.nervous;
                        else break;
                    }
                    else
                    {
                        if (availableMoodDialogue.Contains(NPCEmotions.Mood.scared))
                            return NPCEmotions.Mood.scared;
                        else break;
                    }
                }
            case NPCEmotions.Mood.overwhelmed:
                return availableMoodDialogue[Random.Range(0, availableMoodDialogue.Count)];
        }

        return NPCEmotions.Mood.calm;
    }

    public NPCEmotions.Mood CompareEmotionValues(NPCEmotions.NPCFeelings emotions, NPCEmotions.Personality personality)
    {
        if (emotions.happiness < personality.sadMinThreshold || emotions.happiness > personality.happyMinThreshold || emotions.stress < personality.nervousMinThreshold || emotions.stress > personality.angryMinThreshold || emotions.shock < personality.scaredMinThreshold || emotions.shock > personality.surprisedMinThreshold)
        {
            if (Mathf.Abs(emotions.happiness) > Mathf.Abs(emotions.stress) && Mathf.Abs(emotions.happiness) > Mathf.Abs(emotions.shock))
            {
                if (emotions.happiness >= personality.happyMinThreshold)
                {
                    return NPCEmotions.Mood.happy;
                }
                else if (emotions.happiness <= personality.sadMinThreshold)
                {
                    return NPCEmotions.Mood.sad;
                }

            }
            else if (Mathf.Abs(emotions.stress) > Mathf.Abs(emotions.happiness) && Mathf.Abs(emotions.stress) > Mathf.Abs(emotions.shock))
            {
                if (emotions.stress >= personality.angryMinThreshold)
                {
                    return NPCEmotions.Mood.angry;
                }
                else if (emotions.stress <= personality.nervousMinThreshold)
                {
                    return NPCEmotions.Mood.nervous;
                }
            }
            else if (Mathf.Abs(emotions.shock) > Mathf.Abs(emotions.happiness) && Mathf.Abs(emotions.shock) > Mathf.Abs(emotions.stress))
            {
                if (emotions.shock >= personality.surprisedMinThreshold)
                {
                    return NPCEmotions.Mood.surprised;
                }
                else if (emotions.shock <= personality.scaredMinThreshold)
                {
                    return NPCEmotions.Mood.scared;
                }
            }
        }

        return NPCEmotions.Mood.calm;
    }


}
