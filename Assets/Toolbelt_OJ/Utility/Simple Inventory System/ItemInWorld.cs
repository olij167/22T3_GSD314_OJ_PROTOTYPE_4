using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Add this script to all item prefabs, item = the respective InventoryItem scriptable object
public class ItemInWorld : MonoBehaviour
{
    public InventoryItem item;
    public AudioClip itemCollectedAudio;

    public bool hasDialogue;
    public UnlockNewDialogue unlockNewDialogue;

    private void Start()
    {
        unlockNewDialogue.enabled = false;
    }

}
