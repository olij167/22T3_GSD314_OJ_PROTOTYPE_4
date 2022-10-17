using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 //Script goes on enemy
public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private PlayerHealthOverlay playerHealth;
    [SerializeField] private float invincibilityTimeLength;
    //private float invincibilityTimerReset;
    [SerializeField] private bool invincibleAfterDamaged;
    private bool playerIsInvincible;
    public float damage;
    private float damageReset;

    private void Start()
    {
        damageReset = damage;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TakeDamage());

            if (invincibleAfterDamaged)
            {
                if (!playerIsInvincible)
                {
                    StartCoroutine(InvincibilityFrames());
                }
            }
        }
    }

    public IEnumerator InvincibilityFrames()
    {
        damage = 0;
        playerIsInvincible = true;
        Debug.Log("Player is invincible");

        yield return new WaitForSeconds(invincibilityTimeLength);

        damage = damageReset;
        playerIsInvincible = false;
        Debug.Log("Player is vulnerable");
    }

    public IEnumerator TakeDamage()
    {
        playerHealth.health -= damage;
        playerHealth.ResetDisplayTimer();
        playerHealth.displayDamageOverlay = true;

        yield return null;
    }
}
