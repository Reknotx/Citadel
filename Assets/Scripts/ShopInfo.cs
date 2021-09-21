using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace ShopSystem
{
    [CreateAssetMenu]
    public class ShopInfo : ScriptableObject
    {
        public ShopUpgradeItem healthUpInfo;
        public ShopUpgradeItem attackUpInfo;
        public ShopUpgradeItem attackRangeUpInfo;
        public ShopUpgradeItem speedUpInfo;
        public ShopUpgradeItem manaUpInfo;
        public ShopUpgradeItem spellPotencyUpInfo;
        [Space(10)]
        public SpellItem fireWall;
    }

    public enum StatToIncrease
    {
        health,
        attackPwr,
        attackRng,
        speed,
        mana,
        spellPotency
    }

    [System.Serializable]
    public class ShopUpgradeItem
    {

        [Tooltip("The base cost value of an upgrade.")]
        public int upgradeCost;

        [Tooltip("A value which affects the rate at which the upgrade cost scales.")]
        public float rateOfCostGrowth;

        [Tooltip("The amount that the stat is increased by.")]
        public float increaseValueBy;

        private int _level = 0;

        public StatToIncrease statToIncrease;

        /// <summary> The current level of the upgrade. </summary>
        public int Level
        {
            get => _level;

            set
            {
                _level = (int)Mathf.Clamp01(value);
                upgradeCost = Mathf.RoundToInt(upgradeCost * increaseValueBy);
            }
        }
    }

    //this will need to be updated to match more of the spell system
    //that was put in place by hunter
    [System.Serializable]
    public class SpellItem
    {
        public int baseSpellCost;
        public GameObject spellPrefab;
    }
}