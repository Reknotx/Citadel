using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

[System.Serializable]
public class LootInfo
{
    public string name;

    public GameObject item;

    public float rate;
}


[CreateAssetMenu(fileName = "Loot Table", menuName = "Loot Table")]
public class LootTable : ScriptableObject
{
    public int numSpawnableItems;

    [HideInInspector]
    public List<LootInfo> loot = new List<LootInfo>();

    private float GetWeightTotal()
    {
        float total = 0f;

        foreach (LootInfo info in loot)
        {
            total += info.rate;
        }

        return total;
    }

    public GameObject Drop()
    {
        float total = GetWeightTotal();

        insertionSort(loot);

        float num = UnityEngine.Random.Range(0, total);

        foreach (LootInfo info in loot)
        {
            if (num <= info.rate)
            {
                return info.item;
            }
            else
            {
                num -= info.rate;
            }
        }

        return null;

        void insertionSort(List<LootInfo> unsortedLoot)
        {
            int n = unsortedLoot.Count;
            for (int i = 1; i < n; ++i)
            {
                LootInfo key = unsortedLoot[i];
                int j = i - 1;

                while (j >= 0 && unsortedLoot[j].rate > key.rate)
                {
                    unsortedLoot[j + 1] = unsortedLoot[j];
                    j = j - 1;
                }
                unsortedLoot[j + 1] = key;
            }

            loot.Reverse();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(LootTable))]
public class LootTableEditor : Editor
{
    LootTable lootTableInfo;
    SerializedProperty property;

    private void OnEnable()
    {
        lootTableInfo = (LootTable)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //serializedObject.FindProperty("loot").GetArrayElementAtIndex(0);

        EditorGUI.BeginChangeCheck();

        lootTableInfo.numSpawnableItems = EditorGUILayout.IntSlider("Number of spawnable items", lootTableInfo.numSpawnableItems, 1, 10);

        while (lootTableInfo.numSpawnableItems != lootTableInfo.loot.Count)
        {
            if (lootTableInfo.numSpawnableItems > lootTableInfo.loot.Count)
                lootTableInfo.loot.Add(new LootInfo());
            else
                lootTableInfo.loot.RemoveAt(lootTableInfo.loot.Count - 1);
        }

        for (int index = 0; index < serializedObject.FindProperty("loot").arraySize; index++)
        {
            GameObject itemLocal = (GameObject)serializedObject.FindProperty("loot").GetArrayElementAtIndex(index).FindPropertyRelative("item").objectReferenceValue;
            EditorGUILayout.BeginHorizontal();

            if (itemLocal != null)
                EditorGUILayout.LabelField(itemLocal.name, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight }, GUILayout.MaxWidth(100));
            else
                EditorGUILayout.LabelField("Insert Object", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight }, GUILayout.MaxWidth(100));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("loot").GetArrayElementAtIndex(index), GUIContent.none);
            
            EditorGUILayout.EndHorizontal();
        }

        bool somethingChanged = EditorGUI.EndChangeCheck();

        if (somethingChanged)
            EditorUtility.SetDirty(lootTableInfo);

        serializedObject.ApplyModifiedProperties();
    }


    [CustomPropertyDrawer(typeof(LootInfo))]
    public class LootInfoDrawer : PropertyDrawer
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
            //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), GUIContent.none, style);
            //EditorGUI.PrefixLabel()

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Vector2 nameRectPos = new Vector2(position.x, position.y);
            Vector2 objectRectPos = new Vector2(position.x, position.y);
            Vector2 dropRateRectPos = new Vector2(position.x + 155, position.y);

            Vector2 numberBoxSize = new Vector2(150, position.height);
            Vector2 dropRateBoxSize = new Vector2(position.width - 155, position.height);

            var nameRect = new Rect(nameRectPos, numberBoxSize);
            var objectRect = new Rect(objectRectPos, numberBoxSize);
            var dropRateRect = new Rect(dropRateRectPos, dropRateBoxSize);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            //EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);
            EditorGUI.PropertyField(objectRect, property.FindPropertyRelative("item"), GUIContent.none);
            EditorGUI.Slider(dropRateRect, property.FindPropertyRelative("rate"), 0.1f, 100f, GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
#endif