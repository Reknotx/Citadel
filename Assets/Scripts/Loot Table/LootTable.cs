using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public struct LootInfo
{
    public string name;

    public GameObject item;

    public float rate;
}


[CreateAssetMenu(fileName = "Loot Table", menuName = "Loot Table")]
public class LootTable : ScriptableObject
{
    public int numSpawnableItems;

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

//#if UNITY_EDITOR
//[CustomEditor(typeof(LootTable))]
//public class LootTableEditor : Editor
//{
//    LootTable lootTableInfo;
//    SerializedProperty

//    private void OnEnable()
//    {
//        lootTableInfo = (LootTable)target;
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        EditorGUI.BeginChangeCheck();

//        lootTableInfo.numSpawnableItems = EditorGUILayout.IntSlider("Number of spawnable items", lootTableInfo.numSpawnableItems, 0, 10);

//        while (lootTableInfo.numSpawnableItems != lootTableInfo.loot.Count)
//        {
//            if (lootTableInfo.numSpawnableItems > lootTableInfo.loot.Count)
//                lootTableInfo.loot.Add(new LootInfo());
//            else
//                lootTableInfo.loot.RemoveAt(lootTableInfo.loot.Count - 1);
//        }

//        for (int index = 0; index < lootTableInfo.loot.Count; index++)
//        {
//            lootTableInfo.loot[index] = (LootInfo)EditorGUILayout.PropertyField(lootTableInfo.loot[index], true);
//        }

//        bool somethingChanged = EditorGUI.EndChangeCheck();

//        if (somethingChanged)
//            EditorUtility.SetDirty(roomInfo);

//        serializedObject.ApplyModifiedProperties();
//    }
//}
//#endif