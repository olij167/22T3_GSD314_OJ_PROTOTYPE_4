using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthOverlay : MonoBehaviour
{
    public float health = 25f;
    public float healthRegenSpeed;
    private float maxHealth, halfHealth, quarterHealth;
    //[SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image damageOverlay;
    [SerializeField] private float damageOverlayDisplayTime = 10f;
    private float damageOverlayDisplayTimeReset;
    public bool displayDamageOverlay;

    [SerializeField] private TextMeshProUGUI endText, endSubText;
    [SerializeField] private GameObject endPanel;

    
    void Start()
    {
        damageOverlayDisplayTimeReset = damageOverlayDisplayTime;
        damageOverlay.enabled = false;
        endPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;

        healthRegenSpeed = health / 100;

        maxHealth = health;
        halfHealth = health / 2;
        quarterHealth = health / 4;

        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (displayDamageOverlay)
        {
            if (health > halfHealth)
            {
                DisplayDamageOverlay(0.15f);
            }
            else if (health < halfHealth && health > quarterHealth)
            {
                DisplayDamageOverlay(0.30f);
            }
            else if ( health < quarterHealth && health > 0)
            {
                DisplayDamageOverlay(0.45f);
            }
            else
            {
                DisplayDamageOverlay(0.6f);
                LoseGameUI();
            }


            //switch (health)
            //{
            //    case > 20:
            //        DisplayDamageOverlay(0.15f);
            //        break;
            //    case > 10:
            //        DisplayDamageOverlay(0.30f);
            //        break;
            //    case > 5:
            //        DisplayDamageOverlay(0.45f);
            //        break;
            //    case 0:
            //        DisplayDamageOverlay(0.6f);
            //        LoseGameUI();

            //        break;
            //}
        }
    }

    void DisplayDamageOverlay(float maxAlpha)
    {
        if (displayDamageOverlay)
        {
            damageOverlay.enabled = true;
            damageOverlayDisplayTime -= Time.deltaTime;

            health = Mathf.Lerp(health, maxHealth, Time.deltaTime * healthRegenSpeed);

            if (damageOverlayDisplayTime >= damageOverlayDisplayTimeReset / 2)
            {
                damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, Mathf.Lerp(0f, maxAlpha, damageOverlayDisplayTimeReset - damageOverlayDisplayTime));

            }

            if (damageOverlayDisplayTime <= damageOverlayDisplayTimeReset / 2)
            {
                damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, Mathf.Lerp(0f, maxAlpha, damageOverlayDisplayTime));
            }

            if (damageOverlayDisplayTime <= 0f)
            {
                health = maxHealth;
                damageOverlay.enabled = false;
                damageOverlayDisplayTime = damageOverlayDisplayTimeReset;
                displayDamageOverlay = false;
            }
        }
    }

    public void ResetDisplayTimer()
    {
        damageOverlayDisplayTime = damageOverlayDisplayTimeReset;
    }

    public void LoseGameUI()
    {
        endPanel.SetActive(true);
        endText.text = "You were caught!";
        endSubText.text = "...ouch";

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }
}
