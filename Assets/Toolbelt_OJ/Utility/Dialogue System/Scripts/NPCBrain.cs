using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBrain : MonoBehaviour
{
    public NPCInfo npcInfo;

    public NPCEmotions npcEmotions;

    public NPCDialogue npcDialogue;

    public bool isSpeakingToPlayer;


    public void Start()
    {
        
    }

    [System.Serializable]
    public struct NPCInfoSerialized
    {
        public NPCInfo npcInfo;
        
        public string npcProfileID;
        public string npcName;
        public string npcGender;

        public NPCEmotions npcEmotions;

        public NPCDialogue npcDialogue;
    }

    public NPCInfoSerialized serializedNPCInfo;

    public void SerializeNPCInfo(NPCInfo npc)
    {
        serializedNPCInfo.npcInfo = npc;
        serializedNPCInfo.npcProfileID = npc.npcProfileID;
        serializedNPCInfo.npcName = npc.npcName;
        serializedNPCInfo.npcGender = npc.npcGender;
        serializedNPCInfo.npcEmotions = npc.npcEmotions;
        serializedNPCInfo.npcDialogue = npc.npcDialogue;
    }

    public void SpeakingToPlayer()
    {
        transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
    }


}
