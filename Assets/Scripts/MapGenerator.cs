/*
 * Author: Chase O'Connor
 * Date: 9/2/2021
 * 
 * Brief: This script spawns in the map at the start of the game
 * and each time the player moves to another floor.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The map generator that spawns in the rooms in a 6x6 grid. 
/// </summary>
public class MapGenerator : MonoBehaviour
{
    #region Temporary Variables
    public GameObject basePrefab;
    public GameObject[,] grid;

    private int roomSize = 25;

    [Range(1, 15)]
    public int gridSize = 7;
    #endregion

    public void Start()
    {
        grid = new GameObject[gridSize,gridSize];

        SpawnMap();
    }

    public void SpawnMap()
    {

        for (int i = 0; i < gridSize; i++)
        {
            GameObject row = new GameObject("Row " + i);
            row.transform.parent = transform;

            for (int j = 0; j < gridSize; j++)
            {
                GameObject temp = Instantiate(basePrefab, new Vector3(roomSize * j, roomSize * i), Quaternion.identity);
                temp.name = "Room: (" + j + ", " + i + ")";
                temp.GetComponent<Room>().gridPos = new Vector2(j, i);
                temp.transform.parent = row.transform;
                grid[j, i] = temp;
            }
        }
    }
}
