using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Toolbelt_OJ;

public class LoseGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI endText, endSubText;
    [SerializeField] private GameObject endPanel;

    public void LoseGameUI()
    {
        Time.timeScale = 0;
        endPanel.SetActive(true);
        endText.text = "You were caught!";
        endSubText.text = "...ouch";
    }
}
