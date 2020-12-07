using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController GC;

    private UserInput player { get { return FindObjectOfType<UserInput>(); } set { player = value; } }

    private PlayerUI playerUI { get { return FindObjectOfType<PlayerUI>(); } set { playerUI = value; } }

    private WeaponHandler wp { get { return player.GetComponent<WeaponHandler>(); } set { wp = value; } }

    private Stats stats { get { return player.GetComponent<Stats>(); } set { stats = value; } }
    void Awake()
    {
        if (GC == null)
        {
            GC = this;
        }
        else
        {
            if (GC != this)
            {
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (player)
        {
            if (playerUI)
            {
                if (wp)
                {
                    if (playerUI.ammoText)
                    {
                        if (wp.currentWeapon == null)
                        {
                            playerUI.ammoText.text = "Unarmed.";
                        }
                        else
                        {
                            playerUI.ammoText.text = wp.currentWeapon.Ammo.clipAmmo + "//" + wp.currentWeapon.Ammo.carryingAmmo;
                        }
                    }
                }

               if(stats)
               {
                   if(playerUI.healthBar)
                   {
                       playerUI.healthText.text = stats.health.ToString();
                   }
               }
            }
        }
    }
}
