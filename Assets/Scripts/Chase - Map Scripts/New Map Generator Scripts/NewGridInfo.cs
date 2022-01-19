using UnityEngine;

[System.Serializable]
public class NewGridInfo
{
    [Tooltip("Represents the size of the grid"), Range(7, 15)] 
    public int gridSize = 7;

    [Tooltip("Represents the minimum distance from the spawn room to the boss room."), Range(4f, 15f)] 
    public float minDistSTB = 4f;

    public const int RoomSize = 31;
}