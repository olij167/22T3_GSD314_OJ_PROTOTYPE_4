using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu (menuName = "DialogueSystem/PlayerDialogueOption")]
public class PlayerDialogueOption : ScriptableObject
{
    [TextArea(3, 10)]
    public string dialogue;

    public float happinessEffect, stressEffect, shockEffect;

    //public float npcWillRemember;

    public bool isGoodbyeOption, isChangeTopicOption, hasConditionalEvent;

    public List<ConditionalEvent> conditionalEvents;

    public NPCEmotions.NPCFeelings AffectEmotionValues(NPCEmotions.NPCFeelings npcEmotions)
    {
        npcEmotions.happiness += happinessEffect;
        npcEmotions.stress += stressEffect;
        npcEmotions.shock += shockEffect;

        Debug.Log("Emotions have been affected");

        return npcEmotions;
    }

    public void Debugger()
    {
        Debug.Log("Conditional Event has been invoked!");
    }
}
