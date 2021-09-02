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

    
    void Start()
    {
        currentLife = myLife;
        currentMana = myMana;
    }

    
    void Update()
    {
        calculateLife = currentLife / myLife;
        healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, calculateLife, Time.deltaTime);
        healthText.text = "" + (int)currentLife;

        calculateMana = currentMana / myMana;
        manaBar.fillAmount = Mathf.MoveTowards(manaBar.fillAmount, calculateMana, Time.deltaTime);
        manaText.text = "" + (int)currentMana;
    }

    public void Damage(float damage)
    {
        currentLife -= damage;
    }

    public void ReduceMana(float mana)
    {
        currentMana -= mana;
    }
}
