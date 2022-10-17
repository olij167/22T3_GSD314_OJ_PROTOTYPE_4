using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "DialogueSystem/PlayerDialogueOption")]
public class PlayerDialogueOption : ScriptableObject
{
    [TextArea(3, 10)]
    public string dialogue;

    //public PlayerDialogue.PlayerQuestions npcResponses;
    //public NPCDialogueOption npcResponse;

    //public NPCEmotions.NPCFeelings emotionEffects;
    public float happinessEffect, stressEffect, shockEffect;

    //public float npcWillRemember;

    public bool isGoodbyeOption, isChangeTopicOption;

    public NPCEmotions.NPCFeelings AffectEmotionValues(NPCEmotions.NPCFeelings npcEmotions)
    {
        npcEmotions.happiness += happinessEffect;
        npcEmotions.stress += stressEffect;
        npcEmotions.shock += shockEffect;

        Debug.Log("Emotions have been affected");

        return npcEmotions;
    }
}
