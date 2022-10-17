using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//To create: Right click in project window -> Create -> Inventory item
[CreateAssetMenu(menuName ="InventoryItem")]
public class InventoryItem : ScriptableObject
{
    //the inventory item model
    public GameObject prefab;
    public string itemName;
    public int numCarried;

}
