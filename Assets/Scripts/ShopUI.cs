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

        List<Button> shopButtons;

        private void Start()
        {
            shopButtons = new List<Button>();

            shopButtons.Add(healthUpButton);
            shopButtons.Add(attackUpButton);
            shopButtons.Add(attackRangeUpButton);
            shopButtons.Add(speedUpButton);
            shopButtons.Add(manaUpButton);
            shopButtons.Add(spellPotencyUpButton);

            healthUpButton.onClick.AddListener(() => BuyStatIncrease(info.healthUpInfo.statToIncrease));
            attackUpButton.onClick.AddListener(() => BuyStatIncrease(info.attackUpInfo.statToIncrease));
            attackRangeUpButton.onClick.AddListener(() => BuyStatIncrease(info.attackRangeUpInfo.statToIncrease));
            speedUpButton.onClick.AddListener(() => BuyStatIncrease(info.speedUpInfo.statToIncrease));
            manaUpButton.onClick.AddListener(() => BuyStatIncrease(info.manaUpInfo.statToIncrease));
            spellPotencyUpButton.onClick.AddListener(() => BuyStatIncrease(info.spellPotencyUpInfo.statToIncrease));
        }

        private void OnEnable()
        {
            CheckButtons();
        }

        public void BuyStatIncrease(StatToIncrease stat)
        {
            switch (stat)
            {
                case StatToIncrease.health:
                    //Player.Instance.health += info.healthUpInfo.increaseValueBy;
                    info.healthUpInfo.Level++;
                    break;
                
                case StatToIncrease.attackPwr:
                    //Player.Instance.attackPwr += info.attackUpInfo.increaseValueBy;
                    info.attackUpInfo.Level++;
                    break;
                
                case StatToIncrease.attackRng:
                    //Player.Instance.attackRng += info.attackRangeUpInfo.increaseValueBy;
                    info.attackRangeUpInfo.Level++;
                    break;
                
                case StatToIncrease.speed:
                    //Player.Instance.speed += info.speedUpInfo.increaseValueBy;
                    info.speedUpInfo.Level++;
                    break;
                
                case StatToIncrease.mana:
                    //Player.Instance.mana += info.manaUpInfo.increaseValueBy;
                    info.manaUpInfo.Level++;
                    break;
                
                case StatToIncrease.spellPotency:
                    //Player.Instance.spellPotency += info.spellPotencyUpInfo.increaseValueBy; 
                    info.spellPotencyUpInfo.Level++;
                    break;
                
                default:
                    break;
            }

            CheckButtons();
        }

        public void CheckButtons()
        {
            //if (Player.Instance.gold < info.healthUpInfo.upgradeCost)
            //    healthUpButton.interactable = false;
            //else
            //    healthUpButton.interactable = true;

            //if (Player.Instance.gold < info.attackUpInfo.upgradeCost)
            //    attackUpButton.interactable = false;
            //else
            //    attackUpButton.interactable = true;

            //if (Player.Instance.gold < info.attackRangeUpInfo.upgradeCost)
            //    attackRangeUpButton.interactable = false;
            //else
            //    attackRangeUpButton.interactable = true;

            //if (Player.Instance.gold < info.speedUpInfo.upgradeCost)
            //    speedUpButton.interactable = false;
            //else
            //    speedUpButton.interactable = true;

            //if (Player.Instance.gold < info.manaUpInfo.upgradeCost)
            //    manaUpButton.interactable = false;
            //else
            //    manaUpButton.interactable = true;

            //if (Player.Instance.gold < info.spellPotencyUpInfo.upgradeCost)
            //    spellPotencyUpButton.interactable = false;
            //else
            //    spellPotencyUpButton.interactable = true;
        }
    }
}
