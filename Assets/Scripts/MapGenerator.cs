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

[System.Serializable]
public class GridInfo
{
    [Tooltip("Represents the size of the grid")]
    [Range(1, 15)]
    public int gridSize = 7;

    [Tooltip("Represents the minimum distance from the spawn room to the boss room.")]
    [Range(1f, 10f)]
    public float minDistSTB;

    public GridInfo()
    {
        minDistSTB = 4f;

    }

    public bool CheckSpawnBossDist(Vector2 SpawnRoomPos, Vector2 BRPos)
    {

        return Vector3.Distance(SpawnRoomPos, BRPos) / 31 < minDistSTB;
    }
}

/// <summary>
/// The map generator that spawns in the rooms in a 6x6 grid. 
/// </summary>
public class MapGenerator : MonoBehaviour
{
    #region Temporary Variables
    public GameObject basePrefab;

    /// <summary>
    /// Grid size is set as follows. 
    /// Max Y value will be set to gridSize.
    /// Max X value will be set to gridSize + 1.
    /// 
    /// Grid pos set as follows: grid[y, x]
    /// </summary>
    public GameObject[,] grid;

    private int roomSize = 31;


    #endregion

    public RoomContainer roomCont;


    private Vector2 SpawnRoomPos;
    private Vector2 BossRoomPos;

    public GridInfo gridInfo = new GridInfo();

    

    public void Awake()
    {
        if (roomCont == null)
            Debug.LogError("Room Container is empty. Please insert the scriptable object.");
    }


    public void Start()
    {
        grid = new GameObject[gridInfo.gridSize, gridInfo.gridSize + 1];

        SpawnMap();
    }

    /// <summary> Function which spawns in all of the rooms on the map. </summary>
    public void SpawnMap()
    {
        ///NOTES:
        /// 1. The spawn room will be in column zero so that it can remain in the
        /// array and to ensure that my algorithm for the path making will be consistent
        /// and will work better.
        ///     a. What this means is that nothing will be populating the grid on any row
        ///     at column position zero except for the spawn room.
        ///     
        
        Vector2 trueGridSize = new Vector2(gridInfo.gridSize + 1, gridInfo.gridSize);
        Vector2 tempGridPos;

        #region Spawning the spawn room
        int SRY = Random.Range(0, (int)trueGridSize.y);

        GameObject spawnRoom = Instantiate(roomCont.SpawnRooms[Random.Range(0, roomCont.SpawnRooms.Count)],
                                           new Vector3(0, SRY * roomSize),
                                           Quaternion.identity);

        tempGridPos = new Vector2(0, SRY);

        spawnRoom.GetComponent<Room>().gridPos = tempGridPos;
        SpawnRoomPos = spawnRoom.transform.position;

        spawnRoom.name = "Spawn Room";
        grid[(int)tempGridPos.y, 0] = spawnRoom;

        #endregion

        #region Spawning Boss Room

        int BRGX;
        int BRGY;
        Vector3 BRPos;
        do
        {
            BRGX = Random.Range(1, (int)trueGridSize.x);
            BRGY = Random.Range(0, (int)trueGridSize.y);

            BRPos = new Vector3(BRGX * roomSize, BRGY * roomSize);

            Debug.Log(BRPos);
        } while (!gridInfo.CheckSpawnBossDist(SpawnRoomPos, BRPos));
        

        GameObject bossRoom = Instantiate(roomCont.BossRooms[Random.Range(0, roomCont.BossRooms.Count)],
                                          BRPos,
                                          Quaternion.identity);


        tempGridPos = new Vector2(BRPos.x / roomSize, BRPos.y / roomSize);

        bossRoom.GetComponent<Room>().gridPos = tempGridPos;

        BossRoomPos = bossRoom.transform.position;

        bossRoom.name = "Boss Room: " + tempGridPos;

        grid[(int)tempGridPos.y, (int)tempGridPos.x] = bossRoom;

        Debug.Log(Vector3.Distance(SpawnRoomPos, BossRoomPos));
        #endregion

        #region Spawning in normal rooms

        ///Spawning a row
        for (int y = 0; y < trueGridSize.y; y++)
        {

            GameObject row = new GameObject("Row " + y);
            row.transform.parent = transform;

            ///Spawning in a column
            for (int x = 1; x < trueGridSize.x; x++)
            {
                if (grid[y, x] != null) continue;

                GameObject temp = Instantiate(basePrefab,
                                              new Vector3(roomSize * x, roomSize * y),
                                              Quaternion.identity);

                temp.name = "Room: (" + x + ", " + y + ")";
                
                temp.GetComponent<Room>().gridPos = new Vector2(x, y);
                
                temp.transform.parent = row.transform;
                
                grid[y, x] = temp;
            }
        }

        #endregion
    }


    private void GetDistance()
    {

    }

}
