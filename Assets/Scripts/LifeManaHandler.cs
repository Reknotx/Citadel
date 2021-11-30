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
    /// <summary>
    /// Image holding the UI for player's health bar
    /// </summary>
    public Image healthBar;

    /// <summary>
    /// Image holding the UI for the player's mana bar
    /// </summary>
    public Image manaBar;

    /// <summary>
    /// Text displayed in player's health bar
    /// </summary>
    public Text healthText;

    /// <summary>
    /// Text displayed in player's mana bar
    /// </summary>
    public Text manaText;

    /// <summary>
    /// The player's maximum health pool
    /// </summary>
    public float myLife;

    /// <summary>
    /// The player's maximum mana pool
    /// </summary>
    public float myMana;

    /// <summary>
    /// The amount of health the player currently has
    /// </summary>
    public float currentLife;

    /// <summary>
    /// The amount of mana the player currently has
    /// </summary>
    private float currentMana;

    /// <summary>
    /// value that holds the ratio of the player's max health vs. how much they currently have
    /// </summary>
    private float calculateLife;

    /// <summary>
    /// value that holds the ratio of the player's max mana vs. how much they currently have
    /// </summary>
    private float calculateMana;

    /// <summary>
    /// Sets life and mana to their base values when the scene loads in.
    /// </summary>

    public GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentLife = player.GetComponent<NewPlayer>().Health;
        currentMana = player.GetComponent<NewPlayer>().Mana;
        myLife = player.GetComponent<NewPlayer>().MaxHealth;
        myMana = player.GetComponent<NewPlayer>().Mana;
    }

    /// <summary>
    /// Updates the health and mana bars to their current state, depending on if spells have been cast or if the player has taken damage.
    /// </summary>
    void FixedUpdate()
    {
        ///Calculate the player's current life and display the proper amount in UI
        calculateLife = player.GetComponent<NewPlayer>().Health / player.GetComponent<NewPlayer>().MaxHealth;
        healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, calculateLife, Time.deltaTime);
        healthText.text = "" + (int)player.GetComponent<NewPlayer>().Health;

        ///Calculate the player's current mana and display the proper amount in UI
        calculateMana = player.GetComponent<NewPlayer>().Mana / player.GetComponent<NewPlayer>().MaxMana;
        manaBar.fillAmount = Mathf.MoveTowards(manaBar.fillAmount, calculateMana, Time.deltaTime);
        manaText.text = "" + (int)player.GetComponent<NewPlayer>().Mana;
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
