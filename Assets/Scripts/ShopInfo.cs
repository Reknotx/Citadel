using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


namespace ShopSystem
{
    [CreateAssetMenu]
    public class ShopInfo : ScriptableObject
    {
        public ShopStatUpgrade healthUpInfo;
        public ShopStatUpgrade attackUpInfo;
        public ShopStatUpgrade attackRangeUpInfo;
        public ShopStatUpgrade speedUpInfo;
        public ShopStatUpgrade manaUpInfo;
        public ShopStatUpgrade spellPotencyUpInfo;

        [Space(10)]
        public SpellItem spell1Info;
        public SpellItem spell2Info;
        public SpellItem spell3Info;

        [HideInInspector]
        public int numOfSpells;

        public List<SpellItem> purchaseableSpells;

        protected int numOfItems;
        //public List<Items> puchaseableItems;

        public void Init()
        {
            healthUpInfo.statToIncrease = StatToIncrease.health;
            attackUpInfo.statToIncrease = StatToIncrease.attackPwr;
            attackRangeUpInfo.statToIncrease = StatToIncrease.attackRng;
            speedUpInfo.statToIncrease = StatToIncrease.speed;
            manaUpInfo.statToIncrease = StatToIncrease.mana;
            spellPotencyUpInfo.statToIncrease = StatToIncrease.spellPotency;

            RandomizeSpells();
        }
        public void RandomizeSpells()
        {
            ///Reference the list of all spells that can be purchased by the player.

            ///Randomly select 3 of those spells to be displayed in the shop

            ///The button's onclick needs to be assigned to purchase it's spell
            ///and provide it to the player.

            ///
            List<SpellItem> tempList = new List<SpellItem>();
            for (int i = 0; i < Mathf.Clamp(purchaseableSpells.Count, 0, 3); i++)
            {
                int index;
                while (true)
                {
                    index = Random.Range(0, purchaseableSpells.Count);

                    if (!tempList.Contains(purchaseableSpells[index]))
                        break;
                }

                switch (i)
                {
                    case 0: spell1Info = purchaseableSpells[index]; break;

                    case 1: spell2Info = purchaseableSpells[index]; break;

                    case 2: spell3Info = purchaseableSpells[index]; break;
                }

                tempList.Add(purchaseableSpells[index]);

            }

            spell1Info.name = spell1Info.spellPrefab.name;
            //spell2Info.name = spell2Info.spellPrefab.name;
            //spell3Info.name = spell3Info.spellPrefab.name;
        }
    }

    public abstract class PurchaseableItem
    {
        public abstract void Buy();

        public abstract override string ToString();
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
    public class ShopStatUpgrade : PurchaseableItem
    {

        [Tooltip("The base cost value of an upgrade.")]
        public float upgradeCost;

        [Tooltip("A value which affects the rate at which the upgrade cost scales.")]
        public float rateOfCostGrowth;

        [Tooltip("The amount that the stat is increased by.")]
        public float increaseStatBy;

        [Tooltip("The description of this stat and how much it increases that stat on purchase.")]
        public string description;

        private int _level = 0;

        public StatToIncrease statToIncrease;

        /// <summary> The current level of the upgrade. </summary>
        public int Level
        {
            get => _level;

            set
            {
                _level = value;
                upgradeCost = Mathf.RoundToInt(upgradeCost + rateOfCostGrowth);
            }
        }

        public override string ToString()
        {
            return description + " by " + increaseStatBy + " for " + upgradeCost + " gold.";
        }

        public override void Buy()
        {
            NewPlayer player = NewPlayer.Instance;

            switch (statToIncrease)
            {
                case StatToIncrease.health:
                    player.MaxHealth += (int)increaseStatBy;
                    player.Health += (int)increaseStatBy;
                    player.HealthBar.maxValue += increaseStatBy;
                    player.HealthBar.value += increaseStatBy;
                    player.HealthBar.gameObject.GetComponentInChildren<Text>().text = player.Health.ToString();
                    Debug.Log("Buying health upgrade");
                    break;

                case StatToIncrease.attackPwr:
                    player.combatSystem.meleeSystem.playerMeleeDamage += (int)increaseStatBy;
                    Debug.Log("Buying attack power upgrade");
                    break;

                case StatToIncrease.attackRng:
                    //player.meleeAttackRange += increaseStatBy;
                    Debug.Log("Buying attack range upgrade");
                    return;

                case StatToIncrease.speed:
                    player.speed += increaseStatBy;
                    Debug.Log("Buying speed upgrade");
                    break;

                case StatToIncrease.mana:
                    player.MaxMana += (int)increaseStatBy;
                    player.Mana += (int)increaseStatBy;
                    player.ManaBar.maxValue += increaseStatBy;
                    player.ManaBar.value += increaseStatBy;
                    player.ManaBar.gameObject.GetComponentInChildren<Text>().text = player.Mana.ToString();
                    Debug.Log("Buying mana upgrade");
                    break;

                case StatToIncrease.spellPotency:
                    //Player.Instance.spellPotency += info.spellPotencyUpInfo.increaseValueBy; 
                    Debug.Log("Buying spell potency upgrade");
                    return;

                default:
                    break;
            }
            //if (upgrade == null)
            //{
            //    Debug.LogError("Null reference found when purchasing upgrade.");
            //    return;
            //}

            Level++;
            NewPlayer.Instance.inventory.goldStorage.permanentGold -= upgradeCost;
        }
    }

    //this will need to be updated to match more of the spell system
    //that was put in place by hunter
    [System.Serializable]
    public class SpellItem : PurchaseableItem
    {
        public string name;
        public int spellCost;
        public GameObject spellPrefab;
        public string description;

        public override void Buy()
        {
            NewPlayer.Instance.combatSystem.spellSystem.spellBook.AddSpellToBook(spellPrefab);
        }

        public override string ToString()
        {
            return "Purchase a " + name + " for " + spellCost + " gold.";
        }
    }
    #region custom inspector stuff
#if UNITY_EDITOR
    [CustomEditor(typeof(ShopInfo))]
    public class ShopInfoEditor : Editor
    {
        float labelWidth = 135f;

        ShopInfo shopInfo;

        private void OnEnable()
        {
            shopInfo = (ShopInfo)target;
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            //EditorGUIUtility.currentViewWidth;

            EditorGUILayout.LabelField("Player Stat Upgrades", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
            EditorGUILayout.BeginHorizontal();

            PlaceLabelsForStatUpgrade();

            EditorGUILayout.EndHorizontal();

            PlaceStatUpgradeInfo();

            EditorGUILayout.Space(20);

            EditorGUILayout.LabelField("Spells", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });

            PlaceSpellItemInfo();


            EditorGUI.EndChangeCheck();

            serializedObject.ApplyModifiedProperties();

            void PlaceLabelsForStatUpgrade()
            {
                EditorGUILayout.LabelField("Stat", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight }, GUILayout.Width(labelWidth));
                EditorGUILayout.LabelField("Base Cost", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, wordWrap = true }, GUILayout.Width(50));
                EditorGUILayout.LabelField("Growth of cost", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, wordWrap = true }, GUILayout.Width(50));
                EditorGUILayout.LabelField("Increase by", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, wordWrap = true }, GUILayout.Width(55));
                EditorGUILayout.LabelField("Description", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft });
            }

            void PlaceStatUpgradeInfo()
            {
                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Health Upgrade", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight }, GUILayout.Width(labelWidth));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("healthUpInfo"), GUIContent.none);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Attack Upgrade", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight }, GUILayout.Width(labelWidth));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("attackUpInfo"), GUIContent.none);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Attack Range Upgrade", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight }, GUILayout.Width(labelWidth));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("attackRangeUpInfo"), GUIContent.none);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Speed Upgrade", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight }, GUILayout.Width(labelWidth));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("speedUpInfo"), GUIContent.none);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Mana Upgrade", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight }, GUILayout.Width(labelWidth));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("manaUpInfo"), GUIContent.none);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Spell Potency Upgrade", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight }, GUILayout.Width(labelWidth));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("spellPotencyUpInfo"), GUIContent.none);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }

            void PlaceSpellItemInfo()
            {
                if (shopInfo.purchaseableSpells == null)
                    shopInfo.purchaseableSpells = new List<SpellItem>();

                shopInfo.numOfSpells = EditorGUILayout.IntSlider(GUIContent.none, shopInfo.numOfSpells, 1, 12);

                while (shopInfo.numOfSpells != shopInfo.purchaseableSpells.Count)
                {
                    if (shopInfo.numOfSpells > shopInfo.purchaseableSpells.Count)
                        shopInfo.purchaseableSpells.Add(new SpellItem());
                    else
                        shopInfo.purchaseableSpells.RemoveAt(shopInfo.purchaseableSpells.Count - 1);
                }

                for (int index = 0; index < serializedObject.FindProperty("purchaseableSpells").arraySize; index++)
                {
                    GameObject itemLocal = (GameObject)serializedObject.FindProperty("purchaseableSpells").GetArrayElementAtIndex(index).FindPropertyRelative("spellPrefab").objectReferenceValue;
                    EditorGUILayout.BeginHorizontal();

                    if (itemLocal != null)
                        EditorGUILayout.LabelField(itemLocal.name, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight }, GUILayout.MaxWidth(100));
                    else
                        EditorGUILayout.LabelField("Insert Object", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight }, GUILayout.MaxWidth(100));

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("purchaseableSpells").GetArrayElementAtIndex(index), GUIContent.none);

                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }


    [CustomPropertyDrawer(typeof(ShopStatUpgrade))]
    public class ShopStatUpgradeDrawerUIE : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleRight
            };

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label, style);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Vector2 upgradeCostRectPos = new Vector2(position.x, position.y);
            Vector2 rateOfCostGrowthPos = new Vector2(position.x + 55, position.y);
            Vector2 increaseStatByPos = new Vector2(position.x + 110, position.y);
            Vector2 descriptionPos = new Vector2(position.x + 165, position.y);

            Vector2 numberBoxSize = new Vector2(50, position.height);
            Vector2 descriptionBoxSize = new Vector2(position.width - 165, position.height);

            var upgradeCostRect = new Rect(upgradeCostRectPos, numberBoxSize);
            var rateOfCostGrowthRect = new Rect(rateOfCostGrowthPos, numberBoxSize);
            var increaseStatByRect = new Rect(increaseStatByPos, numberBoxSize);
            var descriptionRect = new Rect(descriptionPos, descriptionBoxSize);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(upgradeCostRect, property.FindPropertyRelative("upgradeCost"), GUIContent.none);
            EditorGUI.PropertyField(rateOfCostGrowthRect, property.FindPropertyRelative("rateOfCostGrowth"), GUIContent.none);
            EditorGUI.PropertyField(increaseStatByRect, property.FindPropertyRelative("increaseStatBy"), GUIContent.none);
            EditorGUI.PropertyField(descriptionRect, property.FindPropertyRelative("description"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }

    
    [CustomPropertyDrawer(typeof(SpellItem))]
    public class SpellItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleRight
            };

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Vector2 objectRectPos = new Vector2(position.x, position.y);
            Vector2 costRectPos = new Vector2(position.x + 105, position.y);
            Vector2 descriptionRectPos = new Vector2(position.x + 160, position.y);

            Vector2 objectSize = new Vector2(100, position.height);
            Vector2 costSize = new Vector2(50, position.height);
            Vector2 descriptionBoxSize = new Vector2(position.width - 160, position.height);

            var objectRect = new Rect(objectRectPos, objectSize);
            var costRect = new Rect(costRectPos, costSize);
            var descriptionRect = new Rect(descriptionRectPos, descriptionBoxSize);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(objectRect, property.FindPropertyRelative("spellPrefab"), GUIContent.none);
            EditorGUI.PropertyField(costRect, property.FindPropertyRelative("spellCost"), GUIContent.none);
            EditorGUI.PropertyField(descriptionRect, property.FindPropertyRelative("description"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }

#endif
    #endregion
}