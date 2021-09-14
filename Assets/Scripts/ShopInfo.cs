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

        /// <summary> The current level of the upgrade. </summary>
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                IncreaseCost();
            }
        }

        void IncreaseCost()
        {
            ///Increase the upgrade cost
        }

    }

    //this will need to be updated to match more of the spell system
    //that was put in place by hunter
    [System.Serializable]
    public class SpellItem
    {
        public int baseSpellCost;
    }
}