using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory
{
    private int healthPotionHealAmnt = 10;
    private int manaPotionRefillAmnt = 10;

    public int HealthPotions = 0;

    public int ManaPotions = 0;

    public bool shuues = false;
    public bool undying = false;
    public bool spellStone = false;
    public bool floatingShield = false;
    public bool medicineStash = false;

    public void UseManaPotion()
    {
        if (ManaPotions == 0) return;
        NewPlayer.Instance.Mana += manaPotionRefillAmnt;

    }

    public void UseHealthPotion()
    {
        if (HealthPotions == 0) return;

        NewPlayer.Instance.Health += healthPotionHealAmnt;

    }
}
