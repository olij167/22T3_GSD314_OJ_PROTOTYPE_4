using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Activate Inventory UI by pressing 'Q'
public class ToggleInventory : MonoBehaviour
{
    public KeyCode inventoryInput = KeyCode.Q;

    [SerializeField] private GameObject inventoryPanel;
    void Start()
    {
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(inventoryInput))
        {
            ToggleInventoryUI();
        }
    }

    private void ToggleInventoryUI()
    {
        if (inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);
        }
        else
        {
            inventoryPanel.SetActive(true);
        }
    }
}
