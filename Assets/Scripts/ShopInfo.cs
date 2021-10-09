using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


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
        public SpellItem fireWall;

        protected int numOfSpells;

        public List<SpellItem> purchaseableSpells;

        protected int numOfItems;
        //public List<Items> puchaseableItems;
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
    public class ShopStatUpgrade
    {

        [Tooltip("The base cost value of an upgrade.")]
        public int upgradeCost;

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
                _level = (int)Mathf.Clamp01(value);
                upgradeCost = Mathf.RoundToInt(upgradeCost * increaseStatBy);
            }
        }
    }

    //this will need to be updated to match more of the spell system
    //that was put in place by hunter
    [System.Serializable]
    public class SpellItem
    {
        public string name;
        public int baseSpellCost;
        public GameObject spellPrefab;
        public string description;

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ShopInfo))]
    public class ShopInfoEditor : Editor
    {
        float labelWidth = 135f;

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

            EditorGUI.EndChangeCheck();

            serializedObject.ApplyModifiedProperties();
            
            void PlaceLabelsForStatUpgrade()
            {
                EditorGUILayout.LabelField("Stat", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight }, GUILayout.Width(labelWidth));
                EditorGUILayout.LabelField("Cost", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter }, GUILayout.Width(50));
                EditorGUILayout.LabelField("Growth", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter }, GUILayout.Width(50));
                EditorGUILayout.LabelField("Increase by", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, wordWrap = true }, GUILayout.Width(55));
                EditorGUILayout.LabelField("Description", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft });
            }
            
            void PlaceStatUpgradeInfo()
            {

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

    }

#endif
}