/*
 * Author: Chase O'Connor
 * Date: 9/2/2021
 * 
 * Brief: This script contains the info of the rooms. At first
 * it will only contain the positions of openings. Later will
 * be updated to contain other information most likely.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Map
{

    /// <summary> Enum for the various positions of the doors in each room. </summary>
    public enum DoorPositions
    {
        EMPTY,

        [Tooltip("Opening on top row left side.")]
        TopLeft,
        [Tooltip("Opening on top row in middle.")]
        TopMiddle,
        [Tooltip("Opening on top row right side.")]
        TopRight,

        [Tooltip("Opening on bottom row left side.")]
        BottomLeft,
        [Tooltip("Opening on bottom row in middle.")]
        BottomMiddle,
        [Tooltip("Opening on bottom row right side.")]
        BottomRight,

        [Tooltip("Opening on left row on top.")]
        LeftTop,
        [Tooltip("Opening on left row in middle.")]
        LeftMiddle,
        [Tooltip("Opening on left row on bottom.")]
        LeftBottom,

        [Tooltip("Opening on right row on top.")]
        RightTop,
        [Tooltip("Opening on right row in middle.")]
        RightMiddle,
        [Tooltip("Opening on right row on bottom.")]
        RightBottom
    }

    [CreateAssetMenu]
    [System.Serializable]
    public class RoomInfo : ScriptableObject
    {
        /// <summary>The number of entrances this room has open.</summary>
        public int numOpenings = 1;

        public List<DoorPositions> openDoors = new List<DoorPositions>(1);

        //public List<DoorPositions> doorPositions = new List<DoorPositions>();


        public int CalcDoorsLeftSide()
        {
            int amnt = 0;

            foreach (DoorPositions positions in openDoors)
            {
                if (positions == DoorPositions.LeftBottom
                    || positions == DoorPositions.LeftMiddle
                    || positions == DoorPositions.LeftTop)
                    amnt++;
            }
            return amnt;
        }


        public int CalcDoorsRightSide()
        {
            int amnt = 0;

            foreach (DoorPositions positions in openDoors)
            {
                if (positions == DoorPositions.RightBottom
                    || positions == DoorPositions.RightMiddle
                    || positions == DoorPositions.RightTop)
                    amnt++;
            }
            return amnt;
        }

        public int CalcDoorsTopSide()
        {
            int amnt = 0;

            foreach (DoorPositions positions in openDoors)
            {
                if (positions == DoorPositions.TopLeft
                    || positions == DoorPositions.TopMiddle
                    || positions == DoorPositions.TopRight)
                    amnt++;
            }
            return amnt;
        }

        public int CalcDoorsBottomSide()
        {
            int amnt = 0;

            foreach (DoorPositions positions in openDoors)
            {
                if (positions == DoorPositions.BottomLeft
                    || positions == DoorPositions.BottomMiddle
                    || positions == DoorPositions.BottomRight)
                    amnt++;
            }
            return amnt;
        }

        public override string ToString()
        {
            Debug.LogFormat("Room openings - Top: {0}, Bottom: {1}, Left: {2}, Right: {3}", CalcDoorsTopSide(),
                CalcDoorsBottomSide(), CalcDoorsLeftSide(), CalcDoorsRightSide());
            return "";
        }

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(RoomInfo))]
    public class RoomInfoEditor : Editor
    {
        RoomInfo roomInfo;

        private void OnEnable()
        {
            roomInfo = (RoomInfo)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            roomInfo.numOpenings = EditorGUILayout.IntSlider("Number of openings", roomInfo.numOpenings, 1, 12);

            while (roomInfo.numOpenings != roomInfo.openDoors.Count)
            {
                if (roomInfo.numOpenings > roomInfo.openDoors.Count)
                    roomInfo.openDoors.Add(DoorPositions.EMPTY);
                else
                    roomInfo.openDoors.RemoveAt(roomInfo.openDoors.Count - 1);
            }

            for (int index = 0; index < roomInfo.numOpenings; index++)
            {
                roomInfo.openDoors[index] = (DoorPositions)EditorGUILayout.EnumPopup("Door Position:", roomInfo.openDoors[index]);
            }

            bool somethingChanged = EditorGUI.EndChangeCheck();

            if (somethingChanged)
                EditorUtility.SetDirty(roomInfo);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}