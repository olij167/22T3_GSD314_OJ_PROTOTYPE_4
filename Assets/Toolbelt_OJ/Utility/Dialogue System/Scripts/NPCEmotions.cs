using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCEmotions
{
    [System.Serializable]
    public struct Personality
    {
        public float happyMinThreshold, sadMinThreshold, angryMinThreshold, nervousMinThreshold, surprisedMinThreshold, scaredMinThreshold;
        public Personality(float happyMinThreshold, float sadMinThreshold, float angryMinThreshold, float nervousMinThreshold, float surprisedMinThreshold, float scaredMinThreshold)
        {
            this.happyMinThreshold = happyMinThreshold; 
            this.sadMinThreshold = sadMinThreshold;
            this.angryMinThreshold = angryMinThreshold;
            this.nervousMinThreshold = nervousMinThreshold;
            this.surprisedMinThreshold = surprisedMinThreshold;
            this.scaredMinThreshold = -scaredMinThreshold;
        }
    }

    public Personality personality;
    

    [System.Serializable]
    public enum Mood { calm, happy, sad, angry, nervous, scared, surprised, alert, excited, delighted, 
        brave, grumpy, worried, dismayed, fearful, startled, upset, alarmed, horrified, overwhelmed }

    [System.Serializable]
    public struct NPCFeelings
    {
        public NPCEmotions.Mood npcMood;


        public float happiness;
        public float stress;
        public float shock;

        public NPCFeelings(Mood npcMood, float happiness, float stress, float shock)
        {
            this.npcMood = npcMood;

            this.happiness = happiness;// happy & sad emotions
            this.stress = stress; // angry and nervous emotions
            this.shock = shock; // surprised & scared emotions
        }
    }

    public NPCFeelings emotion;

    public Mood GetMood() 
    {
        SetMood();
        return GetStrongestEmotion(); 
    }

    public void SetMood()
    {
        emotion.npcMood = GetStrongestEmotion();
        //Debug.Log("Mood has been set");
    }

    public Mood GetStrongestEmotion()
    {
        if (CheckEmotionThreshold(emotion.happiness, personality.happyMinThreshold))
        {
            if (CheckEmotionThreshold(emotion.stress, personality.angryMinThreshold))
            {
                if (CheckEmotionThreshold(emotion.shock, personality.surprisedMinThreshold))
                {
                    //happy & angry & surprised
                    return Mood.overwhelmed;
                }
                else if (CheckEmotionThreshold(emotion.shock, personality.scaredMinThreshold))
                {
                    //happy & angry & scared
                    return Mood.overwhelmed;

                }
                // happy & angry
                return Mood.alert;
            }
            else if (CheckEmotionThreshold(emotion.stress, personality.nervousMinThreshold))
            {
                if (CheckEmotionThreshold(emotion.shock, personality.surprisedMinThreshold))
                {
                    //happy & nervous & surprised
                    return Mood.overwhelmed;

                }
                else if (CheckEmotionThreshold(emotion.shock, personality.scaredMinThreshold))
                {
                    //happy & nervous & scared
                    return Mood.overwhelmed;

                }
                // happy & nervous
                return Mood.excited;

            }
            if (CheckEmotionThreshold(emotion.shock, personality.surprisedMinThreshold))
            {
                // happy & surprised
                return Mood.delighted;
            }
            else if (CheckEmotionThreshold(emotion.shock, personality.scaredMinThreshold))
            {
                // happy & scared
                return Mood.brave;
            }

            // just happy
            return Mood.happy;
        }
        else if (CheckEmotionThreshold(emotion.happiness, personality.sadMinThreshold))
        {
            if (CheckEmotionThreshold(emotion.stress, personality.angryMinThreshold))
            {
                if (CheckEmotionThreshold(emotion.shock, personality.surprisedMinThreshold))
                {
                    //sad & angry & surprised
                    return Mood.overwhelmed;

                }
                else if (CheckEmotionThreshold(emotion.shock, personality.scaredMinThreshold))
                {
                    //sad & angry & scared
                    return Mood.overwhelmed;


                }
                // sad & angry
                return Mood.grumpy;
            }
            else if (CheckEmotionThreshold(emotion.stress, personality.nervousMinThreshold))
            {
                if (CheckEmotionThreshold(emotion.shock, personality.surprisedMinThreshold))
                {
                    //sad & nervous & surprised
                    return Mood.overwhelmed;


                }
                else if (CheckEmotionThreshold(emotion.shock, personality.scaredMinThreshold))
                {
                    //sad & nervous & scared
                    return Mood.overwhelmed;


                }
                // sad & nervous
                return Mood.worried;

            }
            if (CheckEmotionThreshold(emotion.shock, personality.surprisedMinThreshold))
            {
                // sad & surprised
                return Mood.dismayed;
            }
            else if (CheckEmotionThreshold(emotion.shock, personality.scaredMinThreshold))
            {
                // sad & scared
                return Mood.fearful;
            }
            // just sad
            return Mood.sad;
        }
        else if (CheckEmotionThreshold(emotion.stress, personality.angryMinThreshold))
        {
            if (CheckEmotionThreshold(emotion.shock, personality.surprisedMinThreshold))
            {
                // angry and surprised
                return Mood.startled;
            }
            else if (CheckEmotionThreshold(emotion.shock, personality.scaredMinThreshold))
            {
                //angry and scared
                return Mood.upset;
            }
            // just angry
            return Mood.angry;
        }
        else if (CheckEmotionThreshold(emotion.stress, personality.nervousMinThreshold))
        {
            if (CheckEmotionThreshold(emotion.shock, personality.surprisedMinThreshold))
            {
                // nervous and surprised
                return Mood.alarmed;
            }
            else if (CheckEmotionThreshold(emotion.shock, personality.scaredMinThreshold))
            {
                //nervous and scared
                return Mood.horrified;
            }
            //just nervous
            return Mood.nervous;
        }
        else if (CheckEmotionThreshold(emotion.shock, personality.surprisedMinThreshold))
        {
            // just surprised
            return Mood.surprised;
        }
        else if (CheckEmotionThreshold(emotion.shock, personality.scaredMinThreshold))
        {
            //just scared
            return Mood.scared;
        }
        
        //no strong emotions
        return Mood.calm;
    }

    public bool CheckEmotionThreshold(float emotion, float emotionThreshold)
    {
        if (emotionThreshold > 0)
        {
            if (emotion > emotionThreshold)
            {
                return true;
            }
        }
        else
        {
            if (emotion < emotionThreshold)
            {
                return true;
            }
        }

        return false;
    }
}