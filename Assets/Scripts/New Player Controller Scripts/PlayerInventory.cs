using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int healthPotionHealAmnt = 10;
    private int manaPotionRefillAmnt = 10;

    public int HealthPotions = 0;
    private const int _MaxHealthPotions = 5;

    public bool HealthPotionSpace { get { return HealthPotions < _MaxHealthPotions; } }

    public int ManaPotions = 0;
    private const int _MaxManaPotions = 5;

    public bool ManaPotionSpace { get { return ManaPotions < _MaxManaPotions; } }

    public bool shuues = false;
    public bool undying = false;
    public bool spellStone = false;
    public bool floatingShield = false;
    public bool medicineStash = false;
    public bool compass = false;
    public bool serratedStone = false;

    public GoldStorage goldStorage;

    public PlayerInventory()
    {
        goldStorage = new GoldStorage();
    }

    public void UseManaPotion()
    {
        if (ManaPotions == 0) return;
        NewPlayer.Instance.Mana += manaPotionRefillAmnt;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public void AddManaPotion()
    {
        ManaPotions = Mathf.Clamp(ManaPotions++, 0, _MaxManaPotions);
    }

    public void UseHealthPotion()
    {
        if (HealthPotions == 0) return;

        NewPlayer.Instance.Health += healthPotionHealAmnt;

    }

    public void AddHealthPotion()
    {
        HealthPotions = Mathf.Clamp(HealthPotions++, 0, _MaxHealthPotions);
    }

    public class GoldStorage
    {
        /// <summary> The gold that the player has saved up that can be used in the casle. </summary>
        public float permanentGold;

        /// <summary> The gold in the player's inventory in the castle. This can be lost on death. </summary>
        public float gold;

        private const string permGoldStorage = "permanentGold";

        public GoldStorage()
        {
            float permGold = PlayerPrefs.GetFloat(permGoldStorage, 0);

            permanentGold = permGold != 0 ? permGold : 0;
        }

        /// <summary>
        /// Adds the gold in the player's inventory to their permanent gold, and saves it.
        /// </summary>
        public void AddGoldToStorage()
        {
            permanentGold += gold;
            PlayerPrefs.SetFloat(permGoldStorage, permanentGold);
            gold = 0;
        }

        public void AddGoldFromMinesToStorage(int amount)
        {
            permanentGold += amount;
        }
    }
}
