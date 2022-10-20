using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCGenerator : MonoBehaviour
{
    public List<NPCInfo> npcProfiles;

    public NameList maleNamesList, femaleNamesList;
    private List<string> nbNamesList;

    public GameObject npcPrefab;

    public bool spawnRandomProfiles;

    public int numToSpawn;
    public int numToSpawnPerProfile;

    [SerializeField] private PlayerDialogue playerDialogue;

    private void Start()
    {
        nbNamesList = new List<string>();

        foreach(string name in maleNamesList.nameList)
        {
            nbNamesList.Add(name);
        }

        foreach (string name in femaleNamesList.nameList)
        {
            nbNamesList.Add(name);
        }

        if (spawnRandomProfiles)
        {
            SpawnRandomNPCs(numToSpawn);
        }
        else
        {
            SpawnNPCsPerProfile(numToSpawnPerProfile);
        }
    }

    public void SpawnRandomNPCs(int numToSpawn)
    {
        List<NPCInfo> newNPCInfoList = new List<NPCInfo>();

        for (int i = 0; i < numToSpawn; i++)
        {
            Vector3 spawnPos = GenerateRandomWayPoint();
            GameObject newNPC = Instantiate(npcPrefab, spawnPos, Quaternion.identity);
            newNPC.transform.parent = transform;

            NPCInfo newInfo = SetRandomNPCInfo();

            newNPC.GetComponent<NPCBrain>().npcInfo = newInfo;

            RandomiseColour(newNPC);

            newNPC.GetComponent<NPCBrain>().SerializeNPCInfo(newInfo);

            newNPCInfoList.Add(newInfo);

        }

        if (newNPCInfoList.Count == numToSpawn)
        {
            AddNPCsToPlayerDialogue(newNPCInfoList);
        }
    }

    public void SpawnNPCsPerProfile(int numPerProfile)
    {

        List<NPCInfo> newNPCInfoList = new List<NPCInfo>();

        for (int x = 0; x < npcProfiles.Count; x++)
        {
            for (int i = 0; i < numPerProfile; i++)
            {
                Vector3 spawnPos = GenerateRandomWayPoint();
                GameObject newNPC = Instantiate(npcPrefab, spawnPos, Quaternion.identity);
                newNPC.transform.parent = transform;

                NPCInfo newInfo = SetProfileNPCInfo(npcProfiles[x]);

                newNPC.GetComponent<NPCBrain>().npcInfo = newInfo;

                RandomiseColour(newNPC);

                newNPC.GetComponent<NPCBrain>().SerializeNPCInfo(newInfo);

                //playerDialogue.GenerateQuestionsForRandomNPCs(newInfo);
                newNPCInfoList.Add(newInfo);

                //AddRandomNPCsToPlayerDialogue(newNPCInfoList);

            }
        }

        if (newNPCInfoList.Count == numPerProfile * npcProfiles.Count)
        {
            AddNPCsToPlayerDialogue(newNPCInfoList);
        }
    }

    public NPCInfo SetRandomNPCInfo()
    {
        // create new profile
        NPCInfo npcProfile = ScriptableObject.CreateInstance <NPCInfo>();

        int r = Random.Range(0, 2);

        // set gender & name
        switch (r)
        {
            case 0:
                {
                    npcProfile.npcGender = "Male";
                    npcProfile.npcName = maleNamesList.nameList[Random.Range(0, maleNamesList.nameList.Count)];
                    break;
                }
            case 1:
                {
                    npcProfile.npcGender = "Female";
                    npcProfile.npcName = femaleNamesList.nameList[Random.Range(0, femaleNamesList.nameList.Count)];
                    break;
                }
            case 2:
                {
                    npcProfile.npcGender = "Non-binary";
                    npcProfile.npcName = nbNamesList[Random.Range(0, nbNamesList.Count)];
                    break;
                }
        } 

        int randProfile = Random.Range(0, npcProfiles.Count);

        // set personality
        npcProfile.npcEmotions = npcProfiles[randProfile].npcEmotions;

        // set dialogue trees
        npcProfile.npcDialogue = npcProfiles[randProfile].npcDialogue;

        npcProfile.npcProfileID = npcProfiles[randProfile].npcProfileID;

        npcProfile.name = npcProfile.npcName;

        return npcProfile;
    }
    
    public NPCInfo SetProfileNPCInfo(NPCInfo npcProfile)
    {
        // create new profile
        //npcProfile = ScriptableObject.CreateInstance <NPCInfo>();
        NPCInfo newProfile = ScriptableObject.CreateInstance<NPCInfo>();

        int r = Random.Range(0, 2);

        // set gender & name
        switch (r)
        {
            case 0:
                {
                    newProfile.npcGender = "Male";
                    newProfile.npcName = maleNamesList.nameList[Random.Range(0, maleNamesList.nameList.Count)];
                    break;
                }
            case 1:
                {
                    newProfile.npcGender = "Female";
                    newProfile.npcName = femaleNamesList.nameList[Random.Range(0, femaleNamesList.nameList.Count)];
                    break;
                }
            case 2:
                {
                    newProfile.npcGender = "Non-binary";
                    newProfile.npcName = nbNamesList[Random.Range(0, nbNamesList.Count)];
                    break;
                }
        } 

        //int randProfile = Random.Range(0, npcProfiles.Count);

        // set personality
        newProfile.npcEmotions = npcProfile.npcEmotions;

        // set dialogue trees
        newProfile.npcDialogue = npcProfile.npcDialogue;

        newProfile.npcProfileID = npcProfile.npcProfileID;

        newProfile.name = npcProfile.npcName;

        return newProfile;
    }

    public void RandomiseColour(GameObject npc)
    {
        Color npcColour = Random.ColorHSV(0f, 1f, 0.5f, 0.75f, 0.75f, 1f, 1f, 1f);

        //foreach(GameObject child in npc.transform)
        //{
        //    if (child.GetComponent<MeshRenderer>())
        //    {
        //        child.GetComponent<MeshRenderer>().material.color = npcColour;
        //    }
        //}

        for (int i = 0; i < npc.transform.childCount; i++)
        {
            if (npc.transform.GetChild(i).GetComponent<MeshRenderer>())
            {
                npc.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = npcColour;
            }
        }
    }

    public void AddNPCsToPlayerDialogue(List<NPCInfo> npcInfoList)
    {
        playerDialogue.playerQuestions.Clear();

        for (int i = 0; i < npcInfoList.Count; i++)
        {
            // Add random NPC's & Questions to new list
            PlayerDialogue.PlayerQuestions newPlayerQuestions = new PlayerDialogue.PlayerQuestions();

            newPlayerQuestions.npc = npcInfoList[i];

            //Debug.Log("Generating questions for: " + npcInfoList[i].npcName);


            playerDialogue.playerQuestions.Add(newPlayerQuestions);

            
        }

        //playerDialogue.AddDialogueOptions();
    } 
    

    

    public Vector3 GenerateRandomWayPoint()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        int maxIndices = navMeshData.indices.Length - 3;

        // pick the first indice of a random triangle in the nav mesh
        int firstVertexSelected = UnityEngine.Random.Range(0, maxIndices);
        int secondVertexSelected = UnityEngine.Random.Range(0, maxIndices);

        // spawn on verticies
        Vector3 point = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];

        Vector3 firstVertexPosition = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];
        Vector3 secondVertexPosition = navMeshData.vertices[navMeshData.indices[secondVertexSelected]];

        // eliminate points that share a similar X or Z position to stop spawining in square grid line formations
        if ((int)firstVertexPosition.x == (int)secondVertexPosition.x || (int)firstVertexPosition.z == (int)secondVertexPosition.z)
        {
            point = GenerateRandomWayPoint(); // re-roll a position - I'm not happy with this recursion it could be better
        }
        else
        {
            // select a random point on it
            point = Vector3.Lerp(firstVertexPosition, secondVertexPosition, UnityEngine.Random.Range(0.05f, 0.95f));
        }

        return point;
    }
}
