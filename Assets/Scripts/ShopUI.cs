using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace ShopSystem
{
    public class ShopUI : MonoBehaviour
    {
        public ShopInfo info;
        public Button healthUpButton;
        public Button attackUpButton;
        public Button attackRangeUpButton;
        public Button speedUpButton;
        public Button manaUpButton;
        public Button spellPotencyUpButton;
        public Button firewallSpellButton;

        List<Button> shopButtons;

        private void Awake()
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
        }

        private void Start()
        {
            shopButtons = new List<Button>();

            shopButtons.Add(healthUpButton);
            shopButtons.Add(attackUpButton);
            shopButtons.Add(attackRangeUpButton);
            shopButtons.Add(speedUpButton);
            shopButtons.Add(manaUpButton);
            shopButtons.Add(spellPotencyUpButton);
            shopButtons.Add(firewallSpellButton);

            healthUpButton.onClick.AddListener(() => BuyStatIncrease(info.healthUpInfo.statToIncrease));
            attackUpButton.onClick.AddListener(() => BuyStatIncrease(info.attackUpInfo.statToIncrease));
            attackRangeUpButton.onClick.AddListener(() => BuyStatIncrease(info.attackRangeUpInfo.statToIncrease));
            speedUpButton.onClick.AddListener(() => BuyStatIncrease(info.speedUpInfo.statToIncrease));
            manaUpButton.onClick.AddListener(() => BuyStatIncrease(info.manaUpInfo.statToIncrease));
            spellPotencyUpButton.onClick.AddListener(() => BuyStatIncrease(info.spellPotencyUpInfo.statToIncrease));

            firewallSpellButton.onClick.AddListener(() => BuySpell());
        }

        private void OnEnable()
        {
            CheckButtons();
        }

        private void OnDisable()
        {
            if (Player.Instance != null) Player.Instance.canMove = true;
        }

        public void BuyStatIncrease(StatToIncrease stat)
        {
            int goldSpent = 0;

            switch (stat)
            {
                case StatToIncrease.health:
                    Player.Instance.maxHealth += (int)info.healthUpInfo.increaseValueBy;
                    goldSpent = info.healthUpInfo.upgradeCost;
                    info.healthUpInfo.Level++;
                    Debug.Log("Buying health upgrade");
                    break;
                
                case StatToIncrease.attackPwr:
                    Player.Instance.meleeAttackDamage += (int)info.attackUpInfo.increaseValueBy;
                    goldSpent = info.attackUpInfo.upgradeCost;
                    info.attackUpInfo.Level++;
                    Debug.Log("Buying attack power upgrade");
                    break;
                
                case StatToIncrease.attackRng:
                    Player.Instance.meleeAttackRange += info.attackRangeUpInfo.increaseValueBy;
                    goldSpent = info.attackRangeUpInfo.upgradeCost;
                    info.attackRangeUpInfo.Level++;
                    Debug.Log("Buying attack range upgrade");
                    break;
                
                case StatToIncrease.speed:
                    Player.Instance.speed += info.speedUpInfo.increaseValueBy;
                    goldSpent = info.speedUpInfo.upgradeCost;
                    info.speedUpInfo.Level++;
                    Debug.Log("Buying speed upgrade");
                    break;
                
                case StatToIncrease.mana:
                    Player.Instance.maxMana += (int)info.manaUpInfo.increaseValueBy;
                    goldSpent = info.manaUpInfo.upgradeCost;
                    info.manaUpInfo.Level++;
                    Debug.Log("Buying mana upgrade");
                    break;
                
                case StatToIncrease.spellPotency:
                    //Player.Instance.spellPotency += info.spellPotencyUpInfo.increaseValueBy; 
                    goldSpent = info.spellPotencyUpInfo.upgradeCost;
                    info.spellPotencyUpInfo.Level++;
                    Debug.Log("Buying spell potency upgrade");
                    break;
                
                default:
                    break;
            }

            GoldHandler.Instance.MyHardGold -= goldSpent;

            CheckButtons();
        }

        public void CheckButtons()
        {
            float hardGold = GoldHandler.Instance.MyHardGold;

            healthUpButton.interactable = hardGold > info.healthUpInfo.upgradeCost;
            attackUpButton.interactable = hardGold > info.attackUpInfo.upgradeCost;
            attackRangeUpButton.interactable = hardGold > info.attackRangeUpInfo.upgradeCost;
            speedUpButton.interactable = hardGold > info.speedUpInfo.upgradeCost;
            manaUpButton.interactable = hardGold > info.manaUpInfo.upgradeCost;
            spellPotencyUpButton.interactable = hardGold > info.spellPotencyUpInfo.upgradeCost;

            firewallSpellButton.interactable = hardGold > info.fireWall.baseSpellCost;
        }
        public void BuySpell()
        {
            Player.Instance.fireWall_prefab = info.fireWall.spellPrefab;
        }

    }

}
