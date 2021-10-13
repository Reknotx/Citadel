using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShopSystem
{
    ///Notes for the overhaul
    ///1. Buy functions and hover actions need to be assigned on start
    ///so that they are easy to work with and it isn't clunky


    public class ShopUI : MonoBehaviour
    {
        public ShopInfo info;
        public Button healthUpButton;
        public Button attackUpButton;
        public Button attackRangeUpButton;
        public Button speedUpButton;
        public Button manaUpButton;
        public Button spellPotencyUpButton;

        public Button spell1Button;
        public Button spell2Button;
        public Button spell3Button;

        public GameObject popupDisplay;

        List<Button> shopButtons;

        List<Button> spellButtons;

        private void Awake()
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
        }

        private void Start()
        {
            shopButtons = new List<Button>();

            info.Init();

            shopButtons.Add(healthUpButton);
            shopButtons.Add(attackUpButton);
            shopButtons.Add(attackRangeUpButton);
            shopButtons.Add(speedUpButton);
            shopButtons.Add(manaUpButton);
            shopButtons.Add(spellPotencyUpButton);
            shopButtons.Add(spell1Button);
            shopButtons.Add(spell2Button);
            shopButtons.Add(spell3Button);

            healthUpButton.onClick.AddListener(() => Buy(info.healthUpInfo));
            attackUpButton.onClick.AddListener(() => Buy(info.attackUpInfo));
            attackRangeUpButton.onClick.AddListener(() => Buy(info.attackRangeUpInfo));
            speedUpButton.onClick.AddListener(() => Buy(info.speedUpInfo));
            manaUpButton.onClick.AddListener(() => Buy(info.manaUpInfo));
            spellPotencyUpButton.onClick.AddListener(() => Buy(info.spellPotencyUpInfo));

            spell1Button.onClick.AddListener(() => Buy(info.spell1Info));
            spell2Button.onClick.AddListener(() => Buy(info.spell2Info));
            spell3Button.onClick.AddListener(() => Buy(info.spell3Info));

            spell1Button.GetComponentInChildren<Text>().text = info.spell1Info.name;
            spell2Button.GetComponentInChildren<Text>().text = info.spell2Info.name;
            spell3Button.GetComponentInChildren<Text>().text = info.spell3Info.name;
        }

        public void OnEnable()
        {
            CheckButtons();
        }

        public void OnDisable()
        {
            if (Player.Instance != null) Player.Instance.canMove = true;
        }

        public void Buy(PurchaseableItem purchaseableItem)
        {
            Debug.Log("Success");

            purchaseableItem.Buy();

            CheckButtons();
        }

        public void DisplayPopUp(string name)
        {
            PurchaseableItem item = null;

            popupDisplay.SetActive(true);

            //Debug.Log("Trying to display popup");

            switch(name)
            {
                case "health": item = info.healthUpInfo; break;

                case "attackPwr": item = info.attackUpInfo; break;

                case "attackRng": item = info.attackRangeUpInfo; break;

                case "speed": item = info.speedUpInfo; break;

                case "mana": item = info.manaUpInfo; break;

                case "spellPotent": item = info.spellPotencyUpInfo; break;

                case "spell 1": item = info.spell1Info; break;

                case "spell 2": item = info.spell2Info; break;

                case "spell 3": item = info.spell3Info; break;
            }

            popupDisplay.GetComponent<Popup.PopupDisplay>().DescriptionText = item.ToString();
        }

        public void TurnOffPopUp()
        {
            popupDisplay.SetActive(false);
        }

        public void CheckButtons()
        {
            if (GoldHandler.Instance == null) return;

            float hardGold = GoldHandler.Instance.MyHardGold;

            healthUpButton.interactable = hardGold > info.healthUpInfo.upgradeCost;
            attackUpButton.interactable = hardGold > info.attackUpInfo.upgradeCost;
            attackRangeUpButton.interactable = hardGold > info.attackRangeUpInfo.upgradeCost;
            speedUpButton.interactable = hardGold > info.speedUpInfo.upgradeCost;
            manaUpButton.interactable = hardGold > info.manaUpInfo.upgradeCost;
            spellPotencyUpButton.interactable = hardGold > info.spellPotencyUpInfo.upgradeCost;

            spell1Button.interactable = hardGold > info.spell1Info.spellCost;
        }
        //public void BuySpell()
        //{
        //    Player.Instance.fireWall_prefab = info.fireWall.spellPrefab;
        //}
    }

}
