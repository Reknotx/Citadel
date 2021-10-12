using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
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

        List<Button> spellButtons;

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

            healthUpButton.onClick.AddListener(() => Buy(info.healthUpInfo));
            attackUpButton.onClick.AddListener(() => Buy(info.attackUpInfo));
            attackRangeUpButton.onClick.AddListener(() => Buy(info.attackRangeUpInfo));
            speedUpButton.onClick.AddListener(() => Buy(info.speedUpInfo));
            manaUpButton.onClick.AddListener(() => Buy(info.manaUpInfo));
            spellPotencyUpButton.onClick.AddListener(() => Buy(info.spellPotencyUpInfo));

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

        public void Buy(PurchaseableItem purchaseableItem)
        {
            purchaseableItem.Buy();

            CheckButtons();
        }

        public void DisplayPopUp(string description)
        {

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

            firewallSpellButton.interactable = hardGold > info.fireWall.spellCost;
        }
        public void BuySpell()
        {
            Player.Instance.fireWall_prefab = info.fireWall.spellPrefab;
        }


        public void RandomizeSpells()
        {

        }
    }

}
