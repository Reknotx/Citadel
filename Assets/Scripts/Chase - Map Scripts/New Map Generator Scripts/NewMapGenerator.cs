/* Author: Chase O'Connor
 * Date: 9/2/2021
 * 
 * Brief: This script spawns in the map at the start of the game
 * and each time the player moves to another floor.
 *
 * Overhauled on 12/1/2021. Overhaul was needed in order to make the generator
 * work with different room types and to make it more flexible overall.
 *
 * Other factors of the overhaul include refractoring my code to make it more readable
 * as well as cleaner. Overhauling wasn't possible during development as we had more
 * important items to attend to sadly.
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//Included this statement to help shorten code character count.
// using Random = UnityEngine.Random;

namespace Map
{
    /// <summary> The map generator that spawns in the rooms in a 6x6 grid. </summary>
    public class NewMapGenerator : MonoBehaviour
    {
        #region Fields
        #region Public
        public static NewMapGenerator Instance;

        public static List<Room> specialRooms;

        public NewGridInfo gridInfo = new NewGridInfo();

        //Was needed early on to simplify testing area.
        [Header("Enable this if we want to only have one row on the map.")]
        public bool OneRowOnly;
        #endregion

        #region Private
        /// <summary>
        /// <para>
        /// Grid size is set as follows.
        /// </para>
        /// Max Y value will be set to gridSize.
        /// Max X value will be set to gridSize + 1.
        /// <para>
        /// Grid pos set as follows: grid[y, x]
        /// </para>
        /// </summary>
        private Room[,] map;

        /// <summary>
        /// This array is used to lay out the conceptual framework of the map, and
        /// will be used to aid in choosing what rooms we actually need to spawn.
        /// Here we can also figure out later on what rooms will remain empty.
        /// </summary>
        private ConceptGrid conceptGrid;

        private int columns;

        private int rows;

        public RoomContainer roomCont;

        private Vector2 SpawnRoomPos, SpawnRoomGridPos;

        private Vector2 BossRoomPos, BossRoomGridPos;
        private Vector2 trueGridSize;
        #endregion
        #endregion

        public void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(Instance);

            Instance = this;

            if (specialRooms == null)
                specialRooms = new List<Room>();
            else if (specialRooms.Count > 0 && specialRooms[0] == null)
                specialRooms.Clear();

            if (roomCont == null)
                Debug.LogError("Room Container is empty. Please insert the scriptable object.");

        }

        public void Start()
        {
            trueGridSize = OneRowOnly
                ? new Vector2(gridInfo.gridSize + 1, 1)
                : new Vector2(gridInfo.gridSize + 1, gridInfo.gridSize);
            columns = (int) trueGridSize.x;
            rows = (int) trueGridSize.y;

            SpawnMap();
        }

        /// <summary> Function which spawns in all of the rooms on the map. </summary>
        private void SpawnMap()
        {
            map = new Room[(int) trueGridSize.y, (int) trueGridSize.x];
            conceptGrid = new ConceptGrid(trueGridSize, gridInfo);
            //NOTES:
            //1. Now we want to use the paths that were generated in the concept grid
            //to help us determine the rooms we want to spawn.
            //This will help maintain a lot of the current code structure that we have in place.
            //2. Levels need to be picked based on the number of openings they have on each side
            //by just comparing the room openings we can preplan certain rooms so we don't
            //have to delete and respawn prefabs.

            foreach (List<NewGridNode> path in conceptGrid.Paths)
            {
                CreatePath(path);
            }

            #region Spawning in filled rooms
            NewGridNode[,] tempConceptGrid = conceptGrid.grid;

            //Spawning a row
            for (int y = 0; y < rows; y++)
            {

                GameObject row = new GameObject("Row " + y)
                    { transform = { parent = transform } };

                //Spawning in a column
                for (int x = 0; x < columns; x++)
                {
                    if (map[y, x] != null && tempConceptGrid[y,x].roomType != (RoomType.Filled | RoomType.EMPTY)) continue;

                    GameObject tempFilledRoom = SpawnRoom(roomCont.filledRoom, new Vector2(x, y), "Filled Room");
                    
                    tempFilledRoom.transform.parent = row.transform;
                }
            }
            #endregion 
           
            #region Create Path Function
            // <summary>
            // Spawns the necessary rooms along a path by following the layout of the
            // conceptual grid.
            // </summary>
            // <param name="path">The list of nodes along a path on the grid.</param>
            void CreatePath(List<NewGridNode> path)
            {
                //Logic notes for spawning.
                //1. Each room has a certain number of openings each side
                //2. Each door is located in one of three positions on each side.
                //3. Rooms must match their neighbors in terms of the positioning of their
                //doorways that lead to the neighboring rooms.

                GameObject prevRoom = null;

                //Ok this needs to be redone, this isn't nice and just makes clutter.
                for (int pathIndex = 0; pathIndex < path.Count - 1; pathIndex++)
                {
                    NewGridNode currNode = path[pathIndex];

                    List<GameObject> examinedRooms = new List<GameObject>();
                    do
                    {
                        if (map[(int) currNode.gridPos.y, (int) currNode.gridPos.x] != null) break;
                        
                        #region Local Scope Variables
                        GameObject roomObj = GetRoom(currNode.roomType, examinedRooms);
                        
                        if (examinedRooms.Contains(roomObj.GetComponent<GameObject>()))
                            continue;

                        Room room = roomObj.GetComponent<Room>();
                        RoomInfo tempInfo = room.roomInfo;
                        string roomName = GetRoomName(currNode.roomType);
                        #endregion

                        examinedRooms.Add(roomObj);
                        
                        if (currNode.roomType == RoomType.Spawn)
                        {
                            SpawnRoom(roomObj, currNode.gridPos, roomName);
                            room.fogEnabledOnStart = false;
                            prevRoom = roomObj;
                            break;
                        }
                        else if (CompareNodeToRoom(currNode, tempInfo)
                            && EnsureEntrancesLineUp(tempInfo,
                                map[(int) path[pathIndex - 1].gridPos.y, (int) path[pathIndex - 1].gridPos.x].roomInfo,
                                GetPrevRoomDir(currNode.gridPos, path[pathIndex - 1].gridPos))
                        )
                        {
                            // Spawn the room
                            SpawnRoom(roomObj, currNode.gridPos, roomName);
                            prevRoom = roomObj;
                            break;
                        }

                        if (examinedRooms.Count == GetListCount(currNode.roomType))
                        {
                            Debug.LogError("Ran out of rooms of type " + currNode.roomType +
                                           ", about to enter infinite loop. Breaking from loop");
                            Debug.Log("The last room we looked at was " + prevRoom.name);

                            #if UNITY_EDITOR
                            UnityEditor.EditorApplication.isPlaying = false;
                            #else
                            Application.Quit();
                            #endif
                        }
        
                    } while (true);

                    examinedRooms.Clear();
                }
                
                #region Helper Functions

                GameObject GetRoom(RoomType type, List<GameObject> examinedRooms)
                {
                    GameObject room = null;

                    // while (true)
                    // {
                        switch (type)
                        {
                            case RoomType.Normal:
                                room = roomCont.RegularRooms[Random.Range(0, roomCont.RegularRooms.Count)];
                                break;
                            case RoomType.Spawn:
                                room = roomCont.SpawnRooms[Random.Range(0, roomCont.SpawnRooms.Count)];
                                break;
                            case RoomType.Boss:
                                room = roomCont.SpawnRooms[Random.Range(0, roomCont.BossRooms.Count)];
                                break;
                            case RoomType.Shop:
                                room = roomCont.ShopRooms[Random.Range(0, roomCont.ShopRooms.Count)];
                                break;
                        }
                    //     
                    //     if (!examinedRooms.Contains(room)) break;
                    // }

                    Debug.Log("Room found");

                    return room;
                }

                int GetListCount(RoomType type)
                {
                    switch (type)
                    {
                        case RoomType.Spawn:
                            return roomCont.SpawnRooms.Count;
                        case RoomType.Boss:
                            return roomCont.BossRooms.Count;
                        case RoomType.Shop:
                            return roomCont.BossRooms.Count;
                        default:
                            return roomCont.RegularRooms.Count;
                    }
                }

                string GetRoomName(RoomType type)
                {
                    switch (type)
                    {
                        case RoomType.Spawn:
                            return "Spawn Room";
                        case RoomType.Boss:
                            return "Boss Room";
                        case RoomType.Shop:
                            return "Shop Room";
                        case RoomType.Filled:
                            return "Filled Room";
                        default:
                            return "Room";
                    }
                }
                
                bool CompareNodeToRoom(NewGridNode node, RoomInfo roomInfo)
                {
                    if (node == null) Debug.LogError("Help node null");
                    //node.openings.ToString();
                    //roomInfo.ToString();
                    return node.openings.TotalOpenings() == roomInfo.numOpenings &&
                           node.openings.RightSide == roomInfo.CalcDoorsRightSide() &&
                           node.openings.LeftSide == roomInfo.CalcDoorsLeftSide() &&
                           node.openings.TopSide == roomInfo.CalcDoorsTopSide() &&
                           node.openings.BottomSide == roomInfo.CalcDoorsBottomSide();
                }

                bool EnsureEntrancesLineUp(RoomInfo currRoom, RoomInfo prevRoom, Dir prevDir)
                {
                    bool lineUp = false;

                    if (currRoom == null) Debug.LogError("CurrRoomInfo is null");

                    if (prevRoom == null) Debug.LogError("PrevRoomInfo is null");

                    foreach (DoorPositions currPos in currRoom.openDoors)
                    {
                        foreach (DoorPositions prevPos in prevRoom.openDoors)
                        {
                            switch (prevDir)
                            {
                                case Dir.Left:
                                {
                                    //Curr must have openings on left
                                    if ((prevPos == DoorPositions.RightTop && currPos == DoorPositions.LeftTop) ||
                                        (prevPos == DoorPositions.RightMiddle && currPos == DoorPositions.LeftMiddle) ||
                                        (prevPos == DoorPositions.RightBottom && currPos == DoorPositions.LeftBottom))
                                    {
                                        lineUp = true;
                                    }
                                    break;
                                }
                                case Dir.Right:
                                {
                                    //Curr must have openings on right
                                    if ((prevPos == DoorPositions.LeftTop && currPos == DoorPositions.RightTop) ||
                                        (prevPos == DoorPositions.LeftMiddle && currPos == DoorPositions.RightMiddle) ||
                                        (prevPos == DoorPositions.LeftBottom && currPos == DoorPositions.RightBottom))
                                    {
                                        lineUp = true;
                                    }
                                    break;
                                }
                                case Dir.Below:
                                {
                                    //Curr must have openings on bottom
                                    if ((prevPos == DoorPositions.TopLeft && currPos == DoorPositions.BottomLeft) ||
                                        (prevPos == DoorPositions.TopMiddle && currPos == DoorPositions.BottomMiddle) ||
                                        (prevPos == DoorPositions.TopRight && currPos == DoorPositions.BottomRight))
                                    {
                                        lineUp = true;
                                    }
                                    break;
                                }
                                default:
                                {
                                    // Debug.Log("Prev room above current");
                                    // Debug.Log(prevPos.ToString());
                                    //Curr must have openings on top
                                    if ((prevPos == DoorPositions.BottomLeft && currPos == DoorPositions.TopLeft) ||
                                        (prevPos == DoorPositions.BottomMiddle && currPos == DoorPositions.TopMiddle) ||
                                        (prevPos == DoorPositions.BottomRight && currPos == DoorPositions.TopRight))
                                    {
                                        lineUp = true;
                                    }
                                    break;
                                }
                            }
                            if (lineUp) break;
                        }
                    }
                    return lineUp;
                }

                Dir GetPrevRoomDir(Vector2 currRoomPos, Vector2 prevRoomPos)
                {
                    //Curr room to right of prev room so the prev room dir is left
                    if (currRoomPos.x > prevRoomPos.x) return Dir.Left;

                    if (currRoomPos.x < prevRoomPos.x) return Dir.Right;
                    
                    return currRoomPos.y > prevRoomPos.y ? Dir.Below : Dir.Top;
                }
                #endregion

            }
            #endregion
        }

        /// <summary> 
        /// This function will spawn in the room, assign it to the grid, 
        /// and assign the rest of the values. 
        /// </summary>
        /// <param name="room">The room prefab that will be spawned in on the map.</param>
        /// <param name="gridCord">The grid coordinate for the <paramref name="room"/>.</param>
        /// <param name="name">The name of the room that will be spawned. The rooms grid coords are also
        /// included in the name automatically.</param>
        private GameObject SpawnRoom(GameObject room, Vector2 gridCord, string name = "")
        {
            if (map[(int) gridCord.y, (int) gridCord.x] != null) return null;

            GameObject spawnedRoom = Instantiate(room, new Vector3(gridCord.x * NewGridInfo.RoomSize, gridCord.y * NewGridInfo.RoomSize),
                Quaternion.identity);

            spawnedRoom.GetComponent<Room>().gridPos = gridCord;

            map[(int) gridCord.y, (int) gridCord.x] = spawnedRoom.GetComponent<Room>();

            if (!name.Equals(""))
            {
                spawnedRoom.name = name + ": " + gridCord;
                switch (name)
                {
                    case "Spawn Room":
                        SpawnRoomGridPos = gridCord;
                        SpawnRoomPos = spawnedRoom.transform.position;
                        break;
                    case "Boss Room":
                        BossRoomGridPos = gridCord;
                        BossRoomPos = spawnedRoom.transform.position;
                        break;
                }
            }

            return spawnedRoom;
        }


        private void AddSpecialRoomToList(GameObject roomObj) => specialRooms.Add(roomObj.GetComponent<Room>());

        public void ExposeSpecialRooms()
        {
            foreach (Room room in specialRooms) room.TurnOffFog();
        }

        /// <summary> A small directional enum for aiding the algorithm. </summary>
        enum Dir
        {
            Top,
            Below,
            Left,
            Right
        }
    }
}

[Flags]
public enum RoomType
{
    EMPTY,
    Normal,
    Spawn,
    Boss,
    Shop,
    Filled
}