using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory
{
    private const int healthPotionHealAmnt = 10;
    private const int manaPotionRefillAmnt = 10;
    
    public int MaxHealthPotions = 5;
    public int MaxManaPotions = 5;

    public int HealthPotions;
    public int ManaPotions;

    public bool ManaPotionSpace => ManaPotions < MaxManaPotions;
    public bool HealthPotionSpace => HealthPotions < MaxHealthPotions;
    
    public bool shuues = false;
    public bool undying = false;
    public bool spellStone = false;
    public bool floatingShield = false;
    public bool medicineStash = false;
    public bool compass = false;
    public bool serratedStone = false;

    public GoldStorage goldStorage;

    public PlayerInventory() => goldStorage = new GoldStorage();

    public void UseManaPotion()
    {
        if (ManaPotions == 0) return; 
        NewPlayer.Instance.Mana += manaPotionRefillAmnt;
        ManaPotions--;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public void AddManaPotion() => ManaPotions = Mathf.Clamp(ManaPotions++, 0, MaxManaPotions);

    public void UseHealthPotion()
    {
        if (HealthPotions == 0) return;

        HealthPotions--;
        NewPlayer.Instance.Health += healthPotionHealAmnt;
    }

    public void AddHealthPotion() => HealthPotions = Mathf.Clamp(HealthPotions++, 0, MaxHealthPotions);

    public class GoldStorage
    {
        /// <summary> The gold that the player has saved up that can be used in the castle. </summary>
        public float permanentGold;

        /// <summary> The gold in the player's inventory in the castle. This can be lost on death. </summary>
        private float gold;

        private const string permGoldStorage = "permanentGold";

        public GoldStorage()
        {
            float permGold = PlayerPrefs.GetFloat(permGoldStorage, 0);

            permanentGold = permGold != 0 ? permGold : 0;
        }

        /// <summary> Adds the gold in the player's inventory to their permanent gold, and saves it. </summary>
        public void AddHeldGoldToStorage()
        {
            permanentGold += gold;
            PlayerPrefs.SetFloat(permGoldStorage, permanentGold);
            gold = 0;
        }

        /// <summary> The amount of gold to add to the player's permanent gold from the mines. </summary>
        /// <param name="amount">The amount of gold to add to our permanent storage</param>
        public void MineGoldToPermGold(int amount)
        {
            permanentGold += amount;
            PlayerPrefs.SetFloat(permGoldStorage, permanentGold);
        }

        /// <summary> Used to add gold to the player's held gold. Not permanent gold. </summary>
        /// <param name="amount">The amount of gold to add to the player's bag.</param>
        public void AddGold(int amount) => gold += amount;
    }
}
