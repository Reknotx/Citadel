/*
 * Author: Andrew Nave
 * Date: 9/02/2021
 * 
 * Brief: This script manages the values for the player's health and mana and 
 * fills the respective bars accordingly
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManaHandler : MonoBehaviour
{

    public Image healthBar;
    public Image manaBar;
    public Text healthText;
    public Text manaText;

    public float myLife;
    public float myMana;

    private float currentLife;
    private float currentMana;
    private float calculateLife;
    private float calculateMana;

    /// <summary>
    /// Sets life and mana to their base values when the scene loads in.
    /// </summary>
    void Start()
    {
        currentLife = myLife;
        currentMana = myMana;
    }

    /// <summary>
    /// Updates the health and mana bars to their current state, depending on if spells have been cast or if the player has taken damage.
    /// </summary>
    void Update()
    {
        calculateLife = currentLife / myLife;
        healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, calculateLife, Time.deltaTime);
        healthText.text = "" + (int)currentLife;

        calculateMana = currentMana / myMana;
        manaBar.fillAmount = Mathf.MoveTowards(manaBar.fillAmount, calculateMana, Time.deltaTime);
        manaText.text = "" + (int)currentMana;
    }

    /// <param name="damage">Amount of damage taken by the player</param>
    public void Damage(float damage)
    {
        currentLife -= damage;
    }

    /// <param name="mana">Amount of mana lost by the player for casting a spell</param>
    public void ReduceMana(float mana)
    {
        currentMana -= mana;
    }
}
